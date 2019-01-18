using System.Collections;
using System.Windows.Input;
using Lottie.Forms;
using Xamarin.Forms;

namespace News.Controls
{
    public class InfiniteListView : ListView
    {
        #region dependency properties

        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommandProperty), typeof(ICommand), typeof(InfiniteListView), null);
        public static readonly BindableProperty IsLoadingMoreProperty = BindableProperty.Create(nameof(IsLoadingMoreProperty), typeof(bool), typeof(InfiniteListView), false, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(HandleIsLoadingMoreChanged));
        public static readonly BindableProperty IsEmptyProperty = BindableProperty.Create(nameof(IsEmptyProperty), typeof(bool), typeof(InfiniteListView), false, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(HandleIsEmptyChanged));
        public static readonly BindableProperty EmptyTemplateProperty = BindableProperty.Create(nameof(EmptyTemplateProperty), typeof(DataTemplate), typeof(InfiniteListView));

        private static void HandleIsLoadingMoreChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((InfiniteListView)bindable).UpdateLoadingMoreAnimationState((bool)oldValue != (bool)newValue);
        }

        private static void HandleIsEmptyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((InfiniteListView)bindable).UpdateIsEmptyState((bool)oldValue != (bool)newValue);
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

        private AnimationView _animationView;
        private View _emptyTemplateView;

        public InfiniteListView()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;

            Init();
            UpdateLoadingMoreAnimationState(true);
            UpdateIsEmptyState(true);
        }

        private void Init()
        {
            if (_animationView == null)
            {
                _animationView = new AnimationView()
                {
                    Animation = "lottie_load_more.json",
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.Transparent,
                    Loop = true,
                    AutoPlay = false,
                    IsVisible = false,
                    HeightRequest = 220,
                    Margin = new Thickness(0),
                };
            }
        }
        private void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (IsLoadingMore)
                return;

            var items = ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                {
                    LoadMoreCommand.Execute(null);
                }
            }
        }

        private void UpdateLoadingMoreAnimationState(bool stateChanged = false)
        {
            if (!stateChanged || _animationView == null)
            {
                return;
            }

            if (IsLoadingMore)
            {
                if (Footer == null)
                {
                    Footer = _animationView;
                }

                _animationView.AbortAnimation(GetHashCode().ToString());
                _animationView.IsVisible = true;
                _animationView.Play();
            }
            else
            {
                _animationView.AbortAnimation(GetHashCode().ToString());
                _animationView.IsPlaying = false;
                _animationView.IsVisible = false;
                Footer = null;
            }
        }

        private void UpdateIsEmptyState(bool stateChanged = false)
        {
            if (!stateChanged || EmptyTemplate == null)
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