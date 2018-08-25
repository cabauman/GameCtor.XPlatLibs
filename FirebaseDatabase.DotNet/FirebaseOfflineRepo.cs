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

        public FirebaseOfflineRepo(FirebaseClient client, string path, string key = "")
        {
            // The offline database filename is named after type T.
            // So, if you have more than one list of type T objects, you need to differentiate it
            // by adding a filename modifier; which is what we're using the "key" parameter for.
            _baseQuery = client.Child(path);
            _realtimeDb = _baseQuery
                .AsRealtimeDatabase<T>(key, string.Empty, StreamingOptions.Everything, InitialPullStrategy.MissingOnly, true);
        }

        public IObservable<Unit> Add(T item)
        {
            return Observable
                .Start(() => _realtimeDb.Post(item))
                .Do(itemKey => item.Id = itemKey)
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> Add(IEnumerable<T> items)
        {
            // Doesn't work offline. Need offline solution.
            return _baseQuery
                .PatchAsync(items.ToDictionary(_ => FirebaseKeyGenerator.Next()))
                .ToObservable()
                .Concat(_realtimeDb.PullAsync().ToObservable());
        }

        public IObservable<Unit> Delete(string id)
        {
            return Observable
                .Start(() => _realtimeDb.Delete(id));
        }

        public IObservable<T> GetItem(string id)
        {
            return Observable
                .Start(() => _realtimeDb.Database[id].Deserialize<T>());
        }

        public IObservable<IEnumerable<T>> GetItems(bool fetchOnline = false)
        {
            return Observable
                .Return(fetchOnline ? Pull() : Observable.Return(Unit.Default))
                .SelectMany(x => x)
                .SelectMany(_ => _realtimeDb.Once())
                .Do(MapKeyToId)
                .Select(x => x.Object)
                .ToList();
        }

        public IObservable<RepoItemCollection<T>> GetItems(int cursor = 0, int count = 1000, bool forceRefresh = false)
        {
            return Observable
                .Return(forceRefresh ? Pull() : Observable.Return(Unit.Default))
                .SelectMany(x => x)
                .Select(_ => _realtimeDb.Database.Values)
                .Do(
                    x =>
                    {
                        cursor += count;
                        if(cursor >= x.Count)
                        {
                            cursor = -1;
                        };
                    })
                .SelectMany(x => x)
                .Skip(cursor)
                .Take(count)
                .Where(kvp => !string.IsNullOrEmpty(kvp.Data) && kvp.Data != "null" && !kvp.IsPartial)
                .Select(x => x.Deserialize<T>())
                .ToList()
                .Select(x => new RepoItemCollection<T>(cursor, x));
        }

        public IObservable<Unit> Update(T item)
        {
            return Observable
                .Start(() => _realtimeDb.Put(item.Id, item));
        }

        public IObservable<T> Observe()
        {
            _realtimeDb.SyncExceptionThrown += (s, ex) => Console.WriteLine(ex.Exception);

            return _realtimeDb
                .AsObservable()
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        public IObservable<Unit> Upsert(IEnumerable<T> items)
        {
            // Doesn't work offline. Need offline solution.
            return _baseQuery
                .PatchAsync(items.ToDictionary(x => x.Id))
                .ToObservable()
                .Concat(_realtimeDb.PullAsync().ToObservable());
        }

        // May need to modify Json options for this to work right.
        public IObservable<Unit> Delete(IEnumerable<T> items)
        {
            // Doesn't work offline. Need offline solution.
            return _baseQuery
                .PatchAsync(items.ToDictionary(x => x.Id, x => default(string)))
                .ToObservable()
                .Concat(_realtimeDb.PullAsync().ToObservable());
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
