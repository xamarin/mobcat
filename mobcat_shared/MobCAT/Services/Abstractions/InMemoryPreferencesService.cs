using System;
using System.Collections.Concurrent;

namespace Microsoft.MobCAT.Abstractions
{
    public class InMemoryPreferencesService : IPreferencesService
    {
        private readonly ConcurrentDictionary<string, object> _preferences = new ConcurrentDictionary<string, object>();

        /// <inheritdoc />
        public string GetString(string key, string defaultValue)
        {
            if (_preferences.TryGetValue(key, out var value))
            {
                if (value != null)
                    return value.ToString();
            }

            return defaultValue;
        }

        /// <inheritdoc />
        public void SetString(string key, string value)
        {
            if (value == null)
                _preferences.TryRemove(key, out var previousValue);
            else
                _preferences[key] = value;
        }

        /// <inheritdoc />
        public bool GetBool(string key, bool defaultValue)
        {
            if (_preferences.TryGetValue(key, out var value))
            {
                if (value != null)
                    return (bool)value;
            }

            return defaultValue;
        }

        /// <inheritdoc />
        public void SetBool(string key, bool value)
        {
            _preferences[key] = value;
        }

        /// <inheritdoc />
        public int GetInt(string key, int defaultValue)
        {
            if (_preferences.TryGetValue(key, out var value))
            {
                if (value != null)
                    return (int)value;
            }

            return defaultValue;
        }

        /// <inheritdoc />
        public void SetInt(string key, int value)
        {
            _preferences[key] = value;
        }

        /// <inheritdoc />
        public double GetDouble(string key, double defaultValue)
        {
            if (_preferences.TryGetValue(key, out var value))
            {
                if (value != null)
                    return (double)value;
            }

            return defaultValue;
        }

        /// <inheritdoc />
        public void SetDouble(string key, double value)
        {
            _preferences[key] = value;
        }

        /// <inheritdoc />
        public float GetFloat(string key, float defaultValue)
        {
            if (_preferences.TryGetValue(key, out var value))
            {
                if (value != null)
                    return (float)value;
            }

            return defaultValue;
        }

        /// <inheritdoc />
        public void SetFloat(string key, float value)
        {
            _preferences[key] = value;
        }
    }
}
