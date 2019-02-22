using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Storage;

namespace GameCtor.FirebaseStorage.DotNet
{
    public class FirebaseStorageService : IFirebaseStorageService
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

        public IObservable<Unit> Delete(string path, bool ignoreNotFoundException = true)
        {
            return _storage
                .Child(path)
                .DeleteAsync()
                .ToObservable()
                .Catch<Unit, FirebaseStorageException>(
                    ex =>
                        ex.ObjectNotFound() && ignoreNotFoundException ?
                        Observable.Return(Unit.Default) :
                        Observable.Throw<Unit>(ex));
        }

        public IObservable<Either<int, string>> Upload(
            string path,
            Stream stream,
            string mimeType = null)
        {
            return Observable.Create<Either<int, string>>(
                async (observer, ct) =>
                {
                    var task = _storage
                        .Child(path)
                        .PutAsync(stream, ct, mimeType);

                    var progressSubscription = Observable.FromEventPattern<FirebaseStorageProgress>(
                        h => task.Progress.ProgressChanged += h,
                        h => task.Progress.ProgressChanged -= h)
                            .Select(x => x.EventArgs.Percentage)
                            .Subscribe(x => observer.OnNext(Either.Left<int, string>(x)));

                    try
                    {
                        observer.OnNext(Either.Right<int, string>(await task));
                        observer.OnCompleted();
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                    }

                    return progressSubscription;
                });
        }
    }
}