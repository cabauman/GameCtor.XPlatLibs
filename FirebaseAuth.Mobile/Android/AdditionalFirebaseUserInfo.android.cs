using Firebase.Auth;
using System.Collections.Generic;

namespace GameCtor.FirebaseAuth.Mobile
{
    public class AdditionalFirebaseUserInfo : IAdditionalFirebaseUserInfo
    {
        private IAdditionalUserInfo _additionalUserInfo;

        public AdditionalFirebaseUserInfo(IAdditionalUserInfo additionalUserInfo)
        {
            _additionalUserInfo = additionalUserInfo;
        }

        //public IDictionary<string, object> Profile => _additionalUserInfo.Profile;

        public string ProviderId => _additionalUserInfo.ProviderId;

        public string Username => _additionalUserInfo.Username;
    }
}
