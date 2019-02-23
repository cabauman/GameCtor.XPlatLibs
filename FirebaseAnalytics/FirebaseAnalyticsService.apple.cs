using System;
using System.Collections.Generic;
using Firebase.Analytics;
using Foundation;

namespace GameCtor.FirebaseAnalytics
{
    /// <summary>
    /// iOS implementation of Firebase Analytics.
    /// </summary>
    public class FirebaseAnalyticsService : IFirebaseAnalyticsService
    {
        /// <inheritdoc/>
        public void SetUserId(string id)
        {
            Analytics.SetUserId(id);
        }

        /// <inheritdoc/>
        public void SetUserProperty(string name, string value)
        {
            Analytics.SetUserProperty(name, value);
        }

        /// <inheritdoc/>
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
