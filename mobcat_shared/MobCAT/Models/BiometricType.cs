using System;
namespace Microsoft.MobCAT.Models
{
    public enum BiometricType
    {
        /// <summary>
        /// Fingerprint authentication
        /// </summary>
        Fingerprint,
        /// <summary>
        /// Face authentication
        /// </summary>
        Face,
        /// <summary>
        /// Passcord authentication. This is a fallback option on iOS
        /// </summary>
        Passcode,
        /// <summary>
        /// No biometric authentication
        /// </summary>
        None
    }
}
