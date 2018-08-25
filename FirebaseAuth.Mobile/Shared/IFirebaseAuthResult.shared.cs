using System.Threading.Tasks;

namespace GameCtor.FirebaseAuth.Mobile
{
    public interface IFirebaseAuthResult
    {
        IFirebaseUser User { get; }

        IAdditionalFirebaseUserInfo AdditionalUserInfo { get; }
    }
}
