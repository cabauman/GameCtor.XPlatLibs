using Android.App;
using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.Mobile
{
    /// <summary>
    /// Main implementation for IFirebaseAuth
    /// </summary>
    public partial class FirebaseAuthService : IFirebaseAuthService
    {
        internal static IDictionary<Type, FirebaseAuthExceptionType> FirebaseExceptionTypeToEnumDict { get; } = new Dictionary<Type, FirebaseAuthExceptionType>
        {
            { typeof(FirebaseAuthActionCodeException), FirebaseAuthExceptionType.FirebaseAuthActionCode },
            { typeof(FirebaseAuthRecentLoginRequiredException), FirebaseAuthExceptionType.FirebaseAuthRecentLoginRequired },
            { typeof(FirebaseAuthWeakPasswordException), FirebaseAuthExceptionType.FirebaseAuthWeakPassword },
            { typeof(FirebaseAuthInvalidCredentialsException), FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { typeof(FirebaseAuthInvalidUserException), FirebaseAuthExceptionType.FirebaseAuthInvalidUser },
            { typeof(FirebaseAuthUserCollisionException), FirebaseAuthExceptionType.FirebaseAuthUserCollision },
            { typeof(FirebaseAuthEmailException), FirebaseAuthExceptionType.FirebaseAuthEmail },
            { typeof(FirebaseApiNotAvailableException), FirebaseAuthExceptionType.FirebaseApiNotAvailable },
            { typeof(FirebaseTooManyRequestsException), FirebaseAuthExceptionType.FirebaseTooManyRequests },
        };

        private Firebase.Auth.FirebaseAuth _auth;

        public FirebaseAuthService()
        {
            _auth = Firebase.Auth.FirebaseAuth.Instance;
        }

        public IFirebaseUser CurrentUser => _auth.CurrentUser != null ? new FirebaseUser(_auth.CurrentUser) : null;

        public IObservable<bool> IsAuthenticated => throw new NotImplementedException();

        public async Task<string> GetIdTokenAsync()
        {
            var result = await _auth.CurrentUser.GetIdTokenAsync(false);
            return result.Token;
        }

        public IObservable<Unit> SignInAnonymously()
        {
            return _auth.SignInAnonymouslyAsync()
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInWithEmail(string email, string password)
        {
            return _auth.SignInWithEmailAndPasswordAsync(email, password)
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInWithFacebook(string accessToken)
        {
            AuthCredential credential = FacebookAuthProvider.GetCredential(accessToken);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInWithGithub(string token)
        {
            AuthCredential credential = GithubAuthProvider.GetCredential(token);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInWithGoogle(string idToken, string accessToken)
        {
            AuthCredential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        public IObservable<PhoneNumberSignInResult> SignInWithPhoneNumber(string phoneNumber)
        {
            var completionHandler = new PhoneNumberVerificationCallbackWrapper();
            PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber, 60, TimeUnit.Seconds, CrossCurrentActivity.Current.Activity, completionHandler);

            return completionHandler.Verify()
                .SelectMany(
                    verificationResult =>
                    {
                        if (verificationResult.AuthCredential != null)
                        {
                            return SignInAsync(verificationResult.AuthCredential)
                            .ToObservable()
                            .Select(authResult => new PhoneNumberSignInResult() { AuthResult = authResult });
                        }
                        else
                        {
                            var signInResult = new PhoneNumberSignInResult()
                            {
                                VerificationId = verificationResult.VerificationId
                            };

                            return Observable.Return(signInResult);
                        }
                    });
        }

        public IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode)
        {
            AuthCredential credential = PhoneAuthProvider.GetCredential(verificationId, verificationCode);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInWithTwitter(string token, string secret)
        {
            AuthCredential credential = TwitterAuthProvider.GetCredential(token, secret);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        public void SignOut()
        {
            _auth.SignOut();
        }

        private async Task<IFirebaseAuthResult> SignInAsync(AuthCredential credential)
        {
            try
            {
                IAuthResult authResult = await _auth.SignInWithCredentialAsync(credential);
                return new FirebaseAuthResult(authResult);
            }
            catch (FirebaseException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        private FirebaseAuthException GetFirebaseAuthException(Exception ex)
        {
            FirebaseExceptionTypeToEnumDict.TryGetValue(ex.GetType(), out FirebaseAuthExceptionType exceptionType);
            return new FirebaseAuthException(ex.Message, ex, exceptionType);
        }
    }
}
