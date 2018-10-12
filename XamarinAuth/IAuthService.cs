using System;
using System.Reactive;

namespace GameCtor.XamarinAuth
{
    public interface IAuthService
    {
        IObservable<string> SignInSuccessful { get; }

        IObservable<Unit> SignInCanceled { get; }

        IObservable<Exception> SignInFailed { get; }

        void TriggerGoogleAuthFlow(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl);

        void TriggerFacebookAuthFlow(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl);
    }
}