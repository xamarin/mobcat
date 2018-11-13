using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.Services.Abstractions;
using Xamarin.Forms;

namespace Microsoft.MobCAT.Forms.Services
{
    public class PropertyStoreService : IPropertyStoreService
    {
        /// <inheritdoc />
        public void DeleteValue(string key)
        {
            if ((bool)Application.Current?.Properties.ContainsKey(key))
            {
                Application.Current.Properties.Remove(key);
                SaveAsync();
            }
        }

        /// <inheritdoc />
        public T GetValue<T>(string key)
        {
            if (!(bool)Application.Current?.Properties.ContainsKey(key))
                throw new InvalidOperationException($"The specified {nameof(key)} does not exist.");

            return (T)Application.Current?.Properties[key];
        }

        /// <inheritdoc />
        public T GetValueOrSetDefault<T>(string key, T defaultValue = default(T))
        {
            return ((bool)Application.Current?.Properties.ContainsKey(key)) ? 
                (T)Application.Current?.Properties[key] : this.NewValue<T>(key, defaultValue);
        }

        /// <inheritdoc />
        public Task SaveAsync()
        {
            return Application.Current.SavePropertiesAsync();
        }

        /// <inheritdoc />
        public void SetValue<T>(string key, T value)
        {
            this.NewValue<T>(key, value);
        }

        /// <inheritdoc />
        private T NewValue<T>(string key, T value)
        {
            Application.Current.Properties[key] = value;
            SaveAsync();
            return value;
        }
    }
}