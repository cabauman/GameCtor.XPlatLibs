using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using GameCtor.Repository;

namespace GameCtor.FirebaseDatabase.DotNet
{
    public class FirebaseRepo<T> : IRepository<T>
        where T : class, IModel
    {
        private ChildQuery _childQuery;

        public FirebaseRepo(FirebaseClient client, string path)
        {
            _childQuery = client.Child(path);
        }

        public IObservable<Unit> Add(T item)
        {
            return _childQuery
                .PostAsync(item)
                .ToObservable()
                .Do(MapKeyToId)
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> Add(IEnumerable<T> items)
        {
            return _childQuery
                .PatchAsync(items.ToDictionary(_ => FirebaseKeyGenerator.Next()))
                .ToObservable();
        }

        public IObservable<Unit> Upsert(T item)
        {
            return _childQuery
                .Child(item.Id)
                .PutAsync(item)
                .ToObservable();
        }

        public IObservable<Unit> Upsert(IEnumerable<T> items)
        {
            return _childQuery
                .PatchAsync(items.ToDictionary(x => x.Id))
                .ToObservable();
        }

        public IObservable<Unit> Delete(string id)
        {
            return _childQuery
                .Child(id)
                .DeleteAsync()
                .ToObservable();
        }

        public IObservable<Unit> Delete(IEnumerable<T> items)
        {
            return _childQuery
                .PatchAsync(items.ToDictionary(x => x.Id, x => default(string)))
                .ToObservable();
        }

        public IObservable<T> GetItem(string id)
        {
            return _childQuery
                .Child(id)
                .OnceSingleAsync<T>()
                .ToObservable()
                .Do(item => item.Id = id);
        }

        public IObservable<IEnumerable<T>> GetItems(bool fetchOnline = false)
        {
            return _childQuery
                .OnceAsync<T>()
                .ToObservable()
                .SelectMany(x => x)
                .Do(MapKeyToId)
                .Select(x => x.Object)
                .ToList();
        }

        // Will handle paging at a later time
        //public IObservable<RepoItemCollection<T>> GetItems(int cursor = 0, int count = 1000, bool fetchOnline = false)
        //{
        //    return _childQuery
        //        .OrderByKey()
        //        .StartAt(cursor.ToString())
        //        .LimitToFirst(count)
        //        .OnceAsync<T>()
        //        .ToObservable()
        //        .SelectMany(x => x)
        //        .Skip(cursor > -1 ? 1 : 0)
        //        .Do(MapKeyToId)
        //        .Select(x => x.Object)
        //        .ToList()
        //        .Select(x => new RepoItemCollection<T>(int.Parse(x[x.Count - 1].Id), x));
        //}

        public IObservable<T> Observe()
        {
            return _childQuery
                .AsObservable<T>()
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        public IObservable<Unit> Populate(IDictionary<string, T> idToItemDict)
        {
            return _childQuery
                .PutAsync(idToItemDict)
                .ToObservable();
        }

        private void MapKeyToId(FirebaseObject<T> firebaseObj)
        {
            firebaseObj.Object.Id = firebaseObj.Key;
        }
    }
}
