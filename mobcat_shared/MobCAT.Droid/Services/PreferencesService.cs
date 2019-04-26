using System;
using Android.Content;
using Microsoft.MobCAT.Abstractions;

namespace Microsoft.MobCAT.Droid.Services
{
    public class PreferencesService : IPreferencesService
    {
        private readonly ISharedPreferences _preferences;

        public PreferencesService(ISharedPreferences preferences)
        {
            _preferences = preferences;
        }

        /// <inheritdoc />
        public string GetString(string key, string defaultValue)
        {
            return _preferences.GetString(key, defaultValue);
        }

        /// <inheritdoc />
        public void SetString(string key, string value)
        {
            ISharedPreferencesEditor editor = _preferences.Edit();
            if (value == null)
                editor.Remove(key);
            else
                editor.PutString(key, value);
            editor.Commit();
        }

        /// <inheritdoc />
        public bool GetBool(string key, bool defaultValue)
        {
            return _preferences.GetBoolean(key, defaultValue);
        }

        /// <inheritdoc />
        public void SetBool(string key, bool value)
        {
            ISharedPreferencesEditor editor = _preferences.Edit();
            editor.PutBoolean(key, value);
            editor.Commit();
        }

        /// <inheritdoc />
        public int GetInt(string key, int defaultValue)
        {
            return _preferences.GetInt(key, defaultValue);
        }

        /// <inheritdoc />
        public void SetInt(string key, int value)
        {
            ISharedPreferencesEditor editor = _preferences.Edit();
            editor.PutInt(key, value);
            editor.Commit();
        }

        /// <inheritdoc />
        public double GetDouble(string key, double defaultValue)
        {
            return _preferences.GetFloat(key, (float)defaultValue);
        }

        /// <inheritdoc />
        public void SetDouble(string key, double value)
        {
            ISharedPreferencesEditor editor = _preferences.Edit();
            editor.PutFloat(key, (float)value);
            editor.Commit();
        }

        /// <inheritdoc />
        public float GetFloat(string key, float defaultValue)
        {
            return _preferences.GetFloat(key, defaultValue);
        }

        /// <inheritdoc />
        public void SetFloat(string key, float value)
        {
            ISharedPreferencesEditor editor = _preferences.Edit();
            editor.PutFloat(key, value);
            editor.Commit();
        }
    }
}
