using System;
using System.Reactive;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth
{
    public interface IFirebaseAuthService
    {
        bool IsAuthenticated { get; }

        bool IsPhoneNumberLinkedToAccount { get; }

        Task<string> GetIdTokenAsync();

        IObservable<Unit> SignInWithGoogle(string accessToken);

        //IObservable<Unit> SignInWithGoogle(string idToken, string accessToken);

        IObservable<Unit> SignInWithFacebook(string accessToken);

        IObservable<Unit> SignInWithTwitter(string token, string secret);

        IObservable<Unit> SignInWithGithub(string token);

        IObservable<Unit> SignInWithEmail(string email, string password);

        IObservable<PhoneNumberVerificationResult> SignInWithPhoneNumber(string phoneNumber);

        IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode);

        IObservable<Unit> SignInAnonymously();

        IObservable<PhoneNumberVerificationResult> LinkPhoneNumberToCurrentUser(string phoneNumber);

        IObservable<Unit> LinkPhoneNumberToCurrentUser(string verificationId, string verificationCode);

        void SignOut();
    }
}