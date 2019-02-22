using System;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.LocalStorage;

namespace LocalStorage.XamarinEssentials
{
    public class Preferences : ILocalStorageService
    {
        public IObservable<string> Get(string key)
        {
            return Observable.Return(
                Xamarin.Essentials.Preferences.Get(key, null));
        }

        public IObservable<Unit> Set(string key, string value)
        {
            return Observable
                .Return(Unit.Default)
                .Do(_ => Xamarin.Essentials.Preferences.Set(key, value));
        }

        public bool Remove(string key)
        {
            Xamarin.Essentials.Preferences.Remove(key);
            return true;
        }

        public void RemoveAll()
        {
            Xamarin.Essentials.Preferences.Clear();
        }
    }
}
