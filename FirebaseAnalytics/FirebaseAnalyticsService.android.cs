using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Plugin.CurrentActivity;

namespace GameCtor.FirebaseAnalytics
{
    /// <summary>
    /// Android implementation of Firebase Analytics
    /// </summary>
    public class FirebaseAnalyticsService : IFirebaseAnalyticsService
    {
        private readonly Firebase.Analytics.FirebaseAnalytics _firebaseAnalytics;

        /// <summary>
        /// Creates an instance of FirebaseAnalyticsService.
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

        /// <summary>
        /// Sets the user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        public void SetUserId(string id)
        {
            _firebaseAnalytics.SetUserId(id);
        }

        /// <summary>
        /// Sets a user property.
        /// </summary>
        /// <param name="name">The name of a user property.</param>
        /// <param name="value">The value of a user property.</param>
        public void SetUserProperty(string name, string value)
        {
            _firebaseAnalytics.SetUserProperty(name, value);
        }

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="name">The name of an event.</param>
        /// <param name="parameters">A dictionary of event parameters.</param>
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
