namespace GameCtor.FirebaseAuth
{
    public struct PhoneNumberSignInResult
    {
        public IFirebaseAuthResult AuthResult { get; set; }

        public string VerificationId { get; set; }
    }
}
