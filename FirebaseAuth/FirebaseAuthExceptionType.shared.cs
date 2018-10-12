namespace GameCtor.FirebaseAuth
{
    /// <summary>
    /// Common exception types used by Firebase Auth.
    /// </summary>
    public enum FirebaseAuthExceptionType
    {
        /// <summary>
        /// An exception type that is not known.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Thrown if the app is not authorized to use Firebase Authentication. Verify the app's package name and SHA-1 in the Firebase Console.
        /// </summary>
        FirebaseAuth,

        /// <summary>
        /// Thrown if attempting to use an expired or an invalid out of band code.
        /// </summary>
        FirebaseAuthActionCode,

        /// <summary>
        /// Thrown on security sensitive operations on a FirebaseUser instance that require the user to have signed in recently, when the requirement isn't met.
        /// </summary>
        FirebaseAuthRecentLoginRequired,

        /// <summary>
        /// Thrown when using a weak password (less than 6 chars) to create a new account or to update an existing account's password. Use getReason() to get a message with the reason the validation failed that you can display to your users.
        /// </summary>
        FirebaseAuthWeakPassword,

        /// <summary>
        /// Thrown if the request is in some way malformed or has expired. Check the error message for details.
        /// </summary>
        FirebaseAuthInvalidCredentials,

        /// <summary>
        /// Thrown if there's a failed attempt to send an email via Firebase Auth (e.g. a password reset email)
        /// </summary>
        FirebaseAuthEmail,

        /// <summary>
        /// Thrown if the user account you are trying to sign in to has been disabled. Also thrown if credential is an EmailAuthCredential with an email address that does not correspond to an existing user.
        /// </summary>
        FirebaseAuthInvalidUser,

        /// <summary>
        /// Thrown if there already exists an account with the email address asserted by the credential. Resolve this case by calling fetchProvidersForEmail(String) and then asking the user to sign in using one of them.
        /// </summary>
        FirebaseAuthUserCollision,

        /// <summary>
        /// Thrown if the sms quota for the project has been exceeded.
        /// </summary>
        FirebaseTooManyRequests,

        /// <summary>
        /// Thrown if this api is called on a device that does not have Google Play Services.
        /// </summary>
        FirebaseApiNotAvailable
    }
}
