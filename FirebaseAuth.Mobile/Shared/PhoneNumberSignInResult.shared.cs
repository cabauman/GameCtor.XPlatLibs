namespace GameCtor.FirebaseAuth.Mobile
{
    public class PhoneNumberSignInResult
    {
        public IFirebaseAuthResult AuthResult { get; set; }

        public string VerificationId { get; set; }
    }
}
