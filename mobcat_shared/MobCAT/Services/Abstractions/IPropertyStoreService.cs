using System.Threading.Tasks;

namespace Microsoft.MobCAT.Services.Abstractions
{
    public interface IPropertyStoreService
    {
        /// <summary>
        /// Get the value for specified key.
        /// </summary>
        /// <returns>The value for specified key.</returns>
        /// <param name="key">Key.</param>
        T GetValue<T>(string key);

        /// <summary>
        /// Get the value for specified key or sets a new value if one does not currently exist.
        /// </summary>
        /// <returns>The value for specified key if a value does not already exist.</returns>
        /// <param name="key">Key.</param>
        T GetValueOrSetDefault<T>(string key, T defaultValue);

        /// <summary>
        /// Set the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void SetValue<T>(string key, T value);

        /// <summary>
        /// Delete the value for the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        void DeleteValue(string key);

        /// <summary>
        /// Asynchronously requests the underlying property store persists the current state.
        /// </summary>
        /// <remarks>The store should persist automatically (for the lifetime of the application only). This requests the action explicitly.</remarks>
        /// <returns>The Task.</returns>
        Task SaveAsync();
    }
}