using System;
using System.Reactive;

namespace GameCtor.XamarinAuth
{
    public interface IAuthService
    {
        IObservable<string> SignInSuccessful { get; }

        IObservable<Unit> SignInCanceled { get; }

        IObservable<Exception> SignInFailed { get; }

        void TriggerGoogleAuthFlow();

        void TriggerFacebookAuthFlow();
    }
}