using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Firebase.Auth;
using GameCtor.LocalStorage;
using Newtonsoft.Json;

namespace GameCtor.FirebaseAuth.DotNet
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private const string FIREBASE_AUTH_JSON_KEY = "FIREBASE_AUTH_JSON";

        private readonly FirebaseAuthProvider _authProvider;
        private readonly ILocalStorageService _localStorageService;

        private FirebaseAuthLink _authLink;

        public bool IsAuthenticated => _authLink != null;

        public bool IsPhoneNumberLinkedToAccount => throw new NotImplementedException();

        public IObservable<Firebase.Auth.FirebaseAuth> FirebaseAuthRefreshed
        {
            get
            {
                return Observable
                    .FromEventPattern<FirebaseAuthEventArgs>(
                        x => _authLink.FirebaseAuthRefreshed += x,
                        x => _authLink.FirebaseAuthRefreshed -= x)
                    .Select(x => x.EventArgs.FirebaseAuth);
            }
        }

        public FirebaseAuthService(string apiKey, ILocalStorageService localStorageService)
        {
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));

            localStorageService.Get(FIREBASE_AUTH_JSON_KEY)
                .Where(x => x != null)
                .Subscribe(
                    authJson =>
                    {
                        var auth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(authJson);
                        _authLink = new FirebaseAuthLink(_authProvider, auth);
                    });

            FirebaseAuthRefreshed
                .Select(firebaseAuth => SaveAccount(firebaseAuth))
                .Subscribe();
        }

        public async Task<string> GetIdTokenAsync()
        {
            return (await _authLink.GetFreshAuthAsync()).FirebaseToken;
        }

        public IObservable<PhoneNumberVerificationResult> LinkPhoneNumberToCurrentUser(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> LinkPhoneNumberToCurrentUser(string verificationId, string verificationCode)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SignInAnonymously()
        {
            return _authProvider
                .SignInAnonymouslyAsync()
                .ToObservable()
                .Do(authLink => _authLink = authLink)
                .SelectMany(authLink => SaveAccount(authLink));
        }

        public IObservable<Unit> SignInWithFacebook(string authToken)
        {
            return SignInWithOAuth(FirebaseAuthType.Facebook, authToken);
        }

        public IObservable<Unit> SignInWithGoogle(string authToken)
        {
            return SignInWithOAuth(FirebaseAuthType.Google, authToken);
        }

        public IObservable<PhoneNumberVerificationResult> SignInWithPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode)
        {
            throw new NotImplementedException();
        }

        public void SignOut()
        {
            _authLink = null;
            _localStorageService.RemoveAll();
        }

        private IObservable<Unit> SignInWithOAuth(FirebaseAuthType authType, string authToken)
        {
            return _authProvider
                .SignInWithOAuthAsync(authType, authToken)
                .ToObservable()
                .Do(authLink => _authLink = authLink)
                .SelectMany(authLink => SaveAccount(authLink));
        }

        private IObservable<Unit> SaveAccount(Firebase.Auth.FirebaseAuth firebaseAuth)
        {
            string json = JsonConvert.SerializeObject(firebaseAuth);
            return _localStorageService.Set(FIREBASE_AUTH_JSON_KEY, json);
        }
    }
}