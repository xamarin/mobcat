using System;

namespace MobCAT.ClientSecrets
{
    /// <summary>
    /// Specifies that the field value should be replaced with the corresponding environment variable at built-time.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ClientSecretAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSecretAttribute"/> class.
        /// </summary>
        /// <remarks>The name of the field is used to resolve an environment variable containing the new value for that field where no key is provided explicitly.</remarks>
        public ClientSecretAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSecretAttribute"/> class.
        /// </summary>
        /// <param name="key">The key that is used to resolve an environment variable containing the new value for that field.</param>
        public ClientSecretAttribute(string key)
        {
        }
    }
}