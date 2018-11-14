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
        /// <returns>The set.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void Set(string key, string value);

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
}