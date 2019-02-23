using System.Threading.Tasks;
using Firebase.Auth;

namespace GameCtor.FirebaseAuth.Mobile
{
    public class FirebaseAuthResult : IFirebaseAuthResult
    {
        private IAuthResult _authResult;

        public FirebaseAuthResult(IAuthResult authResult)
        {
            _authResult = authResult;
        }

        /// <inheritdoc/>
        public IFirebaseUser User => new FirebaseUser(_authResult.User);

        /// <inheritdoc/>
        public IAdditionalFirebaseUserInfo AdditionalUserInfo => new AdditionalFirebaseUserInfo(_authResult.AdditionalUserInfo);
    }
}
