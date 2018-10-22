using System;
using System.IO;
using System.Reactive;
using System.Threading;
using Firebase.Storage;

namespace GameCtor.FirebaseStorage.DotNet
{
    public interface IFirebaseStorageService
    {
        IObservable<string> GetDownloadUrl(string path);

        IObservable<Unit> Delete(string path);

        IObservable<Either<int, string>> Upload(string path, Stream stream, string mimeType = null);

        //FirebaseStorageTask Upload(string path, Stream stream);

        //FirebaseStorageTask Upload(string path, Stream stream, CancellationToken ct, string mimeType = null);
    }
}