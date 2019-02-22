using Firebase.Storage;
using Newtonsoft.Json.Linq;

namespace GameCtor.FirebaseStorage.DotNet
{
    public static class FirebaseStorageExceptionExtensions
    {
        public static string GetErrorCode(this FirebaseStorageException @this)
        {
            var o = JObject.Parse(@this.ResponseData);
            return o["error"]["code"].ToString();
        }

        public static bool ObjectNotFound(this FirebaseStorageException @this)
        {
            return @this.GetErrorCode() == "404";
        }
    }
}
