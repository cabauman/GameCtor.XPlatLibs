using System;
using System.Reactive;

namespace GameCtor.LocalStorage
{
    public interface ILocalStorageService
    {
        IObservable<string> Get(string key);

        IObservable<Unit> Set(string key, string value);

        bool Remove(string key);

        void RemoveAll();
    }
}