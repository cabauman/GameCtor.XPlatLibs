using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth
{
    public interface IFirebaseAuthResult
    {
        /// <summary>
        /// Gets ther user.
        /// </summary>
        IFirebaseUser User { get; }

        /// <summary>
        /// Gets additional user info.
        /// </summary>
        IAdditionalFirebaseUserInfo AdditionalUserInfo { get; }
    }
}
