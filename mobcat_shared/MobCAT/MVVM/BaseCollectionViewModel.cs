using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Microsoft.MobCAT.MVVM
{
    public abstract class BaseCollectionViewModel<T> : BaseViewModel where T : BaseNotifyPropertyChanged
    {
        ObservableCollection<T> _items;

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>Items.</value>
        public ObservableCollection<T> Items
        {
            get => _items;
            set => RaiseAndUpdate(ref _items, value);
        }

        T selectedItem;

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>Selected Item.</value>
        public T SelectedItem
        {
            get => selectedItem;
            set
            {
                RaiseAndUpdate(ref selectedItem, value);
                ItemSelected?.Invoke();
            }
        }

        /// <summary>
        /// Gets or sets the action that is called when an item is selected.
        /// </summary>
        /// <value>The item selected action.</value>
        public Action ItemSelected { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when a reload is requested.
        /// </summary>
        /// <value>Reload action.</value>
        public Action ReloadAction { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when an action fails.
        /// </summary>
        /// <value>Failed action.</value>
        public Action FailedAction { get; set; }

        /// <summary>
        /// Loads the items.
        /// </summary>
        /// <returns>Task.</returns>
        public abstract Task LoadItems();
    }
}