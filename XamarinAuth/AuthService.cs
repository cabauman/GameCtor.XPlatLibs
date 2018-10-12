using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xamarin.Auth;

namespace GameCtor.XamarinAuth
{
    public partial class AuthService : IAuthService
    {
        private string _provider;
        private Subject<Unit> _authFlowTriggered;

        public AuthService()
        {
            _authFlowTriggered = new Subject<Unit>();

            var authCompleted = _authFlowTriggered
                .Select(
                    _ =>
                    {
                        return Observable.FromEventPattern<AuthenticatorCompletedEventArgs>(
                            x => AuthenticationState.Authenticator.Completed += x,
                            x => AuthenticationState.Authenticator.Completed -= x);
                    })
                .Switch();

            SignInCanceled = authCompleted
                .Where(x => !x.EventArgs.IsAuthenticated)
                .Select(_ => Unit.Default);

            SignInSuccessful = authCompleted
                .Where(x => x.EventArgs.IsAuthenticated)
                .Select(x => ExtractAuthToken(x.EventArgs.Account));

            SignInFailed = _authFlowTriggered
                .Select(
                    _ =>
                    {
                        return Observable.FromEventPattern<AuthenticatorErrorEventArgs>(
                            x => AuthenticationState.Authenticator.Error += x,
                            x => AuthenticationState.Authenticator.Error -= x)
                                .Select(x => x.EventArgs.Exception);
                    })
                .Switch();
        }

        public IObservable<string> SignInSuccessful { get; private set; }

        public IObservable<Unit> SignInCanceled { get; private set; }

        public IObservable<Exception> SignInFailed { get; private set; }

        public void TriggerGoogleAuthFlow(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl)
        {
            _provider = "google";
            TriggerOAuth2Flow(clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl);
        }

        public void TriggerFacebookAuthFlow(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl)
        {
            _provider = "facebook";
            TriggerOAuth2Flow(clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl);
        }

        public void TriggerOAuth2Flow(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl)
        {
            if(string.IsNullOrEmpty(accessTokenUrl))
            {
                AuthenticationState.Authenticator = new OAuth2Authenticator(
                    clientId,
                    scope,
                    new Uri(authorizeUrl),
                    new Uri(redirectUrl),
                    getUsernameAsync: null,
                    isUsingNativeUI: true);
            }
            else
            {
                AuthenticationState.Authenticator = new OAuth2Authenticator(
                    clientId,
                    clientSecret,
                    scope,
                    new Uri(authorizeUrl),
                    new Uri(redirectUrl),
                    new Uri(accessTokenUrl),
                    getUsernameAsync: null,
                    isUsingNativeUI: true);
            }

            _authFlowTriggered.OnNext(Unit.Default);
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(AuthenticationState.Authenticator);
        }

        public void TriggerOAuth1Flow(OAuth1Config config)
        {
            AuthenticationState.Authenticator = new OAuth1Authenticator(
                config.ConsumerKey,
                config.ConsumerSecret,
                new Uri(config.RequestTokenUrl),
                new Uri(config.AuthorizeUrl),
                new Uri(config.AccessTokenUrl),
                new Uri(config.CallbackUrl),
                getUsernameAsync: null,
                isUsingNativeUI: false);

            _authFlowTriggered.OnNext(Unit.Default);
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(AuthenticationState.Authenticator);
        }

        public void GetAccountInfo(string provider)
        {
            var account = Xamarin.Auth.AccountStore
                .Create()
                .FindAccountsForService(provider)
                .FirstOrDefault();
        }

        private string ExtractAuthToken(Xamarin.Auth.Account account)
        {
            Xamarin.Auth.AccountStore
                .Create()
                .Save(account, _provider);

            string authToken = account.Properties["access_token"];
            if(AuthenticationState.Authenticator.GetType() == typeof(OAuth1Authenticator))
            {
                authToken = account.Properties["oauth_token"];
            }

            return authToken;
        }
    }
}
