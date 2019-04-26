using System;
using System.Linq;

namespace Microsoft.MobCAT.Services
{
    public interface ISecureStorageService
    {
        /// <summary>
        /// Get the value for specified key.
        /// </summary>
        /// <returns>The value for specified key.</returns>
        /// <param name="key">Key.</param>
        string Get(string key);

        /// <summary>
        /// Set the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <param name="requireBiometricAuth">True if record should be secured with biometric auth</param>
        void Set(string key, string value, bool requireBiometricAuth = false);

        /// <summary>
        /// Delete the value for the specified key.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="key">Key.</param>
        bool Delete(string key);

        /// <summary>
        /// Check a specificed key exists.
        /// </summary>
        /// <returns>True if the specified key exists.</returns>
        /// <param name="key">Key.</param>
        bool Exists(string key);
    }

    public static class SecureStorageServiceExtensions
    {
        /// <summary>
        /// Set the specified key and username/password pair.
        /// </summary>
        /// <param name="service">ISecureStorageService instance.</param>
        /// <param name="key">Key.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="requireBiometricAuth">If set to <c>true</c> require biometric auth.</param>
        public static void SetAccount(this ISecureStorageService service, string key, string username, string password, bool requireBiometricAuth = false)
        {
            var account = $"{username.Length};{username}{password}";
            service.Set(key, account, requireBiometricAuth);
        }

        /// <summary>
        /// Gets the account for the specified key.
        /// </summary>
        /// <returns>The account with Item1 as username and Item2 as password.</returns>
        /// <param name="service">ISecureStorageService instance.</param>
        /// <param name="key">Key.</param>
        public static Tuple<string, string> GetAccount(this ISecureStorageService service, string key)
        {
            var record = service.Get(key);

            if (string.IsNullOrWhiteSpace(record))
                return null;

            var account = record.Split(new char[] { ';' }, 2);

            if (int.TryParse(account[0], out int usernameLength))
            {
                var credentials = account.ElementAtOrDefault(1);

                if (string.IsNullOrWhiteSpace(credentials))
                    return null;

                var username = credentials.Substring(0, usernameLength);
                var password = credentials.Substring(usernameLength, credentials.Length - usernameLength);

                return new Tuple<string, string>(username, password);
            }

            return null;
        }
    }
}