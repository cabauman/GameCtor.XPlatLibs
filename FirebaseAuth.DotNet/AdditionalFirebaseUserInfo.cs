using Firebase.Auth;
using System;
using System.Collections.Generic;

namespace GameCtor.FirebaseAuth
{
    public class AdditionalFirebaseUserInfo : IAdditionalFirebaseUserInfo
    {
        public AdditionalFirebaseUserInfo()
        {
        }

        public string ProviderId => throw new NotImplementedException();

        public string Username => throw new NotImplementedException();
    }
}
