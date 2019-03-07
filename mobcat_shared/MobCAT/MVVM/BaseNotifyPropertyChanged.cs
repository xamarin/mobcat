using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Microsoft.MobCAT.MVVM
{
    public class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        readonly Dictionary<string, List<Action>> _propertyWatchers = new Dictionary<string, List<Action>>();

        /// <summary>
        /// Raises PropertyChanged after updating the backing property with the specified value.
        /// </summary>
        /// <returns><c>true</c>, if and update was raised, <c>false</c> otherwise.</returns>
        /// <param name="field">Field.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool RaiseAndUpdate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            Raise(propertyName);

            return true;
        }

        /// <summary>
        /// Raises PropertyChanged for the a named property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected void Raise(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            if (!_propertyWatchers.ContainsKey(propertyName))
                return;

            var watchers = _propertyWatchers[propertyName];

            foreach (Action watcher in watchers)
                watcher();
        }

        /// <summary>
        /// Watch a property and execute an action when it broadcasts a change notification.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="action">Action.</param>
        public void WatchProperty(string propertyName, Action action)
        {
            if (!_propertyWatchers.ContainsKey(propertyName))
            {
                _propertyWatchers[propertyName] = new List<Action>();
            }

            _propertyWatchers[propertyName].Add(action);
        }

        /// <summary>
        /// Clears all property watchers.
        /// </summary>
        public void ClearWatchers()
        {
            _propertyWatchers.Clear();
        }
    }
}