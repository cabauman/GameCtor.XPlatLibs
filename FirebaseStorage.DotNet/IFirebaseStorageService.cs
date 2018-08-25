using System;
using System.Threading.Tasks;

namespace GameCtor.FirebaseStorage.DotNet
{
    public interface IFirebaseStorageService
    {
        IObservable<string> GetDownloadUrl(string filename);
    }
}