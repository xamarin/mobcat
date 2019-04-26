using System;
namespace Microsoft.MobCAT.Abstractions
{
    public interface IPreferencesService
    {
        /// <summary>
        /// Gets the string with specified key.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        string GetString(string key, string defaultValue);

        /// <summary>
        /// Sets the string.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void SetString(string key, string value);

        /// <summary>
        /// Gets the bool.
        /// </summary>
        /// <returns>The bool value for the key.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        bool GetBool(string key, bool defaultValue);

        /// <summary>
        /// Sets the bool.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value to store.</param>
        void SetBool(string key, bool value);

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        int GetInt(string key, int defaultValue);

        /// <summary>
        /// Sets the int.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void SetInt(string key, int value);

        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <returns>The double.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        double GetDouble(string key, double defaultValue);

        /// <summary>
        /// Sets the double.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void SetDouble(string key, double value);

        /// <summary>
        /// Gets the float.
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        float GetFloat(string key, float defaultValue);

        /// <summary>
        /// Sets the float.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void SetFloat(string key, float value);
    }
}
