using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Firebase;
using Firebase.Auth;

namespace GameCtor.FirebaseAuth.Mobile
{
    internal class PhoneNumberVerificationCallbackWrapper : PhoneAuthProvider.OnVerificationStateChangedCallbacks
    {
        private Subject<PhoneNumberVerificationResult> _subject = new Subject<PhoneNumberVerificationResult>();

        public IObservable<PhoneNumberVerificationResult> Verify()
        {
            return _subject.AsObservable();
        }

        public override void OnVerificationCompleted(PhoneAuthCredential credential)
        {
            var result = new PhoneNumberVerificationResult
            {
                AuthCredential = credential
            };

            _subject.OnNext(result);
            _subject.OnCompleted();
        }

        public override void OnVerificationFailed(FirebaseException exception)
        {
            FirebaseAuthService.FirebaseExceptionTypeToEnumDict.TryGetValue(exception.GetType(), out FirebaseAuthExceptionType exceptionType);
            var customException = new FirebaseAuthException(exception.Message, exception, exceptionType);
            _subject.OnError(customException);
        }

        public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
        {
            var result = new PhoneNumberVerificationResult
            {
                VerificationId = verificationId
            };

            _subject.OnNext(result);
            _subject.OnCompleted();
        }
    }

    internal class PhoneNumberVerificationResult
    {
        public AuthCredential AuthCredential { get; set; }

        public string VerificationId { get; set; }
    }
}
