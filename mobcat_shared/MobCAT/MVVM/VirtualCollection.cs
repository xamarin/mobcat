using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobCAT.MVVM
{
    public class VirtualCollection<TItem> : ObservableCollection<TItem>
    {
        public int VirtualPage => Count > 0 ? (((Count - 1) / VirtualPageSize) + 1) : 0;

        public bool FullyLoaded => Count >= VirtualCount;

        private int _virtualPageSize = 25;
        public int VirtualPageSize
        {
            get { return _virtualPageSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException($"Unable to set {nameof(VirtualPageSize)} smaller than 0");
                }

                if (RaiseAndUpdate(ref _virtualPageSize, value))
                {
                    Raise(nameof(VirtualPage));
                    Raise(nameof(FullyLoaded));
                }
            }
        }

        private int _virtualCount;
        public int VirtualCount
        {
            get { return _virtualCount; }
            set
            {
                // We cannot set new Virtual count lower than number of already loaded items
                var newValue = Math.Max(value, Count);
                if (RaiseAndUpdate(ref _virtualCount, newValue))
                {
                    Raise(nameof(VirtualPage));
                    Raise(nameof(FullyLoaded));
                }
            }
        }

        public VirtualCollection()
        {

        }

        public VirtualCollection(IEnumerable<TItem> collection)
            : base(collection)
        {
        }

        public void AddPage(IEnumerable<TItem> collection, int? virtualCount = null, int? pageNumber = null, int? pageSize = null)
        {
            foreach (var article in collection)
            {
                this.Add(article);
            }

            if (virtualCount != null)
            {
                VirtualCount = virtualCount.Value;
            }

            if (pageSize != null)
            {
                VirtualPageSize = pageSize.Value;
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (Count > VirtualCount)
            {
                VirtualCount = Count;
            }
            else
            {
                Raise(nameof(VirtualPage));
                Raise(nameof(FullyLoaded));
            }

            // System.Diagnostics.Debug.WriteLine($"Count = {Count} | VirtualCount = {VirtualCount} | VirtualPageSize {VirtualPageSize} | VirtualPage = {VirtualPage}");
        }

        protected bool RaiseAndUpdate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            Raise(propertyName);

            return true;
        }

        protected void Raise(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}