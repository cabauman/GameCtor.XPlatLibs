using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using GameCtor.Firebase.AuthWrapper;

namespace GameCtor.FirebaseAuth.Mobile
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        public FirebaseAuthService()
        {
        }

        public bool IsAuthenticated
        {
            get { return CrossFirebaseAuth.Current.CurrentUser != null; }
        }

        public bool IsPhoneNumberLinkedToAccount
        {
            get
            {
                return false;
            }
        }

        public async Task<string> GetIdTokenAsync()
        {
            return await CrossFirebaseAuth.Current.CurrentUser.GetIdTokenAsync(false);
        }

        public IObservable<Unit> SignInWithFacebook(string authToken)
        {
            return CrossFirebaseAuth.Current.SignInWithFacebookAsync(authToken)
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInWithGoogle(string authToken)
        {
            return CrossFirebaseAuth.Current.SignInWithGoogleAsync(null, authToken)
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInAnonymously()
        {
            return CrossFirebaseAuth.Current.SignInAnonymouslyAsync()
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<PhoneNumberVerificationResult> SignInWithPhoneNumber(string phoneNumber)
        {
            return Observable
                .Return(new PhoneNumberVerificationResult(false, "123456"))
                .Delay(TimeSpan.FromSeconds(1));

            return CrossFirebaseAuth.Current.SignInWithPhoneNumberAsync(phoneNumber)
                .ToObservable()
                .Select(
                    x =>
                    {
                        return new PhoneNumberVerificationResult(x.AuthResult != null, x.VerificationId);
                    });
        }

        public IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode)
        {
            return Observable
                .Return(Unit.Default)
                .Delay(TimeSpan.FromSeconds(1));

            return CrossFirebaseAuth.Current.SignInWithPhoneNumberAsync(verificationId, verificationCode)
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<PhoneNumberVerificationResult> LinkPhoneNumberToCurrentUser(string phoneNumber)
        {
            return Observable
                .Return(new PhoneNumberVerificationResult(false, "123456"))
                .Delay(TimeSpan.FromSeconds(1));

            return CrossFirebaseAuth.Current.LinkPhoneNumberWithCurrentUserAsync(phoneNumber)
                .ToObservable()
                .Select(
                    x =>
                    {
                        return new PhoneNumberVerificationResult(x.AuthResult != null, x.VerificationId);
                    });
        }

        public IObservable<Unit> LinkPhoneNumberToCurrentUser(string verificationId, string verificationCode)
        {
            return Observable
                .Return(Unit.Default)
                .Delay(TimeSpan.FromSeconds(1));

            return CrossFirebaseAuth.Current.CurrentUser.LinkWithPhoneNumberAsync(verificationId, verificationCode)
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public void SignOut()
        {
            CrossFirebaseAuth.Current.SignOut();
        }
    }
}