using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobCAT.Forms.Controls
{
    public class InfiniteListView : ListView
    {
        #region dependency properties

        public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(nameof(ItemSelectedCommandProperty), typeof(ICommand), typeof(InfiniteListView), null);
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommandProperty), typeof(ICommand), typeof(InfiniteListView), null);
        public static readonly BindableProperty IsLoadingMoreProperty = BindableProperty.Create(nameof(IsLoadingMoreProperty), typeof(bool), typeof(InfiniteListView), false, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(HandleIsLoadingMoreChanged));
        public static readonly BindableProperty IsEmptyProperty = BindableProperty.Create(nameof(IsEmptyProperty), typeof(bool), typeof(InfiniteListView), false, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(HandleIsEmptyChanged));
        public static readonly BindableProperty EmptyTemplateProperty = BindableProperty.Create(nameof(EmptyTemplateProperty), typeof(DataTemplate), typeof(InfiniteListView));

        private static void HandleIsLoadingMoreChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stateChanged = (bool)oldValue != (bool)newValue;
            if (stateChanged)
            {
                ((InfiniteListView)bindable).OnIsLoadingMoreChanged();
            }
        }

        private static void HandleIsEmptyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stateChanged = (bool)oldValue != (bool)newValue;
            if (stateChanged)
            {
                ((InfiniteListView)bindable).OnIsEmptyChanged();
            }
        }

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

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

        public bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
            set { SetValue(IsEmptyProperty, value); }
        }

        public DataTemplate EmptyTemplate
        {
            get { return (DataTemplate)GetValue(EmptyTemplateProperty); }
            set { SetValue(EmptyTemplateProperty, value); }
        }

        #endregion

        private View _emptyTemplateView;

        public InfiniteListView()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
            ItemSelected += InfiniteListView_ItemSelected;
            Init();
        }

        private void Init()
        {
            OnIsLoadingMoreChanged();
            OnIsEmptyChanged();
        }

        private void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (IsLoadingMore)
                return;

            if (ItemsSource is IList items && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                {
                    LoadMoreCommand.Execute(null);
                }
            }
        }

        private void InfiniteListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                // Ignore item deselected events
                return;
            }

            if (ItemSelectedCommand != null && ItemSelectedCommand.CanExecute(null))
            {
                ItemSelectedCommand.Execute(e.SelectedItem);
            }
        }

        protected virtual void OnIsLoadingMoreChanged()
        {
            // Do nothing
        }

        protected virtual void OnIsEmptyChanged()
        {
            if (EmptyTemplate == null)
            {
                return;
            }

            if (IsEmpty)
            {
                // We use the ListView footer as a container for the empty template content
                if (_emptyTemplateView == null)
                {
                    var templateContent = EmptyTemplate.CreateContent();
                    if (templateContent is View templateContentView)
                    {
                        var container = new Grid
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Padding = new Thickness(50),
                            HeightRequest = 200,
                        };

                        container.Children.Add(templateContentView);
                        _emptyTemplateView = container;
                    }
                }

                if (_emptyTemplateView != null)
                {
                    Header = _emptyTemplateView;
                }
            }
            else
            {
                Header = null;
            }
        }
    }
}