using System.Collections.Generic;

namespace GameCtor.FirebaseAuth.Mobile
{
    public interface IAdditionalFirebaseUserInfo
    {
        //IDictionary<string, object> Profile { get; }

        string ProviderId { get; }

        string Username { get; }
    }
}
