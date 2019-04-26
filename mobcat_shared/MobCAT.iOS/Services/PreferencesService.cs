using System;
using Foundation;
using Microsoft.MobCAT.Abstractions;
using Microsoft.MobCAT.iOS.Extensions;

namespace Microsoft.MobCAT.iOS.Services
{
    public class PreferencesService : IPreferencesService
    {
        private readonly NSUserDefaults _standardDefaults;

        public PreferencesService()
        {
            _standardDefaults = NSUserDefaults.StandardUserDefaults;
        }

        /// <inheritdoc />
        public string GetString(string key, string defaultValue)
        {
            string value = _standardDefaults.StringForKey(key);
            if (value == null)
            {
                return defaultValue;
            }

            return value;
        }

        /// <inheritdoc />
        public void SetString(string key, string value)
        {
            if (value == null)
            {
                _standardDefaults.RemoveObject(key);
            }
            else
            {
                _standardDefaults.SetString(value, key);
            }
            _standardDefaults.Synchronize();
        }

        /// <inheritdoc />
        public bool GetBool(string key, bool defaultValue)
        {
            return _standardDefaults.BoolForKey(defaultValue, key);
        }

        /// <inheritdoc />
        public void SetBool(string key, bool value)
        {
            _standardDefaults.SetBool(value, key);
            _standardDefaults.Synchronize();
        }

        /// <inheritdoc />
        public int GetInt(string key, int defaultValue)
        {
            return _standardDefaults.IntForKey(defaultValue, key);
        }

        /// <inheritdoc />
        public void SetInt(string key, int value)
        {
            _standardDefaults.SetInt(value, key);
            _standardDefaults.Synchronize();
        }

        /// <inheritdoc />
        public double GetDouble(string key, double defaultValue)
        {
            return _standardDefaults.DoubleForKey(defaultValue, key);
        }

        /// <inheritdoc />
        public void SetDouble(string key, double value)
        {
            _standardDefaults.SetDouble(value, key);
            _standardDefaults.Synchronize();
        }

        /// <inheritdoc />
        public float GetFloat(string key, float defaultValue)
        {
            return _standardDefaults.FloatForKey(defaultValue, key);
        }

        /// <inheritdoc />
        public void SetFloat(string key, float value)
        {
            _standardDefaults.SetFloat(value, key);
            _standardDefaults.Synchronize();
        }
    }
}
