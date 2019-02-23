using System;
using System.Reactive;

namespace GameCtor.XamarinAuth
{
    public interface IAuthService
    {
        /// <summary>
        /// Gets an observable that emits an auth token if sign-in was successful.
        /// </summary>
        IObservable<string> SignInSuccessful { get; }

        /// <summary>
        /// Gets an observable that emits if sign-in was canceled.
        /// </summary>
        IObservable<Unit> SignInCanceled { get; }

        /// <summary>
        /// Gets an observable that emits an exception if sign-in failed.
        /// </summary>
        IObservable<Exception> SignInFailed { get; }

        /// <summary>
        /// Presents the Google sign-in UI.
        /// </summary>
        /// <param name="clientId">The client ID.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="authorizeUrl">The authorize URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="accessTokenUrl">The access token URL.</param>
        void TriggerGoogleAuthFlow(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl);

        /// <summary>
        /// Presents the Facebook sign-in UI.
        /// </summary>
        /// <param name="clientId">The client ID.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="authorizeUrl">The authorize URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="accessTokenUrl">The access token URL.</param>
        void TriggerFacebookAuthFlow(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl);

        /// <summary>
        /// Presents the Github sign-in UI.
        /// </summary>
        /// <param name="clientId">The client ID.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="authorizeUrl">The authorize URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="accessTokenUrl">The access token URL.</param>
        void TriggerGithubAuthFlow(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl);

        /// <summary>
        /// Presents the LinkedIn sign-in UI.
        /// </summary>
        /// <param name="clientId">The client ID.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="authorizeUrl">The authorize URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="accessTokenUrl">The access token URL.</param>
        void TriggerLinkedInAuthFlow(
            string clientId,
            string clientSecret,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string accessTokenUrl);

        /// <summary>
        /// Presents the Twitter sign-in UI.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="requestTokenUrl">The request token URL.</param>
        /// <param name="authorizeUrl">The authorize URL.</param>
        /// <param name="accessTokenUrl">The access token URL.</param>
        /// <param name="callbackUrl">The callback URL.</param>
        void TriggerTwitterAuthFlow(
            string consumerKey,
            string consumerSecret,
            string requestTokenUrl,
            string authorizeUrl,
            string accessTokenUrl,
            string callbackUrl);
    }
}