using System;
using System.IO;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace GameCtor.FirebaseStorage.DotNet
{
    public class FirebaseStorageService
    {
        Firebase.Storage.FirebaseStorage _storage;

        public FirebaseStorageService(Firebase.Storage.FirebaseStorage storage)
        {
            _storage = storage;
        }

        public IObservable<string> GetDownloadUrl(string path)
        {
            return _storage
                .Child(path)
                .GetDownloadUrlAsync()
                .ToObservable();
        }

        public IObservable<Unit> Delete(string path)
        {
            return _storage
                .Child(path)
                .DeleteAsync()
                .ToObservable();
        }

        public async Task Upload(string path, Stream stream, CancellationToken ct, string mimeType = null)
        {
            await _storage
                .Child(path)
                .PutAsync(stream, ct, mimeType);
        }
    }
}