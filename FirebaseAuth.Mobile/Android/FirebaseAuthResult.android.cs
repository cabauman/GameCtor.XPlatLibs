using Firebase.Auth;
using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.Mobile
{
    public class FirebaseAuthResult : IFirebaseAuthResult
    {
        private IAuthResult _authResult;

        public FirebaseAuthResult(IAuthResult authResult)
        {
            _authResult = authResult;
        }

        public IFirebaseUser User
        {
            get { return new FirebaseUser(_authResult.User); }
        }

        public IAdditionalFirebaseUserInfo AdditionalUserInfo
        {
            get { return new AdditionalFirebaseUserInfo(_authResult.AdditionalUserInfo); }
        }
    }
}
