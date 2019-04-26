using System;
using System.Globalization;
using Microsoft.MobCAT.Abstractions;

namespace Microsoft.MobCAT.Services
{
    public static class Preferences
    {
        private const string DateFormat = "O";
        private static IPreferencesService _registeredService;
        private static Func<IPreferencesService> _registeredServiceFunc;

        private static IPreferencesService RegisteredService
        {
            get
            {
                if (_registeredService == null)
                {
                    _registeredService = _registeredServiceFunc?.Invoke() ?? ServiceContainer.Resolve<IPreferencesService>(true);
                    if (_registeredService == null)
                    {
                        _registeredService = new InMemoryPreferencesService();
                        Logger.Warn("No preferences provider was registered.  Falling back to the in memory provider.");
                    }
                }
                return _registeredService;
            }
        }

        /// <summary>
        /// Registers the IPreferencesService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(IPreferencesService service)
        {
            ServiceContainer.Register<IPreferencesService>(service);
            _registeredServiceFunc = null;
            _registeredService = service;
        }

        /// <summary>
        /// Lazily registers the IPreferencesService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(Func<IPreferencesService> service)
        {
            ServiceContainer.Register<IPreferencesService>(service);
            _registeredService = null;
            _registeredServiceFunc = service;
        }

        /// <summary>
        /// Gets the string with specified key.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static string GetString(string key, string defaultValue = null)
        => RegisteredService.GetString(key, defaultValue);

        /// <summary>
        /// Sets the string.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SetString(string key, string value)
        => RegisteredService.SetString(key, value);

        /// <summary>
        /// Gets the bool.
        /// </summary>
        /// <returns>The bool value for the key.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static bool GetBool(string key, bool defaultValue = false)
        => RegisteredService.GetBool(key, defaultValue);

        /// <summary>
        /// Sets the bool.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value to store.</param>
        public static void SetBool(string key, bool value)
        => RegisteredService.SetBool(key, value);

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static int GetInt(string key, int defaultValue = 0)
        => RegisteredService.GetInt(key, defaultValue);

        /// <summary>
        /// Sets the int.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SetInt(string key, int value)
        => RegisteredService.SetInt(key, value);

        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <returns>The double.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static double GetDouble(string key, double defaultValue = 0)
        => RegisteredService.GetDouble(key, defaultValue);

        /// <summary>
        /// Sets the double.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SetDouble(string key, double value)
        => RegisteredService.SetDouble(key, value);

        /// <summary>
        /// Gets the float.
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static float GetFloat(string key, float defaultValue = 0)
        => RegisteredService.GetFloat(key, defaultValue);

        /// <summary>
        /// Sets the float.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SetFloat(string key, float value)
        => RegisteredService.SetFloat(key, value);

        /// <summary>
        /// Gets the enum.
        /// </summary>
        /// <returns>The enum.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetEnum<T>(string key, T defaultValue)
        {
            string valueAsString = GetString(key);
            if (valueAsString != null)
            {
                try
                {
                    return (T)Enum.Parse(defaultValue.GetType(), valueAsString);
                }
                catch (Exception)
                {
                    Logger.Warn("An error occured getting an preferences value as an enumeration.  Key: {0}  Value: {1}", key, valueAsString);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets the enum.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void SetEnum<T>(string key, T value)
        {
            string valueAsString = value.ToString();
            SetString(key, valueAsString);
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <returns>The date.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static DateTime GetDate(string key, DateTime defaultValue)
        {
            string valueAsString = GetString(key);
            if (valueAsString != null)
            {
                DateTime.TryParseExact(valueAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var date);
                return date;
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets the date.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SetDate(string key, DateTime value)
        {
            string valueAsString = value.ToString(DateFormat, CultureInfo.InvariantCulture);
            SetString(key, valueAsString);
        }
    }
}
