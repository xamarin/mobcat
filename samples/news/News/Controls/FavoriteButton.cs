using System;
using System.Threading.Tasks;
using Lottie.Forms;
using News.Helpers;
using Xamarin.Forms;

namespace News.Controls
{
    public class FavoriteButton : ImageButton
    {
        #region dependency properties

        public static readonly BindableProperty IsFavoriteProperty = BindableProperty.Create(nameof(IsFavorite), typeof(bool), typeof(FavoriteButton), false, propertyChanged: OnIsFavoriteChanged);

        public bool IsFavorite
        {
            get { return (bool)GetValue(IsFavoriteProperty); }
            set { SetValue(IsFavoriteProperty, value); }
        }

        private static void OnIsFavoriteChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var button = (FavoriteButton)bindable;
            var stateChanged = (bool)oldValue != (bool)newValue;
            if (stateChanged)
            {
                button.UpdateControlStateWithAnimation();
            }
        }

        #endregion

        private AnimationView _animationView;

        public FavoriteButton()
        {
            Init();
            UpdateControlState();
        }

        private void Init()
        {
            if (_animationView == null)
            {
                _animationView = new AnimationView()
                {
                    Animation = "lottie_favorite_add.json",
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.Transparent,
                    Loop = false,
                    AutoPlay = false,
                    IsVisible = false,
                    PlaybackFinishedCommand = new Command(UpdateControlState),
                };
            }
        }

        private void UpdateControlState()
        {
            _animationView.IsPlaying = false;
            _animationView.IsVisible = false;

            System.Diagnostics.Debug.WriteLine($"UpdateControlState: IsFavorite = {IsFavorite}");
            var favoriteSource = IsFavorite ? "ic_favorite_fill.png" : "ic_favorite_empty.png";
            Source = ImageSource.FromFile(favoriteSource);
        }

        private void UpdateControlStateWithAnimation()
        {
            if (IsFavorite)
            {
                System.Diagnostics.Debug.WriteLine($"Add to favorites state");

                if(Parent is Layout<View> animationContainer)
                {
                    //await Task.Delay(1000);
                    if (!animationContainer.Children.Contains(_animationView))
                    {
                        animationContainer.Children.Add(_animationView);
                    }

                    Source = null;
                    _animationView.AbortAnimation(GetHashCode().ToString());
                    _animationView.IsVisible = true;
                    _animationView.Play();
                    return;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Favorite button doesn't have parent container to render animation.");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Remove from favorites state");
            }

            UpdateControlState();
        }
    }
}
