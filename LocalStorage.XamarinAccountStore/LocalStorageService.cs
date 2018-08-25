using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using GameCtor.LocalStorage;

namespace LocalStorage.XamarinAccountStore
{
    public class LocalStorageService : ILocalStorageService
    {
        private const string DEFAULT_SERVICE_ID = "DEFAULT_SERVICE_ID";

        private Xamarin.Auth.Account _account;

        public LocalStorageService()
        {
            _account = Xamarin.Auth.AccountStore
                .Create()
                .FindAccountsForService(DEFAULT_SERVICE_ID)
                .FirstOrDefault();

            if(_account == null)
            {
                _account = new Xamarin.Auth.Account(username: null, properties: new Dictionary<string, string>());
                Xamarin.Auth.AccountStore
                    .Create()
                    .Save(_account, DEFAULT_SERVICE_ID);
            }
        }

        public IObservable<string> Get(string key)
        {
            _account.Properties.TryGetValue(key, out string value);
            return Observable.Return(value);
        }

        public IObservable<Unit> Set(string key, string value)
        {
            _account.Properties[key] = value;

            return Xamarin.Auth.AccountStore
                .Create()
                .SaveAsync(_account, DEFAULT_SERVICE_ID)
                .ToObservable();
        }

        public bool Remove(string key)
        {
            if(key == null) return false;

            _account.Properties.Remove(key);
            Xamarin.Auth.AccountStore
                .Create()
                .Save(_account, DEFAULT_SERVICE_ID);

            return true;
        }

        public void RemoveAll()
        {
            Xamarin.Auth.AccountStore
                .Create()
                .Delete(_account, DEFAULT_SERVICE_ID);
        }
    }
}
