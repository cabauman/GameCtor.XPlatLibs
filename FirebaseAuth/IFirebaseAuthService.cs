using System;
using System.Reactive;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth
{
    public interface IFirebaseAuthService
    {
        /// <summary>
        /// Gets the current user.
        /// </summary>
        IFirebaseUser CurrentUser { get; }

        /// <summary>
        /// Gets an observable that emits a value indicating whether this user is authenticated.
        /// </summary>
        IObservable<bool> IsAuthenticated { get; }

        /// <summary>
        /// Retrieves the user ID token.
        /// </summary>
        /// <returns>Task containing the ID token.</returns>
        Task<string> GetIdTokenAsync();

        /// <summary>
        /// Signs in to Firebase with the given Google credentials.
        /// </summary>
        /// <param name="idToken">The ID Token from Google.</param>
        /// <param name="accessToken">The Access Token from Google.</param>
        /// <returns>User account</returns>
        IObservable<Unit> SignInWithGoogle(string idToken, string accessToken);

        /// <summary>
        /// Signs in to Firebase with the given Facebook credentials.
        /// </summary>
        /// <param name="accessToken">The Access Token from Facebook.</param>
        /// <returns>User account</returns>
        IObservable<Unit> SignInWithFacebook(string accessToken);

        /// <summary>
        /// Signs in to Firebase with the given Twitter credentials.
        /// </summary>
        /// <param name="token">The Twitter OAuth token.</param>
        /// <param name="secret">The Twitter OAuth secret.</param>
        /// <returns>User account</returns>
        IObservable<Unit> SignInWithTwitter(string token, string secret);

        /// <summary>
        /// Signs in to Firebase with the given  Github credentials.
        /// </summary>
        /// <param name="token">The GitHub OAuth access token.</param>
        /// <returns>User account</returns>
        IObservable<Unit> SignInWithGithub(string token);

        /// <summary>
        /// Signs the user in via email.
        /// </summary>
        /// <param name="email">The user’s email address.</param>
        /// <param name="password">The user’s password.</param>
        /// <returns>User account</returns>
        IObservable<Unit> SignInWithEmail(string email, string password);

        /// <summary>
        /// Attempts to sign in to Firebase with the given phone number, or provides a verification ID if unable.
        /// </summary>
        /// <param name="phoneNumber">The phone number for the account the user is signing up for or signing into. Make sure to pass in a phone number with country code prefixed with plus sign ('+').</param>
        /// <returns>PhoneNumberSignInResult containing either a verification ID or an IAuthResultWrapper</returns>
        IObservable<PhoneNumberSignInResult> SignInWithPhoneNumber(string phoneNumber);

        /// <summary>
        /// Signs in to Firebase with the given phone number credentials.
        /// </summary>
        /// <param name="verificationId">The verification ID.</param>
        /// <param name="verificationCode">The 6 digit SMS-code sent to the user.</param>
        /// <returns>User account</returns>
        IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode);

        /// <summary>
        /// Signs in the user anonymously without requiring any credential.
        /// </summary>
        /// <returns>Task of IFirebaseAuthResult with the result of the operation</returns>
        IObservable<Unit> SignInAnonymously();

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        void SignOut();
    }
}