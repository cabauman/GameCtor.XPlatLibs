using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.Mobile
{
    /// <summary>
    /// Main interface for Firebase authentication
    /// </summary>
    public interface IFirebaseAuth
    {
        /// <summary>
        /// Gets the current user.
        /// </summary>
        IFirebaseUser CurrentUser { get; }

        /// <summary>
        /// Attempts to link the given phone number to the current user, or provides a verification ID if unable.
        /// </summary>
        /// <param name="phoneNumber">The phone number for the account the user is trying to link. Make sure to pass in a phone number with country code prefixed with plus sign ('+').</param>
        /// <returns>PhoneNumberSignInResult containing either a verification ID or an IAuthResultWrapper</returns>
        Task<PhoneNumberSignInResult> LinkPhoneNumberWithCurrentUserAsync(string phoneNumber);

        /// <summary>
        /// Signs in the user anonymously without requiring any credential.
        /// </summary>
        /// <returns>Task of IUserWrapper with the result of the operation</returns>
        Task<IFirebaseAuthResult> SignInAnonymouslyAsync();

        /// <summary>
        /// Attempts to sign in to Firebase with the given phone number, or provides a verification ID if unable.
        /// </summary>
        /// <param name="phoneNumber">The phone number for the account the user is signing up for or signing into. Make sure to pass in a phone number with country code prefixed with plus sign ('+').</param>
        /// <returns>PhoneNumberSignInResult containing either a verification ID or an IAuthResultWrapper</returns>
        Task<PhoneNumberSignInResult> SignInWithPhoneNumberAsync(string phoneNumber);

        /// <summary>
        /// Signs in to Firebase with the given phone number credentials.
        /// </summary>
        /// <param name="verificationId"></param>
        /// <param name="verificationCode">The 6 digit SMS-code sent to the user.</param>
        /// <returns>User account</returns>
        Task<IFirebaseAuthResult> SignInWithPhoneNumberAsync(string verificationId, string verificationCode);

        /// <summary>
        /// Signs in to Firebase with the given Google credentials.
        /// </summary>
        /// <param name="idToken">The ID Token from Google.</param>
        /// <param name="accessToken">The Access Token from Google.</param>
        /// <remarks>Both parameters are optional but at least one must be present.</remarks>
        /// <returns>User account</returns>
        Task<IFirebaseAuthResult> SignInWithGoogleAsync(string idToken, string accessToken);

        /// <summary>
        /// Signs in to Firebase with the given Facebook credentials.
        /// </summary>
        /// <param name="accessToken">The Access Token from Facebook.</param>
        /// <returns>User account</returns>
        Task<IFirebaseAuthResult> SignInWithFacebookAsync(string accessToken);

        /// <summary>
        /// Signs in to Firebase with the given Twitter credentials.
        /// </summary>
        /// <param name="token">The Twitter OAuth token.</param>
        /// <param name="secret">The Twitter OAuth secret.</param>
        /// <returns>User account</returns>
        Task<IFirebaseAuthResult> SignInWithTwitterAsync(string token, string secret);

        /// <summary>
        /// Signs in to Firebase with the given  Github credentials.
        /// </summary>
        /// <param name="token">The GitHub OAuth access token.</param>
        /// <returns>User account</returns>
        Task<IFirebaseAuthResult> SignInWithGithubAsync(string token);

        /// <summary>
        /// Signs the user in via email.
        /// </summary>
        /// <param name="email">The user’s email address.</param>
        /// <param name="password">The user’s password.</param>
        /// <returns>User account</returns>
        Task<IFirebaseAuthResult> SignInWithEmailAsync(string email, string password);

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        void SignOut();
    }
}
