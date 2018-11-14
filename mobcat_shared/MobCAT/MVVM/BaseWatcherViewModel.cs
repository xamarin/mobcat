using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.MobCAT.MVVM
{
    public abstract class BaseWatcherViewModel
    {
        readonly List<KeyValuePair<string, List<Action>>> PropertyWatchers = new List<KeyValuePair<string, List<Action>>>();
        bool _isBusy = false;

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets whether the ViewModel is busy.
        /// </summary>
        /// <value><c>true</c> if is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy
        {
            get 
            { 
                return _isBusy; 
            }
            set
            {
                RaiseAndUpdate(ref _isBusy, value);
                Raise(nameof(IsNotBusy));
            }
        }

        /// <summary>
        /// Gets whether the ViewModel is not busy.
        /// </summary>
        /// <value><c>true</c> if is busy; otherwise, <c>false</c>.</value>
        public bool IsNotBusy => !IsBusy;

        protected void RaiseAndUpdate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            Raise(propertyName);
        }

        protected void Raise(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            var watchers = PropertyWatchers.FirstOrDefault(pw => pw.Key == propertyName);

            if (watchers.Equals(default(KeyValuePair<string, List<Action>>)))
                return;

            foreach (Action watcher in watchers.Value)
                watcher();
        }

        /// <summary>
        /// Associates an action with a given property which is subsequently invoked when the respective property changes.
        /// </summary>
        /// <param name="propertyName">The name of the property to watch.</param>
        /// <param name="action">The action to perform upon property change notification.</param>
        public void WatchProperty(string propertyName, Action action)
        {
            if (PropertyWatchers.All(pw => pw.Key != propertyName))
                PropertyWatchers.Add(new KeyValuePair<string, List<Action>>(propertyName, new List<Action>()));

            PropertyWatchers.First(pw => pw.Key == propertyName).Value.Add(action);
        }

        /// <summary>
        /// Clears the collection of properties veing watched.
        /// </summary>
        public void ClearWatchers()
        {
            PropertyWatchers.Clear();
        }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        /// <returns>The Task.</returns>
        public virtual Task InitAsync() => Task.FromResult(true);

        public virtual void Dispose() { }
    }
}