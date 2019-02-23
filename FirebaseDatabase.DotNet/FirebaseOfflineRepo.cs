using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using GameCtor.Repository;

namespace GameCtor.FirebaseDatabase.DotNet
{
    public class FirebaseOfflineRepo<T> : IRepository<T>
        where T : class, IModel
    {
        private readonly RealtimeDatabase<T> _realtimeDb;
        private readonly ChildQuery _baseQuery;

        public FirebaseOfflineRepo(
            FirebaseClient client,
            string path,
            string key = "",
            StreamingOptions streaming = StreamingOptions.None,
            InitialPullStrategy initialPull = InitialPullStrategy.Everything)
        {
            // The offline database filename is named after type T.
            // So, if you have more than one list of type T objects, you need to differentiate it
            // by adding a filename modifier; which is what we're using the "key" parameter for.
            _baseQuery = client.Child(path);
            _realtimeDb = _baseQuery
                .AsRealtimeDatabase<T>(key, string.Empty, streaming, initialPull, true);

            SyncExceptionThrown = Observable
                .FromEventPattern<ExceptionEventArgs>(
                    h => _realtimeDb.SyncExceptionThrown += h,
                    h => _realtimeDb.SyncExceptionThrown -= h);
        }

        public IObservable<EventPattern<ExceptionEventArgs>> SyncExceptionThrown { get; }

        public IObservable<Unit> Add(T item)
        {
            return Observable
                .Start(() => _realtimeDb.Post(item))
                .Do(itemKey => item.Id = itemKey)
                .Select(_ => Unit.Default);
        }

        /// <inheritdoc/>
        public IObservable<Unit> Add(IEnumerable<T> items)
        {
            // Doesn't work offline. Need offline solution.
            return _baseQuery
                .PatchAsync(items.ToDictionary(x => x.Id = FirebaseKeyGenerator.Next()))
                .ToObservable()
                .SelectMany(_realtimeDb.PullAsync().ToObservable());
        }

        public IObservable<Unit> Add2(IEnumerable<T> items)
        {
            return items
                .ToObservable()
                .SelectMany(x => Add(x))
                .LastAsync();
        }

        /// <inheritdoc/>
        public IObservable<Unit> Upsert(T item)
        {
            return Observable
                .Start(() => _realtimeDb.Patch(item.Id, item));
        }

        /// <inheritdoc/>
        public IObservable<Unit> Upsert(IEnumerable<T> items)
        {
            // Doesn't work offline. Need offline solution.
            return _baseQuery
                .PatchAsync(items.ToDictionary(x => x.Id = x.Id ?? FirebaseKeyGenerator.Next()))
                .ToObservable()
                .SelectMany(_ => _realtimeDb.PullAsync().ToObservable());
        }

        /// <inheritdoc/>
        public IObservable<Unit> Delete(string id)
        {
            return Observable
                .Start(() => _realtimeDb.Delete(id));
        }

        /// <inheritdoc/>
        public IObservable<Unit> Delete(IEnumerable<T> items)
        {
            // Doesn't work offline. Need offline solution.
            return _baseQuery
                .PatchAsync(items.ToDictionary(x => x.Id, x => default(string)))
                .ToObservable()
                .Concat(_realtimeDb.PullAsync().ToObservable());
        }

        /// <inheritdoc/>
        public IObservable<T> GetItem(string id)
        {
            return Observable
                .Start(
                    () =>
                    {
                        _realtimeDb.Database.TryGetValue(id, out OfflineEntry item);
                        return item?.Deserialize<T>();
                    });
        }

        /// <inheritdoc/>
        public IObservable<IEnumerable<T>> GetItems(bool fetchOnline = false)
        {
            return Observable
                .Return(fetchOnline || _realtimeDb.Database.Count == 0 ? Pull() : Observable.Return(Unit.Default))
                .SelectMany(x => x)
                .SelectMany(_ => _realtimeDb.Once())
                .Do(MapKeyToId)
                .Select(x => x.Object)
                .ToList();
        }

        // Will handle paging at a later time
        //public IObservable<RepoItemCollection<T>> GetItems(int cursor = 0, int count = 1000, bool fetchOnline = false)
        //{
        //    return Observable
        //        .Return(fetchOnline || _realtimeDb.Database.Count == 0 ? Pull() : Observable.Return(Unit.Default))
        //        .SelectMany(x => x)
        //        .Select(_ => _realtimeDb.Database.Values)
        //        .Do(
        //            x =>
        //            {
        //                cursor += count;
        //                if(cursor >= x.Count)
        //                {
        //                    cursor = -1;
        //                };
        //            })
        //        .SelectMany(x => x)
        //        .Skip(cursor)
        //        .Take(count)
        //        .Where(kvp => !string.IsNullOrEmpty(kvp.Data) && kvp.Data != "null" && !kvp.IsPartial)
        //        .Select(x => x.Deserialize<T>())
        //        .ToList()
        //        .Select(x => new RepoItemCollection<T>(cursor, x));
        //}

        private static readonly Dictionary<Firebase.Database.Streaming.FirebaseEventSource, FirebaseEventSource> EventSourceMap = new Dictionary<Firebase.Database.Streaming.FirebaseEventSource, FirebaseEventSource>
        {
            { Firebase.Database.Streaming.FirebaseEventSource.Online, FirebaseEventSource.Online },
            { Firebase.Database.Streaming.FirebaseEventSource.OnlineInitial, FirebaseEventSource.Online },
            { Firebase.Database.Streaming.FirebaseEventSource.OnlinePull, FirebaseEventSource.Online },
            { Firebase.Database.Streaming.FirebaseEventSource.OnlinePush, FirebaseEventSource.Online },
            { Firebase.Database.Streaming.FirebaseEventSource.OnlineStream, FirebaseEventSource.Online },
            { Firebase.Database.Streaming.FirebaseEventSource.Offline, FirebaseEventSource.Offline },
        };

        private static readonly Dictionary<Firebase.Database.Streaming.FirebaseEventType, FirebaseEventType> EventTypeMap = new Dictionary<Firebase.Database.Streaming.FirebaseEventType, FirebaseEventType>
        {
            { Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate, FirebaseEventType.AddOrUpdate },
            { Firebase.Database.Streaming.FirebaseEventType.Delete, FirebaseEventType.Delete },
        };

        public IObservable<FirebaseEvent<T>> Observe()
        {
            return _realtimeDb
                .AsObservable()
                .Do(MapKeyToId)
                .Select(
                    x =>
                    {
                        EventSourceMap.TryGetValue(x.EventSource, out var eventSource);
                        EventTypeMap.TryGetValue(x.EventType, out var eventType);
                        return new FirebaseEvent<T>(x.Key, x.Object, eventSource, eventType);
                    });
        }

        private IObservable<Unit> Pull()
        {
            return _realtimeDb
                .PullAsync()
                .ToObservable();
        }

        private void MapKeyToId(FirebaseObject<T> firebaseObj)
        {
            firebaseObj.Object.Id = firebaseObj.Key;
        }
    }
}
