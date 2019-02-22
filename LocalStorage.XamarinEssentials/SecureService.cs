using System;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using GameCtor.LocalStorage;

namespace LocalStorage.XamarinEssentials
{
    public class SecureStorage : ILocalStorageService
    {
        public IObservable<string> Get(string key)
        {
            return Xamarin.Essentials.SecureStorage
                .GetAsync(key)
                .ToObservable();
        }

        public IObservable<Unit> Set(string key, string value)
        {
            return Xamarin.Essentials.SecureStorage
                .SetAsync(key, value)
                .ToObservable();
        }

        public bool Remove(string key)
        {
            return Xamarin.Essentials.SecureStorage.Remove(key);
        }

        public void RemoveAll()
        {
            Xamarin.Essentials.SecureStorage.RemoveAll();
        }
    }
}
