using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Plugin.CurrentActivity;

namespace GameCtor.FirebaseAnalytics
{
    /// <summary>
    /// Android implementation of Firebase Analytics.
    /// </summary>
    public class FirebaseAnalyticsService : IFirebaseAnalyticsService
    {
        private readonly Firebase.Analytics.FirebaseAnalytics _firebaseAnalytics;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirebaseAnalyticsService"/> class.
        /// </summary>
        public FirebaseAnalyticsService()
        {
            Context context = CrossCurrentActivity.Current.Activity;
            if (context == null)
            {
                throw new NullReferenceException("Initialize CrossCurrentActivity before using Firebase Analytics.");
            }

            _firebaseAnalytics = Firebase.Analytics.FirebaseAnalytics.GetInstance(context);
        }

        /// <inheritdoc/>
        public void SetUserId(string id)
        {
            _firebaseAnalytics.SetUserId(id);
        }

        /// <inheritdoc/>
        public void SetUserProperty(string name, string value)
        {
            _firebaseAnalytics.SetUserProperty(name, value);
        }

        /// <inheritdoc/>
        public void LogEvent(string name, IDictionary<string, object> parameters)
        {
            Bundle bundle = new Bundle();
            foreach (var item in parameters)
            {
                if (item.Value.GetType() == typeof(string))
                {
                    bundle.PutString(item.Key, item.Value.ToString());
                }
                else if (item.Value.GetType() == typeof(bool))
                {
                    bundle.PutBoolean(item.Key, (bool)item.Value);
                }
            }

            _firebaseAnalytics.LogEvent(name, bundle);
        }
    }
}
