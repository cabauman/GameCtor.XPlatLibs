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
    public class FirebaseAuthService : IFirebaseAuthService, IDisposable
    {
        private const string FIREBASE_AUTH_JSON_KEY = "FIREBASE_AUTH_JSON";

        private readonly FirebaseAuthProvider _authProvider;
        private readonly ILocalStorageService _localStorageService;
        private readonly IObservable<Firebase.Auth.FirebaseAuth> _whenAuthRefreshed;
        private readonly IDisposable _accountSavedSubscription;

        private FirebaseAuthLink _authLink;

        public FirebaseAuthService(string apiKey, ILocalStorageService localStorageService)
        {
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            _localStorageService = localStorageService;

            _localStorageService
                .Get(FIREBASE_AUTH_JSON_KEY)
                .Where(x => x != null)
                .Subscribe(
                    authJson =>
                    {
                        var auth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(authJson);
                        _authLink = new FirebaseAuthLink(_authProvider, auth);
                    });

            _whenAuthRefreshed = Observable
                .FromEventPattern<FirebaseAuthEventArgs>(
                    x => _authLink.FirebaseAuthRefreshed += x,
                    x => _authLink.FirebaseAuthRefreshed -= x)
                .Select(x => x.EventArgs.FirebaseAuth);

            _accountSavedSubscription = _whenAuthRefreshed
                .Select(firebaseAuth => SaveAccount(firebaseAuth))
                .Subscribe();
        }

        public IFirebaseUser CurrentUser => _authLink != null ? new FirebaseUser(_authLink) : null;

        public async Task<string> GetIdTokenAsync()
        {
            return (await _authLink.GetFreshAuthAsync()).FirebaseToken;
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

        public IObservable<Unit> SignInWithGoogle(string idToken, string authToken)
        {
            return SignInWithOAuth(FirebaseAuthType.Google, authToken);
        }

        public IObservable<Unit> SignInWithTwitter(string token, string secret)
        {
            return SignInWithOAuth(FirebaseAuthType.Twitter, token);
        }

        public IObservable<Unit> SignInWithGithub(string token)
        {
            return SignInWithOAuth(FirebaseAuthType.Github, token);
        }

        public IObservable<Unit> SignInWithEmail(string email, string password)
        {
            return _authProvider
                .SignInWithEmailAndPasswordAsync(email, password)
                .ToObservable()
                .Do(authLink => _authLink = authLink)
                .SelectMany(authLink => SaveAccount(authLink));
        }

        public IObservable<PhoneNumberSignInResult> SignInWithPhoneNumber(string phoneNumber)
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
            _localStorageService.Remove(FIREBASE_AUTH_JSON_KEY);
        }

        public void Dispose()
        {
            _accountSavedSubscription.Dispose();
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