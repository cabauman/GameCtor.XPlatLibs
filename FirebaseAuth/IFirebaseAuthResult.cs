using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth
{
    public interface IFirebaseAuthResult
    {
        IFirebaseUser User { get; }

        IAdditionalFirebaseUserInfo AdditionalUserInfo { get; }
    }
}
