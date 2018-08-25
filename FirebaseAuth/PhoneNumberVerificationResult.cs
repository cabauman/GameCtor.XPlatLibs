namespace GameCtor.FirebaseAuth
{
    public struct PhoneNumberVerificationResult
    {
        public PhoneNumberVerificationResult(bool authenticated, string verificationId)
        {
            Authenticated = authenticated;
            VerificationId = verificationId;
        }

        public bool Authenticated { get; }

        public string VerificationId { get; }
    }
}