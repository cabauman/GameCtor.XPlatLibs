using Firebase;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.DotNet
{
    public class FirebaseUser : IFirebaseUser
    {
        private FirebaseAuthLink _user;

        public FirebaseUser(FirebaseAuthLink user)
        {
            _user = user;
        }

        public bool IsEmailVerified => _user.User.IsEmailVerified;

        public string DisplayName => _user.User.DisplayName;

        public bool IsAnonymous => throw new NotImplementedException();

        public string PhoneNumber => _user.User.PhoneNumber;

        public Uri PhotoUrl => new Uri(_user.User.PhotoUrl.ToString());

        public string Email => _user.User.Email;

        public string ProviderId => throw new NotImplementedException();

        public string Uid => _user.User.LocalId;

        /// <summary>
        /// Attaches the given phone credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="verificationId">The verification ID obtained by calling VerifyPhoneNumber.</param>
        /// <param name="verificationCode">The 6 digit SMS-code sent to the user.</param>
        /// <returns>Updated current account</returns>
        public Task<IFirebaseAuthResult> LinkWithPhoneNumberAsync(string verificationId, string verificationCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attaches the given Google credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="idToken"></param>
        /// <param name="accessToken"></param>
        /// <returns>Updated current account</returns>
        public async Task<IFirebaseAuthResult> LinkWithGoogleAsync(string idToken, string accessToken)
        {
            FirebaseAuthLink authLink = await _user.LinkToAsync(FirebaseAuthType.Google, accessToken);
            return new FirebaseAuthResult(authLink);
        }

        /// <summary>
        /// Attaches the given Facebook credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="accessToken">The Access Token from Facebook.</param>
        /// <returns>Updated current account</returns>
        public async Task<IFirebaseAuthResult> LinkWithFacebookAsync(string accessToken)
        {
            FirebaseAuthLink authLink = await _user.LinkToAsync(FirebaseAuthType.Facebook, accessToken);
            return new FirebaseAuthResult(authLink);
        }

        /// <summary>
        /// Attaches the given Twitter credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The Twitter OAuth token.</param>
        /// <param name="secret">The Twitter OAuth secret.</param>
        /// <returns>Updated current account</returns>
        public async Task<IFirebaseAuthResult> LinkWithTwitterAsync(string token, string secret)
        {
            FirebaseAuthLink authLink = await _user.LinkToAsync(FirebaseAuthType.Twitter, token);
            return new FirebaseAuthResult(authLink);
        }

        /// <summary>
        /// Attaches the given Github credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The GitHub OAuth access token.</param>
        /// <returns>Updated current account</returns>
        public async Task<IFirebaseAuthResult> LinkWithGithubAsync(string token)
        {
            FirebaseAuthLink authLink = await _user.LinkToAsync(FirebaseAuthType.Github, token);
            return new FirebaseAuthResult(authLink);
        }

        /// <summary>
        /// Attaches the given email credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="email">The user’s email address.</param>
        /// <param name="password">The user’s password.</param>
        /// <returns>Updated current account</returns>
        public async Task<IFirebaseAuthResult> LinkWithEmailAsync(string email, string password)
        {
            FirebaseAuthLink authLink = await _user.LinkToAsync(email, password);
            return new FirebaseAuthResult(authLink);
        }

        /// <summary>
        /// Detaches credentials from a given provider type from this user. This prevents the user from signing in to this account in the future with credentials from such provider.
        /// </summary>
        /// <param name="providerId">A unique identifier of the type of provider to be unlinked.</param>
        /// <returns>Task of IUserWrapper</returns>
        public Task<IFirebaseUser> UnlinkAsync(string providerId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches a Firebase Auth ID Token for the user.
        /// </summary>
        /// <param name="forceRefresh">Force refreshes the token. Should only be set to true if the token is invalidated out of band.</param>
        /// <returns>Task with the ID Token</returns>
        public Task<string> GetIdTokenAsync(bool forceRefresh)
        {
            throw new NotImplementedException();
            //return _user.FirebaseToken;
        }
    }
}
