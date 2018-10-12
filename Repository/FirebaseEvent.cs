namespace GameCtor.Repository
{
    public class FirebaseEvent<T>
    {
        public FirebaseEvent(string key, T obj, FirebaseEventSource eventSource, FirebaseEventType eventType)
        {
            Key = key;
            Object = obj;
            EventSource = eventSource;
            EventType = eventType;
        }

        public string Key { get; }

        public T Object { get; }

        public FirebaseEventSource EventSource { get; }

        public FirebaseEventType EventType { get; }
    }
}
