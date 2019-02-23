using System;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using GameCtor.LocalStorage;

namespace LocalStorage.XamarinEssentials
{
    public class SecureStorage : ILocalStorageService
    {
        /// <inheritdoc/>
        public IObservable<string> Get(string key)
        {
            return Xamarin.Essentials.SecureStorage
                .GetAsync(key)
                .ToObservable();
        }

        /// <inheritdoc/>
        public IObservable<Unit> Set(string key, string value)
        {
            return Xamarin.Essentials.SecureStorage
                .SetAsync(key, value)
                .ToObservable();
        }

        /// <inheritdoc/>
        public bool Remove(string key)
        {
            return Xamarin.Essentials.SecureStorage.Remove(key);
        }

        /// <inheritdoc/>
        public void RemoveAll()
        {
            Xamarin.Essentials.SecureStorage.RemoveAll();
        }
    }
}
