using System;

namespace GameCtor.Repository
{
    public interface IFirebaseRepository<T> : IRepository<T>
    {
        IObservable<FirebaseEvent<T>> Observe();
    }
}
