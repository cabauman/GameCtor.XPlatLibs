using System;

namespace GameCtor.FirebaseAnalytics
{
    /// <summary>
    /// Cross FirebaseAnalytics
    /// </summary>
    public static class CrossFirebaseAnalytics
    {
        private static Lazy<IFirebaseAnalyticsService> implementation = new Lazy<IFirebaseAnalyticsService>(() => CreateFirebaseAnalytics(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets a value indicating whether the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Gets the current plugin implementation to use.
        /// </summary>
        public static IFirebaseAnalyticsService Current
        {
            get
            {
                IFirebaseAnalyticsService ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        private static IFirebaseAnalyticsService CreateFirebaseAnalytics()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new FirebaseAnalyticsService();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
}
