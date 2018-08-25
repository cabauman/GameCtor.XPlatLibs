using System;
using System.Collections.Generic;
using System.Reactive;

namespace GameCtor.Repository
{
    public interface IRepository<T>
        where T : IModel
    {
        IObservable<Unit> Add(T item);

        IObservable<Unit> Update(T item);

        IObservable<Unit> Delete(string id);
         
        IObservable<T> GetItem(string id);

        IObservable<IEnumerable<T>> GetItems(bool fetchOnline = false);

        //IObservable<RepoItemCollection<T>> GetItems(int cursor = 0, int limit = 1000, bool fetchOnline = false);
    }
}
