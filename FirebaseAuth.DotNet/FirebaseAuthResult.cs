using Firebase.Auth;
using System;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.DotNet
{
    public class FirebaseAuthResult : IFirebaseAuthResult
    {
        private FirebaseAuthLink _authLink;

        public FirebaseAuthResult(FirebaseAuthLink authLink)
        {
            _authLink = authLink;
        }

        public IFirebaseUser User
        {
            get { return new FirebaseUser(_authLink); }
        }

        public IAdditionalFirebaseUserInfo AdditionalUserInfo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
