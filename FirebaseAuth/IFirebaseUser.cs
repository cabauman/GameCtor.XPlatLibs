using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth
{
    public interface IFirebaseUser
    {
        bool IsEmailVerified { get; }

        string DisplayName { get; }

        bool IsAnonymous { get; }

        string PhoneNumber { get; }

        Uri PhotoUrl { get; }

        string Email { get; }

        string ProviderId { get; }

        IList<string> Providers { get; }

        string Uid { get; }

        /// <summary>
        /// Attempts to link the given phone number to the user, or provides a verification ID if unable.
        /// </summary>
        /// <param name="phoneNumber">The phone number the user is trying to link. Make sure to pass in a phone number with country code prefixed with plus sign ('+').</param>
        /// <returns>PhoneNumberSignInResult containing either a verification ID or an IAuthResultWrapper</returns>
        IObservable<PhoneNumberSignInResult> LinkWithPhoneNumber(string phoneNumber);

        /// <summary>
        /// Attaches the given phone credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="verificationId">The verification ID obtained by calling VerifyPhoneNumber.</param>
        /// <param name="verificationCode">The 6 digit SMS-code sent to the user.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        IObservable<IFirebaseAuthResult> LinkWithPhoneNumber(string verificationId, string verificationCode);

        /// <summary>
        /// Attaches the given Google credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="idToken">The ID Token from Google.</param>
        /// <param name="accessToken">The Access Token from Google.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        IObservable<IFirebaseAuthResult> LinkWithGoogle(string idToken, string accessToken);

        /// <summary>
        /// Attaches the given Facebook credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="accessToken">The Access Token from Facebook.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        IObservable<IFirebaseAuthResult> LinkWithFacebook(string accessToken);

        /// <summary>
        /// Attaches the given Twitter credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The Twitter OAuth token.</param>
        /// <param name="secret">The Twitter OAuth secret.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        IObservable<IFirebaseAuthResult> LinkWithTwitter(string token, string secret);

        /// <summary>
        /// Attaches the given Github credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="token">The GitHub OAuth access token.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        IObservable<IFirebaseAuthResult> LinkWithGithub(string token);

        /// <summary>
        /// Attaches the given email credentials to the user. This allows the user to sign in to this account in the future with credentials for such provider.
        /// </summary>
        /// <param name="email">The user’s email address.</param>
        /// <param name="password">The user’s password.</param>
        /// <returns>Instance of IFirebaseAuthResult</returns>
        IObservable<IFirebaseAuthResult> LinkWithEmail(string email, string password);

        /// <summary>
        /// Detaches credentials from a given provider type from this user. This prevents the user from signing in to this account in the future with credentials from such provider.
        /// </summary>
        /// <param name="providerId">A unique identifier of the type of provider to be unlinked.</param>
        /// <returns>Instance of IFirebaseUser</returns>
        IObservable<IFirebaseUser> Unlink(string providerId);

        /// <summary>
        /// Fetches a Firebase Auth ID Token for the user.
        /// </summary>
        /// <param name="forceRefresh">Force refreshes the token. Should only be set to true if the token is invalidated out of band.</param>
        /// <returns>Task with the ID Token</returns>
        Task<string> GetIdTokenAsync(bool forceRefresh);
    }
}
