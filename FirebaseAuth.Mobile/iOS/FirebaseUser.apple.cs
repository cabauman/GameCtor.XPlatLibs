using Firebase.Auth;
using Foundation;
using System;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.Mobile
{
    public class FirebaseUser : IFirebaseUser
    {
        private User _user;

        public FirebaseUser(User user)
        {
            _user = user;
        }

        public bool IsEmailVerified => _user.IsEmailVerified;

        public string DisplayName => _user.DisplayName;

        public bool IsAnonymous => _user.IsAnonymous;

        public string PhoneNumber => _user.PhoneNumber;

        public Uri PhotoUrl => new Uri(_user.PhotoUrl.AbsoluteString);

        public string Email => _user.Email;

        public string ProviderId => _user.ProviderId;

        public string Uid => _user.Uid;

        /// <summary>
        /// Attaches the given phone credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="verificationId">The verification ID obtained by calling VerifyPhoneNumber.</param>
        /// <param name="verificationCode">The 6 digit SMS-code sent to the user.</param>
        /// <returns>Task of IFirebaseAuthResult</returns>
        public async Task<IFirebaseAuthResult> LinkWithPhoneNumberAsync(string verificationId, string verificationCode)
        {
            AuthCredential credential = PhoneAuthProvider.DefaultInstance.GetCredential(verificationId, verificationCode);
            return await LinkWithCredentialAsync(credential);
        }

        /// <summary>
        /// Attaches the given Google credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="idToken">The ID Token from Google.</param>
        /// <param name="accessToken">The Access Token from Google.</param>
        /// <returns>Task of IFirebaseAuthResult</returns>
        public async Task<IFirebaseAuthResult> LinkWithGoogleAsync(string idToken, string accessToken)
        {
            AuthCredential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            return await LinkWithCredentialAsync(credential);
        }

        /// <summary>
        /// Attaches the given Facebook credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="accessToken">The Access Token from Facebook.</param>
        /// <returns>Task of IFirebaseAuthResult</returns>
        public async Task<IFirebaseAuthResult> LinkWithFacebookAsync(string accessToken)
        {
            AuthCredential credential = FacebookAuthProvider.GetCredential(accessToken);
            return await LinkWithCredentialAsync(credential);
        }

        /// <summary>
        /// Attaches the given Twitter credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The Twitter OAuth token.</param>
        /// <param name="secret">The Twitter OAuth secret.</param>
        /// <returns>Task of IFirebaseAuthResult</returns>
        public async Task<IFirebaseAuthResult> LinkWithTwitterAsync(string token, string secret)
        {
            AuthCredential credential = TwitterAuthProvider.GetCredential(token, secret);
            return await LinkWithCredentialAsync(credential);
        }

        /// <summary>
        /// Attaches the given Github credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The GitHub OAuth access token.</param>
        /// <returns>Task of IFirebaseAuthResult</returns>
        public async Task<IFirebaseAuthResult> LinkWithGithubAsync(string token)
        {
            AuthCredential credential = GitHubAuthProvider.GetCredential(token);
            return await LinkWithCredentialAsync(credential);
        }

        /// <summary>
        /// Attaches the given email credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="email">The user’s email address.</param>
        /// <param name="password">The user’s password.</param>
        /// <returns>Task of IFirebaseAuthResult</returns>
        public async Task<IFirebaseAuthResult> LinkWithEmailAsync(string email, string password)
        {
            AuthCredential credential = EmailAuthProvider.GetCredentialFromPassword(email, password);
            return await LinkWithCredentialAsync(credential);
        }

        /// <summary>
        /// Detaches credentials from a given provider type from this user. This prevents the user from signing in to this account in the future with credentials from such provider.
        /// </summary>
        /// <param name="providerId">A unique identifier of the type of provider to be unlinked.</param>
        /// <returns>Task of IFirebaseUser</returns>
        public async Task<IFirebaseUser> UnlinkAsync(string providerId)
        {
            try
            {
                User user = await Auth.DefaultInstance.CurrentUser.UnlinkAsync(providerId);
                return new FirebaseUser(user);
            }
            catch(NSErrorException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
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
                return await _user.GetIdTokenAsync(forceRefresh);
            }
            catch(NSErrorException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        private async Task<IFirebaseAuthResult> LinkWithCredentialAsync(AuthCredential credential)
        {
            try
            {
                AuthDataResult authResult = await _user.LinkAndRetrieveDataAsync(credential);
                return new FirebaseAuthResult(authResult);
            }
            catch(NSErrorException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        private FirebaseAuthException GetFirebaseAuthException(NSErrorException ex)
        {
            AuthErrorCode errorCode;
            if(IntPtr.Size == 8) // 64 bits devices
            {
                errorCode = (AuthErrorCode)((long)ex.Error.Code);
            }
            else // 32 bits devices
            {
                errorCode = (AuthErrorCode)((int)ex.Error.Code);
            }

            FirebaseAuthImplementation.FirebaseExceptionTypeToEnumDict.TryGetValue(errorCode, out FirebaseAuthExceptionType exceptionType);
            throw new FirebaseAuthException(ex.Message, ex, exceptionType);
        }
    }
}
