using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;

namespace GameCtor.FirebaseAuth.DotNet
{
    /// <summary>
    /// FirebaseAuthenticationDotNet implementation of IFirebaseUser.
    /// </summary>
    public class FirebaseUser : IFirebaseUser
    {
        private FirebaseAuthLink _authLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirebaseUser"/> class.
        /// </summary>
        /// <param name="authLink">The auth link.</param>
        public FirebaseUser(FirebaseAuthLink authLink)
        {
            _authLink = authLink;
        }

        /// <inheritdoc />
        public bool IsEmailVerified => _authLink.User.IsEmailVerified;

        /// <inheritdoc />
        public string DisplayName => _authLink.User.DisplayName;

        /// <inheritdoc />
        public bool IsAnonymous => throw new NotImplementedException();

        /// <inheritdoc />
        public string PhoneNumber => _authLink.User.PhoneNumber;

        /// <inheritdoc />
        public Uri PhotoUrl => new Uri(_authLink.User.PhotoUrl.ToString());

        /// <inheritdoc />
        public string Email => _authLink.User.Email;

        /// <inheritdoc />
        public string ProviderId => _authLink.User.FederatedId;

        /// <inheritdoc />
        public IList<string> Providers => throw new NotImplementedException();

        /// <inheritdoc />
        public string Uid => _authLink.User.LocalId;

        /// <inheritdoc />
        public IObservable<PhoneNumberSignInResult> LinkWithPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IObservable<IFirebaseAuthResult> LinkWithPhoneNumber(string verificationId, string verificationCode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IObservable<IFirebaseAuthResult> LinkWithGoogle(string idToken, string accessToken)
        {
            return _authLink
                .LinkToAsync(FirebaseAuthType.Github, accessToken)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <inheritdoc />
        public IObservable<IFirebaseAuthResult> LinkWithFacebook(string accessToken)
        {
            return _authLink
                .LinkToAsync(FirebaseAuthType.Facebook, accessToken)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <inheritdoc />
        public IObservable<IFirebaseAuthResult> LinkWithTwitter(string token, string secret)
        {
            return _authLink
                .LinkToAsync(FirebaseAuthType.Twitter, token)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <inheritdoc />
        public IObservable<IFirebaseAuthResult> LinkWithGithub(string token)
        {
            return _authLink
                .LinkToAsync(FirebaseAuthType.Github, token)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <inheritdoc />
        public IObservable<IFirebaseAuthResult> LinkWithEmail(string email, string password)
        {
            return _authLink
                .LinkToAsync(email, password)
                .ToObservable()
                .Select(authLink => new FirebaseAuthResult(authLink));
        }

        /// <inheritdoc />
        public IObservable<IFirebaseUser> Unlink(string providerId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<string> GetIdTokenAsync(bool forceRefresh)
        {
            return (await _authLink.GetFreshAuthAsync()).FirebaseToken;
        }
    }
}
