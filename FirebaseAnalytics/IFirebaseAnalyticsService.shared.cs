using System.Collections.Generic;

namespace GameCtor.FirebaseAnalytics
{
    /// <summary>
    /// Firebase Analytics interface
    /// </summary>
    public interface IFirebaseAnalyticsService
    {
        /// <summary>
        /// Sets the user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        void SetUserId(string id);

        /// <summary>
        /// Sets a user property.
        /// </summary>
        /// <param name="name">The name of a user property.</param>
        /// <param name="value">The value of a user property.</param>
        void SetUserProperty(string name, string value);

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="name">The name of an event.</param>
        /// <param name="parameters">A dictionary of event parameters.</param>
        void LogEvent(string name, IDictionary<string, object> parameters);
    }
}
