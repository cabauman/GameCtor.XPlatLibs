using System;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace GameCtor.FirebaseStorage.DotNet
{
    public interface IFirebaseStorageService
    {
        IObservable<string> GetDownloadUrl(string path);

        IObservable<Unit> Delete(string path);

        Task Upload(string path, Stream stream);

        Task Upload(string path, Stream stream, CancellationToken ct, string mimeType = null);
    }
}