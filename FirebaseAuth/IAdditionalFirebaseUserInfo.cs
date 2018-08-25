using System.Collections.Generic;

namespace GameCtor.FirebaseAuth
{
    public interface IAdditionalFirebaseUserInfo
    {
        //IDictionary<string, object> Profile { get; }

        string ProviderId { get; }

        string Username { get; }
    }
}
