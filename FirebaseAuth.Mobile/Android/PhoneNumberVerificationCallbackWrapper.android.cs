using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.Mobile
{
    public partial class FirebaseAuthImplementation
    {
        private class PhoneNumberVerificationCallbackWrapper : PhoneAuthProvider.OnVerificationStateChangedCallbacks
        {
            private TaskCompletionSource<PhoneNumberVerificationResult> _tcs;

            public Task<PhoneNumberVerificationResult> Verify()
            {
                _tcs = new TaskCompletionSource<PhoneNumberVerificationResult>();
                return _tcs.Task;
            }

            public override void OnVerificationCompleted(PhoneAuthCredential credential)
            {
                var result = new PhoneNumberVerificationResult
                {
                    AuthCredential = credential
                };

                _tcs.SetResult(result);
            }

            public override void OnVerificationFailed(FirebaseException exception)
            {
                _tcs.SetException(exception);
            }

            public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
            {
                var result = new PhoneNumberVerificationResult
                {
                    VerificationId = verificationId
                };

                _tcs.SetResult(result);
            }
        }

        private class PhoneNumberVerificationResult
        {
            public AuthCredential AuthCredential { get; set; }

            public string VerificationId { get; set; }
        }
    }
}
