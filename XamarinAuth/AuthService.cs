using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xamarin.Auth;

namespace GameCtor.XamarinAuth
{
    public partial class AuthService : IAuthService
    {
        private Subject<Unit> _authFlowTriggered;
        private OAuth2Config _googleConfig;
        private OAuth2Config _facebookConfig;

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

        public void InitGoogleConfig(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl)
        {
            _googleConfig = new OAuth2Config()
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope,
                AuthorizeUrl = authorizeUrl,
                RedirectUrl = redirectUrl,
                AccessTokenUrl = accessTokenUrl
            };
        }

        public void InitFacebookConfig(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl)
        {
            _facebookConfig = new OAuth2Config()
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope,
                AuthorizeUrl = authorizeUrl,
                RedirectUrl = redirectUrl,
                AccessTokenUrl = accessTokenUrl
            };
        }

        private void TriggerOAuth2Flow(OAuth2Config config)
        {
            AuthenticationState.Authenticator = new OAuth2Authenticator(
                config.ClientId,
                config.ClientSecret,
                config.Scope,
                new Uri(config.AuthorizeUrl),
                new Uri(config.RedirectUrl),
                new Uri(config.AccessTokenUrl),
                getUsernameAsync: null,
                isUsingNativeUI: true);

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

        public void TriggerGoogleAuthFlow()
        {
            TriggerOAuth2Flow(_googleConfig);
        }

        public void TriggerFacebookAuthFlow()
        {
            TriggerOAuth2Flow(_facebookConfig);
        }

        private string ExtractAuthToken(Xamarin.Auth.Account account)
        {
            string authToken = account.Properties["access_token"];
            if(AuthenticationState.Authenticator.GetType() == typeof(OAuth1Authenticator))
            {
                authToken = account.Properties["oauth_token"];
            }

            return authToken;
        }
    }
}
