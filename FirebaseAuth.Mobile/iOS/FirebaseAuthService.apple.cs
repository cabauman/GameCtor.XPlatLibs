using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace GameCtor.FirebaseAuth.Mobile
{
    /// <summary>
    /// iOS implementation of IFirebaseAuthService.
    /// </summary>
    public class FirebaseAuthService : IFirebaseAuthService
    {
        internal static IDictionary<AuthErrorCode, FirebaseAuthExceptionType> FirebaseExceptionTypeToEnumDict { get; } = new Dictionary<AuthErrorCode, FirebaseAuthExceptionType>
        {
            { AuthErrorCode.ExpiredActionCode, FirebaseAuthExceptionType.FirebaseAuthActionCode },
            { AuthErrorCode.InvalidActionCode, FirebaseAuthExceptionType.FirebaseAuthActionCode },
            { AuthErrorCode.RequiresRecentLogin, FirebaseAuthExceptionType.FirebaseAuthRecentLoginRequired },
            { AuthErrorCode.WeakPassword, FirebaseAuthExceptionType.FirebaseAuthWeakPassword },
            { AuthErrorCode.UserDisabled, FirebaseAuthExceptionType.FirebaseAuthInvalidUser },
            { AuthErrorCode.UserNotFound, FirebaseAuthExceptionType.FirebaseAuthInvalidUser },
            { AuthErrorCode.AccountExistsWithDifferentCredential, FirebaseAuthExceptionType.FirebaseAuthUserCollision },
            { AuthErrorCode.InvalidRecipientEmail, FirebaseAuthExceptionType.FirebaseAuthEmail },
            { AuthErrorCode.InvalidSender, FirebaseAuthExceptionType.FirebaseAuthEmail },
            { AuthErrorCode.InvalidMessagePayload, FirebaseAuthExceptionType.FirebaseAuthEmail },
            { AuthErrorCode.TooManyRequests, FirebaseAuthExceptionType.FirebaseTooManyRequests },
            { AuthErrorCode.QuotaExceeded, FirebaseAuthExceptionType.FirebaseTooManyRequests },
            { AuthErrorCode.InvalidCredential, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.InvalidPhoneNumber, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.MissingPhoneNumber, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.OperationNotAllowed, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.WrongPassword, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.InvalidEmail, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.MissingVerificationID, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.MissingVerificationCode, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.InvalidVerificationID, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.InvalidVerificationCode, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
            { AuthErrorCode.SessionExpired, FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials },
        };

        /// <inheritdoc/>
        public IFirebaseUser CurrentUser => Auth.DefaultInstance.CurrentUser != null ? new FirebaseUser(Auth.DefaultInstance.CurrentUser) : null;

        /// <inheritdoc/>
        public IObservable<bool> IsAuthenticated => throw new NotImplementedException();

        /// <inheritdoc/>
        public IObservable<Unit> SignInAnonymously()
        {
            return Observable.Create<Unit>(
                async observer =>
                {
                    AuthDataResult authResult = null;

                    try
                    {
                        authResult = await Auth.DefaultInstance.SignInAnonymouslyAsync();
                    }
                    catch (NSErrorException ex)
                    {
                        observer.OnError(GetFirebaseAuthException(ex));
                        return;
                    }

                    observer.OnNext(Unit.Default /*new FirebaseAuthResult(authResult)*/);
                    observer.OnCompleted();
                });
        }

        /// <inheritdoc/>
        public IObservable<PhoneNumberSignInResult> SignInWithPhoneNumber(string phoneNumber)
        {
            return Observable.Create<PhoneNumberSignInResult>(
                async observer =>
                {
                    string verificationId = null;

                    try
                    {
                        verificationId = await PhoneAuthProvider.DefaultInstance.VerifyPhoneNumberAsync(phoneNumber, null);
                    }
                    catch (NSErrorException ex)
                    {
                        observer.OnError(GetFirebaseAuthException(ex));
                        return;
                    }

                    var result = new PhoneNumberSignInResult()
                    {
                        VerificationId = verificationId
                    };

                    observer.OnNext(result);
                    observer.OnCompleted();
                });
        }

        /// <inheritdoc/>
        public IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode)
        {
            AuthCredential credential = PhoneAuthProvider.DefaultInstance.GetCredential(verificationId, verificationCode);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        /// <inheritdoc/>
        public IObservable<Unit> SignInWithGoogle(string idToken, string accessToken)
        {
            AuthCredential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        /// <inheritdoc/>
        public IObservable<Unit> SignInWithFacebook(string accessToken)
        {
            AuthCredential credential = FacebookAuthProvider.GetCredential(accessToken);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        /// <inheritdoc/>
        public IObservable<Unit> SignInWithTwitter(string token, string secret)
        {
            AuthCredential credential = TwitterAuthProvider.GetCredential(token, secret);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        /// <inheritdoc/>
        public IObservable<Unit> SignInWithGithub(string token)
        {
            AuthCredential credential = GitHubAuthProvider.GetCredential(token);
            return SignInAsync(credential).ToObservable().Select(_ => Unit.Default);
        }

        /// <inheritdoc/>
        public IObservable<Unit> SignInWithEmail(string email, string password)
        {
            return Observable.Create<Unit>(
                async observer =>
                {
                    AuthDataResult authResult = null;

                    try
                    {
                        authResult = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                    }
                    catch (NSErrorException ex)
                    {
                        observer.OnError(GetFirebaseAuthException(ex));
                        return;
                    }

                    observer.OnNext(Unit.Default /*new FirebaseAuthResult(authResult)*/);
                    observer.OnCompleted();
                });
        }

        /// <inheritdoc/>
        public async Task<string> GetIdTokenAsync()
        {
            try
            {
                return await Auth.DefaultInstance.CurrentUser.GetIdTokenAsync();
            }
            catch (NSErrorException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        /// <inheritdoc/>
        public void SignOut()
        {
            bool success = Auth.DefaultInstance.SignOut(out NSError error);
            if (!success)
            {
                throw new NSErrorException(error);
            }
        }

        private async Task<IFirebaseAuthResult> SignInAsync(AuthCredential credential)
        {
            try
            {
                AuthDataResult authResult = await Auth.DefaultInstance.SignInAndRetrieveDataWithCredentialAsync(credential);
                return new FirebaseAuthResult(authResult);
            }
            catch (NSErrorException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        private FirebaseAuthException GetFirebaseAuthException(NSErrorException ex)
        {
            AuthErrorCode errorCode;
            if (IntPtr.Size == 8)
            {
                // 64 bits devices
                errorCode = (AuthErrorCode)((long)ex.Error.Code);
            }
            else
            {
                // 32 bits devices
                errorCode = (AuthErrorCode)((int)ex.Error.Code);
            }

            FirebaseExceptionTypeToEnumDict.TryGetValue(errorCode, out FirebaseAuthExceptionType exceptionType);
            return new FirebaseAuthException(ex.Message, ex, exceptionType);
        }
    }
}
