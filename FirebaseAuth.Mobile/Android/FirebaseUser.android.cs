using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.Mobile
{
    public class FirebaseUser : IFirebaseUser
    {
        private global::Firebase.Auth.FirebaseUser _user;

        public FirebaseUser(global::Firebase.Auth.FirebaseUser user)
        {
            _user = user;
        }

        public bool IsEmailVerified => _user.IsEmailVerified;

        public string DisplayName => _user.DisplayName;

        public bool IsAnonymous => _user.IsAnonymous;

        public string PhoneNumber => _user.PhoneNumber;

        public Uri PhotoUrl => new Uri(_user.PhotoUrl.ToString());

        public string Email => _user.Email;

        public string ProviderId => _user.ProviderId;

        public IList<string> Providers => _user.Providers;

        public string Uid => _user.Uid;

        /// <summary>
        /// Attempts to link the given phone number to the user, or provides a verification ID if unable.
        /// </summary>
        /// <param name="phoneNumber">The phone number the user is trying to link. Make sure to pass in a phone number with country code prefixed with plus sign ('+').</param>
        /// <returns>PhoneNumberSignInResult containing either a verification ID or an IAuthResultWrapper</returns>
        public IObservable<PhoneNumberSignInResult> LinkWithPhoneNumber(string phoneNumber)
        {
            var completionHandler = new PhoneNumberVerificationCallbackWrapper();
            PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber, 60, TimeUnit.Seconds, CrossCurrentActivity.Current.Activity, completionHandler);

            return completionHandler.Verify()
                .SelectMany(
                    verificationResult =>
                    {
                        if (verificationResult.AuthCredential != null)
                        {
                            return LinkWithCredentialAsync(verificationResult.AuthCredential)
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

        /// <summary>
        /// Attaches the given phone credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="verificationId">The verification ID obtained by calling VerifyPhoneNumber.</param>
        /// <param name="verificationCode">The 6 digit SMS-code sent to the user.</param>
        /// <returns>Updated current account</returns>
        public IObservable<IFirebaseAuthResult> LinkWithPhoneNumber(string verificationId, string verificationCode)
        {
            AuthCredential credential = PhoneAuthProvider.GetCredential(verificationId, verificationCode);
            return _user.LinkWithCredentialAsync(credential).ToObservable().Select(x => new FirebaseAuthResult(x));
            //return LinkWithCredentialAsync(credential).ToObservable();
        }

        /// <summary>
        /// Attaches the given Google credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="idToken"></param>
        /// <param name="accessToken"></param>
        /// <returns>Updated current account</returns>
        public IObservable<IFirebaseAuthResult> LinkWithGoogle(string idToken, string accessToken)
        {
            AuthCredential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            return LinkWithCredentialAsync(credential).ToObservable();
        }

        /// <summary>
        /// Attaches the given Facebook credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="accessToken">The Access Token from Facebook.</param>
        /// <returns>Updated current account</returns>
        public IObservable<IFirebaseAuthResult> LinkWithFacebook(string accessToken)
        {
            AuthCredential credential = FacebookAuthProvider.GetCredential(accessToken);
            return LinkWithCredentialAsync(credential).ToObservable();
        }

        /// <summary>
        /// Attaches the given Twitter credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The Twitter OAuth token.</param>
        /// <param name="secret">The Twitter OAuth secret.</param>
        /// <returns>Updated current account</returns>
        public IObservable<IFirebaseAuthResult> LinkWithTwitter(string token, string secret)
        {
            AuthCredential credential = TwitterAuthProvider.GetCredential(token, secret);
            return LinkWithCredentialAsync(credential).ToObservable();
        }

        /// <summary>
        /// Attaches the given Github credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The GitHub OAuth access token.</param>
        /// <returns>Updated current account</returns>
        public IObservable<IFirebaseAuthResult> LinkWithGithub(string token)
        {
            AuthCredential credential = GithubAuthProvider.GetCredential(token);
            return LinkWithCredentialAsync(credential).ToObservable();
        }

        /// <summary>
        /// Attaches the given email credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="email">The user’s email address.</param>
        /// <param name="password">The user’s password.</param>
        /// <returns>Updated current account</returns>
        public IObservable<IFirebaseAuthResult> LinkWithEmail(string email, string password)
        {
            AuthCredential credential = EmailAuthProvider.GetCredential(email, password);
            return LinkWithCredentialAsync(credential).ToObservable();
        }

        /// <summary>
        /// Detaches credentials from a given provider type from this user. This prevents the user from signing in to this account in the future with credentials from such provider.
        /// </summary>
        /// <param name="providerId">A unique identifier of the type of provider to be unlinked.</param>
        /// <returns>Task of IFirebaseUser</returns>
        public IObservable<IFirebaseUser> Unlink(string providerId)
        {
            return Observable.Create<IFirebaseUser>(
                async observer =>
                {
                    IAuthResult authResult = null;

                    try
                    {
                        authResult = await _user.UnlinkAsync(providerId);
                    }
                    catch(Exception ex)
                    {
                        observer.OnError(GetFirebaseAuthException(ex));
                        return;
                    }

                    observer.OnNext(new FirebaseUser(authResult.User));
                    observer.OnCompleted();
                });

            //return Observable.Create<IFirebaseUser>(
            //    observer =>
            //    {
            //        var subscription = _user.UnlinkAsync(providerId)
            //            .ToObservable()
            //            .Select(authResult => new FirebaseUser(authResult.User))
            //            .Subscribe(x => observer.OnNext(x), ex => observer.OnError(GetFirebaseAuthException(ex)));

            //        return new CompositeDisposable(subscription);
            //    });
        }

        /// <summary>
        /// Fetches a Firebase Auth ID Token for the user.
        /// </summary>
        /// <param name="forceRefresh">Force refreshes the token. Should only be set to true if the token is invalidated out of band.</param>
        /// <returns>Task with the ID Token</returns>
        public async Task<string> GetIdTokenAsync(bool forceRefresh)
        {
            try
            {
                GetTokenResult tokenResult = await _user.GetIdTokenAsync(forceRefresh);
                return tokenResult.Token;
            }
            catch(FirebaseException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        private async Task<IFirebaseAuthResult> LinkWithCredentialAsync(AuthCredential credential)
        {
            IAuthResult authResult = null;

            try
            {
                authResult = await _user.LinkWithCredentialAsync(credential);
            }
            catch(Firebase.Auth.FirebaseAuthException ex)
            {
                Console.WriteLine(ex.Message);
                throw GetFirebaseAuthException(ex);
            }
            catch(RuntimeWrappedException ex)
            {
                var authEx = ex.WrappedException as Firebase.Auth.FirebaseAuthException;
                throw GetFirebaseAuthException(ex);
            }
            catch
            {
                Console.WriteLine("Uh oh");
                throw;
            }

            return new FirebaseAuthResult(authResult);
        }

        private FirebaseAuthException GetFirebaseAuthException(Exception ex)
        {
            FirebaseAuthService.FirebaseExceptionTypeToEnumDict.TryGetValue(ex.GetType(), out FirebaseAuthExceptionType exceptionType);
            return new FirebaseAuthException(ex.Message, ex, exceptionType);
        }
    }
}
