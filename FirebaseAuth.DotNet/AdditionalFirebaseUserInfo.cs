using System;
using System.Collections.Generic;
using Firebase.Auth;

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
