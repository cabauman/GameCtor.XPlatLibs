using System.Collections.Generic;

namespace GameCtor.FirebaseAuth
{
    public interface IAdditionalFirebaseUserInfo
    {
        //IDictionary<string, object> Profile { get; }

        /// <summary>
        /// Gets the provider ID.
        /// </summary>
        string ProviderId { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        string Username { get; }
    }
}
