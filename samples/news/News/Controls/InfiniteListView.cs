using System;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace News.Controls
{
    public class InfiniteListView : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommandProperty), typeof(ICommand), typeof(InfiniteListView), null);
        public static readonly BindableProperty IsLoadingMoreProperty = BindableProperty.Create(nameof(IsLoadingMoreProperty), typeof(bool), typeof(InfiniteListView), false);


        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public bool IsLoadingMore
        {
            get { return (bool)GetValue(IsLoadingMoreProperty); }
            set { SetValue(IsLoadingMoreProperty, value); }
        }

        public InfiniteListView()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

        private void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (IsLoadingMore)
                return;

            var items = ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(null);
            }
        }
    }
}
