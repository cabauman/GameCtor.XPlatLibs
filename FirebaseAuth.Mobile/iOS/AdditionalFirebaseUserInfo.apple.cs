using System.Collections.Generic;
using Firebase.Auth;

namespace GameCtor.FirebaseAuth.Mobile
{
    public class AdditionalFirebaseUserInfo : IAdditionalFirebaseUserInfo
    {
        private AdditionalUserInfo _additionalUserInfo;

        public AdditionalFirebaseUserInfo(AdditionalUserInfo additionalUserInfo)
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
