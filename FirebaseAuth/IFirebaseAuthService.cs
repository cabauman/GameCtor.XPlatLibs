using System;
using System.Reactive;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth
{
    public interface IFirebaseAuthService
    {
        IFirebaseUser CurrentUser { get; }

        IObservable<bool> IsAuthenticated { get; }

        Task<string> GetIdTokenAsync();

        IObservable<Unit> SignInWithGoogle(string idToken, string accessToken);

        IObservable<Unit> SignInWithFacebook(string accessToken);

        IObservable<Unit> SignInWithTwitter(string token, string secret);

        IObservable<Unit> SignInWithGithub(string token);

        IObservable<Unit> SignInWithEmail(string email, string password);

        IObservable<PhoneNumberSignInResult> SignInWithPhoneNumber(string phoneNumber);

        IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode);

        IObservable<Unit> SignInAnonymously();

        void SignOut();
    }
}