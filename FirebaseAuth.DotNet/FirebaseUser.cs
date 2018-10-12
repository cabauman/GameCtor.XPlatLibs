using Firebase;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.DotNet
{
    public class FirebaseUser : IFirebaseUser
    {
        private FirebaseAuthLink _authLink;

        public FirebaseUser(FirebaseAuthLink authLink)
        {
            _authLink = authLink;
        }

        public bool IsEmailVerified => _authLink.User.IsEmailVerified;

        public string DisplayName => _authLink.User.DisplayName;

        public bool IsAnonymous => throw new NotImplementedException();

        public string PhoneNumber => _authLink.User.PhoneNumber;

        public Uri PhotoUrl => new Uri(_authLink.User.PhotoUrl.ToString());

        public string Email => _authLink.User.Email;

        public string ProviderId => _authLink.User.FederatedId;

        public IList<string> Providers => throw new NotImplementedException();

        public string Uid => _authLink.User.LocalId;

        /// <summary>
        /// Attempts to link the given phone number to the user, or provides a verification ID if unable.
        /// </summary>
        /// <param name="phoneNumber">The phone number the user is trying to link. Make sure to pass in a phone number with country code prefixed with plus sign ('+').</param>
        /// <returns>PhoneNumberSignInResult containing either a verification ID or an IAuthResultWrapper</returns>
        public IObservable<PhoneNumberSignInResult> LinkWithPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attaches the given phone credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="verificationId">The verification ID obtained by calling VerifyPhoneNumber.</param>
        /// <param name="verificationCode">The 6 digit SMS-code sent to the user.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        public IObservable<IFirebaseAuthResult> LinkWithPhoneNumber(string verificationId, string verificationCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attaches the given Google credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="idToken"></param>
        /// <param name="accessToken"></param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        public IObservable<IFirebaseAuthResult> LinkWithGoogle(string idToken, string accessToken)
        {
            return _authLink
                .LinkToAsync(FirebaseAuthType.Github, accessToken)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <summary>
        /// Attaches the given Facebook credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="accessToken">The Access Token from Facebook.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        public IObservable<IFirebaseAuthResult> LinkWithFacebook(string accessToken)
        {
            return _authLink
                .LinkToAsync(FirebaseAuthType.Facebook, accessToken)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <summary>
        /// Attaches the given Twitter credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The Twitter OAuth token.</param>
        /// <param name="secret">The Twitter OAuth secret.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        public IObservable<IFirebaseAuthResult> LinkWithTwitter(string token, string secret)
        {
            return _authLink
                .LinkToAsync(FirebaseAuthType.Twitter, token)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <summary>
        /// Attaches the given Github credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The GitHub OAuth access token.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        public IObservable<IFirebaseAuthResult> LinkWithGithub(string token)
        {
            return _authLink
                .LinkToAsync(FirebaseAuthType.Github, token)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <summary>
        /// Attaches the given email credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="email">The user’s email address.</param>
        /// <param name="password">The user’s password.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        public IObservable<IFirebaseAuthResult> LinkWithEmail(string email, string password)
        {
            return _authLink
                .LinkToAsync(email, password)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <summary>
        /// Detaches credentials from a given provider type from this user. This prevents the user from signing in to this account in the future with credentials from such provider.
        /// </summary>
        /// <param name="providerId">A unique identifier of the type of provider to be unlinked.</param>
        /// <returns>Instance of IFirebaseUser</returns>
        public IObservable<IFirebaseUser> Unlink(string providerId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches a Firebase Auth ID Token for the user.
        /// </summary>
        /// <param name="forceRefresh">Force refreshes the token. Should only be set to true if the token is invalidated out of band.</param>
        /// <returns>Task with the ID Token</returns>
        public async Task<string> GetIdTokenAsync(bool forceRefresh)
        {
            return (await _authLink.GetFreshAuthAsync()).FirebaseToken;
        }
    }
}
