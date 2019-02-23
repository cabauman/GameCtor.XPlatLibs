using System;

namespace GameCtor.FirebaseAuth.Mobile
{
    /// <summary>
    /// Cross platform Firebase authentication
    /// </summary>
    public static class CrossFirebaseAuth
    {
        private static Lazy<IFirebaseAuthService> implementation = new Lazy<IFirebaseAuthService>(() => CreateFirebaseAuth(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets a value indicating whether the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Gets the current plugin implementation to use.
        /// </summary>
        public static IFirebaseAuthService Current
        {
            get
            {
                IFirebaseAuthService ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        private static IFirebaseAuthService CreateFirebaseAuth()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new FirebaseAuthService();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly." +
                "You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
}
