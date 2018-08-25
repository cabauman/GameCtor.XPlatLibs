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

        IObservable<Unit> SignInWithFacebook(string authToken);

        IObservable<Unit> SignInWithGoogle(string authToken);

        IObservable<PhoneNumberVerificationResult> SignInWithPhoneNumber(string phoneNumber);

        IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode);

        IObservable<Unit> SignInAnonymously();

        IObservable<PhoneNumberVerificationResult> LinkPhoneNumberToCurrentUser(string phoneNumber);

        IObservable<Unit> LinkPhoneNumberToCurrentUser(string verificationId, string verificationCode);

        void SignOut();
    }
}