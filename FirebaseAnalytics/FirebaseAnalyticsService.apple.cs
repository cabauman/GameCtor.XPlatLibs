using System;
using System.Collections.Generic;
using Firebase.Analytics;
using Foundation;

namespace GameCtor.FirebaseAnalytics
{
    /// <summary>
    /// iOS implementation of Firebase Analytics
    /// </summary>
    public class FirebaseAnalyticsService : IFirebaseAnalyticsService
    {
        /// <summary>
        /// Sets the user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        public void SetUserId(string id)
        {
            Analytics.SetUserId(id);
        }

        /// <summary>
        /// Sets a user property.
        /// </summary>
        /// <param name="name">The name of a user property.</param>
        /// <param name="value">The value of a user property.</param>
        public void SetUserProperty(string name, string value)
        {
            Analytics.SetUserProperty(name, value);
        }

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="name">The name of an event.</param>
        /// <param name="parameters">A dictionary of event parameters.</param>
        public void LogEvent(string name, IDictionary<string, object> parameters)
        {
            var dict = new NSDictionary<NSString, NSObject>();
            foreach (var item in parameters)
            {
                if (item.Value.GetType() == typeof(string))
                {
                    dict[item.Key] = new NSString(item.Value.ToString());
                }
                else if (item.Value.GetType() == typeof(bool))
                {
                    dict[item.Key] = new NSNumber((bool)item.Value);
                }
            }

            Analytics.LogEvent(name, dict);
        }
    }
}
