using System;
using System.Collections;
using System.Windows.Input;
using Lottie.Forms;
using Microsoft.MobCAT.MVVM;
using Xamarin.Forms;

namespace News.Controls
{
    public class InfiniteListView : ListView
    {
        #region dependency properties

        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommandProperty), typeof(ICommand), typeof(InfiniteListView), null);
        public static readonly BindableProperty IsLoadingMoreProperty = BindableProperty.Create(nameof(IsLoadingMoreProperty), typeof(bool), typeof(InfiniteListView), false, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(HandleIsLoadingMoreChanged));

        private static void HandleIsLoadingMoreChanged(BindableObject bindable, object oldValue, object newValue)
        {
            System.Diagnostics.Debug.WriteLine($"LoadingMore is changing from {oldValue} to {newValue}");
            ((InfiniteListView)bindable).UpdateLoadingMoreAnimationState((bool)oldValue != (bool)newValue);
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

        #endregion

        private AnimationView _animationView;

        public InfiniteListView()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;

            Init();
            UpdateLoadingMoreAnimationState(true);
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
    }
}