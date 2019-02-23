using System.Collections.Generic;
using Firebase.Auth;

namespace GameCtor.FirebaseAuth.Mobile
{
    /// <summary>
    /// Android implementation of the IAdditionalFirebaseUserInfo interface.
    /// </summary>
    public class AdditionalFirebaseUserInfo : IAdditionalFirebaseUserInfo
    {
        private IAdditionalUserInfo _additionalUserInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalFirebaseUserInfo"/> class.
        /// </summary>
        /// <param name="additionalUserInfo">Additional user info.</param>
        public AdditionalFirebaseUserInfo(IAdditionalUserInfo additionalUserInfo)
        {
            _additionalUserInfo = additionalUserInfo;
        }

        //public IDictionary<string, object> Profile => _additionalUserInfo.Profile;

        /// <inheritdoc/>
        public string ProviderId => _additionalUserInfo.ProviderId;

        /// <inheritdoc/>
        public string Username => _additionalUserInfo.Username;
    }
}
