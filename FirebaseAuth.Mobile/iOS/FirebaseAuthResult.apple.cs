using System.Threading.Tasks;
using Firebase.Auth;

namespace GameCtor.FirebaseAuth.Mobile
{
    public class FirebaseAuthResult : IFirebaseAuthResult
    {
        private AuthDataResult _authResult;

        public FirebaseAuthResult(AuthDataResult authResult)
        {
            _authResult = authResult;
        }

        /// <inheritdoc/>
        public IFirebaseUser User => new FirebaseUser(_authResult.User);

        /// <inheritdoc/>
        public IAdditionalFirebaseUserInfo AdditionalUserInfo => new AdditionalFirebaseUserInfo(_authResult.AdditionalUserInfo);
    }
}
