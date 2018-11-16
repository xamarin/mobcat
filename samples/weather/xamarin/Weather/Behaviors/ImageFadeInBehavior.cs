using System;
using System.ComponentModel;
using Microsoft.MobCAT.Forms.Behaviors;
using Xamarin.Forms;

namespace Weather.Behaviors
{
    public class ImageFadeInBehavior : BehaviorBase<Image>
    {
        public static readonly BindableProperty DurationProperty =
            BindableProperty.Create(nameof(Duration),
                                    typeof(uint),
                                    typeof(ImageFadeInBehavior),
                                    default(uint));

        public uint Duration
        {
            get { return (uint)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        protected override void OnAttachedTo(Image bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.PropertyChanged += OnLabelPropertyChanged;
        }

        protected override void OnDetachingFrom(Image bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.PropertyChanged -= OnLabelPropertyChanged;
        }

        async void OnLabelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Image image)
            {
                if (e.PropertyName == nameof(Image.Source) && image.Source != default(ImageSource))
                {
                    await image.FadeTo(opacity: 1.0d, length: Duration, easing: Easing.CubicIn);
                }
            }
        }
    }
}
