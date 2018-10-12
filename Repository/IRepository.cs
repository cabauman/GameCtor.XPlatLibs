using System;
using System.Collections.Generic;
using System.Reactive;

namespace GameCtor.Repository
{
    public interface IRepository<T>
    {
        IObservable<Unit> Add(T item);

        IObservable<Unit> Add(IEnumerable<T> items);

        IObservable<Unit> Upsert(T item);

        IObservable<Unit> Upsert(IEnumerable<T> items);

        IObservable<Unit> Delete(string id);

        IObservable<Unit> Delete(IEnumerable<T> items);

        IObservable<T> GetItem(string id);

        IObservable<IEnumerable<T>> GetItems(bool fetchOnline = false);

        //IObservable<RepoItemCollection<T>> GetItems(int cursor = 0, int limit = 1000, bool fetchOnline = false);
    }
}
