using Firebase.Auth;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.Mobile
{
    public class FirebaseAuthResult : IFirebaseAuthResult
    {
        private AuthDataResult _authResult;

        public FirebaseAuthResult(AuthDataResult authResult)
        {
            _authResult = authResult;
        }

        public IFirebaseUser User => new FirebaseUser(_authResult.User);

        public IAdditionalFirebaseUserInfo AdditionalUserInfo => new AdditionalFirebaseUserInfo(_authResult.AdditionalUserInfo);
    }
}
