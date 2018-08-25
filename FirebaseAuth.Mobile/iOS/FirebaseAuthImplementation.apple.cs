using Firebase.Auth;
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace GameCtor.FirebaseAuth.Mobile
{
    /// <summary>
    /// Main implementation for IFirebaseAuth
    /// </summary>
    public class FirebaseAuthImplementation : IFirebaseAuth
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

        /// <summary>
        /// Gets the current user.
        /// </summary>
        public IFirebaseUser CurrentUser
        {
            get { return new FirebaseUser(Auth.DefaultInstance.CurrentUser); }
        }

        /// <summary>
        /// Attempts to link the given phone number to the current user, or provides a verification ID if unable.
        /// </summary>
        /// <param name="phoneNumber">The phone number for the account the user is trying to link. Make sure to pass in a phone number with country code prefixed with plus sign ('+').</param>
        /// <returns>PhoneNumberSignInResult containing either a verification ID or an IAuthResultWrapper</returns>
        public async Task<PhoneNumberSignInResult> LinkPhoneNumberWithCurrentUserAsync(string phoneNumber)
        {
            try
            {
                string verificationId = await PhoneAuthProvider.DefaultInstance.VerifyPhoneNumberAsync(phoneNumber, null);
                return new PhoneNumberSignInResult()
                {
                    VerificationId = verificationId
                };
            }
            catch(NSErrorException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        /// <summary>
        /// Signs in the user anonymously without requiring any credential.
        /// </summary>
        /// <returns>Task of IFirebaseAuthResult with the result of the operation</returns>
        public async Task<IFirebaseAuthResult> SignInAnonymouslyAsync()
        {
            try
            {
                var authResult = await Auth.DefaultInstance.SignInAnonymouslyAsync();
                return new FirebaseAuthResult(authResult);
            }
            catch(NSErrorException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        /// <summary>
        /// Attempts to sign in to Firebase with the given phone number, or provides a verification ID if unable.
        /// </summary>
        /// <param name="phoneNumber">The phone number for the account the user is signing up for or signing into. Make sure to pass in a phone number with country code prefixed with plus sign ('+').</param>
        /// <returns>PhoneNumberSignInResult containing either a verification ID or an IAuthResultWrapper</returns>
        public async Task<PhoneNumberSignInResult> SignInWithPhoneNumberAsync(string phoneNumber)
        {
            try
            {
                string verificationId = await PhoneAuthProvider.DefaultInstance.VerifyPhoneNumberAsync(phoneNumber, null);
                return new PhoneNumberSignInResult()
                {
                    VerificationId = verificationId
                };
            }
            catch(NSErrorException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        /// <summary>
        /// Signs in to Firebase with the given phone number credentials.
        /// </summary>
        /// <param name="verificationId"></param>
        /// <param name="verificationCode">The 6 digit SMS-code sent to the user.</param>
        /// <returns>User account</returns>
        public async Task<IFirebaseAuthResult> SignInWithPhoneNumberAsync(string verificationId, string verificationCode)
        {
            AuthCredential credential = PhoneAuthProvider.DefaultInstance.GetCredential(verificationId, verificationCode);
            return await SignInAsync(credential);
        }

        /// <summary>
        /// Signs in to Firebase with the given Google credentials.
        /// </summary>
        /// <param name="idToken">The ID Token from Google.</param>
        /// <param name="accessToken">The Access Token from Google.</param>
        /// <returns>User account</returns>
        public async Task<IFirebaseAuthResult> SignInWithGoogleAsync(string idToken, string accessToken)
        {
            AuthCredential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            return await SignInAsync(credential);
        }

        /// <summary>
        /// Signs in to Firebase with the given Facebook credentials.
        /// </summary>
        /// <param name="accessToken">The Access Token from Facebook.</param>
        /// <returns>User account</returns>
        public async Task<IFirebaseAuthResult> SignInWithFacebookAsync(string accessToken)
        {
            AuthCredential credential = FacebookAuthProvider.GetCredential(accessToken);
            return await SignInAsync(credential);
        }

        /// <summary>
        /// Signs in to Firebase with the given Twitter credentials.
        /// </summary>
        /// <param name="token">The Twitter OAuth token.</param>
        /// <param name="secret">The Twitter OAuth secret.</param>
        /// <returns>User account</returns>
        public async Task<IFirebaseAuthResult> SignInWithTwitterAsync(string token, string secret)
        {
            AuthCredential credential = TwitterAuthProvider.GetCredential(token, secret);
            return await SignInAsync(credential);
        }

        /// <summary>
        /// Signs in to Firebase with the given  Github credentials.
        /// </summary>
        /// <param name="token">The GitHub OAuth access token.</param>
        /// <returns>User account</returns>
        public async Task<IFirebaseAuthResult> SignInWithGithubAsync(string token)
        {
            AuthCredential credential = GitHubAuthProvider.GetCredential(token);
            return await SignInAsync(credential);
        }

        /// <summary>
        /// Signs the user in via email.
        /// </summary>
        /// <param name="email">The user’s email address.</param>
        /// <param name="password">The user’s password.</param>
        /// <returns>User account</returns>
        public async Task<IFirebaseAuthResult> SignInWithEmailAsync(string email, string password)
        {
            try
            {
                AuthDataResult authResult = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                return new FirebaseAuthResult(authResult);
            }
            catch(NSErrorException ex)
            {
                throw GetFirebaseAuthException(ex);
            }
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        public void SignOut()
        {
            bool success = Auth.DefaultInstance.SignOut(out NSError error);
            if(!success)
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

            FirebaseExceptionTypeToEnumDict.TryGetValue(errorCode, out FirebaseAuthExceptionType exceptionType);
            throw new FirebaseAuthException(ex.Message, ex, exceptionType);
        }
    }
}
