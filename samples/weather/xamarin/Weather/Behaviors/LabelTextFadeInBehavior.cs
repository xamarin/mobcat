using System.ComponentModel;
using Microsoft.MobCAT.Forms.Behaviors;
using Xamarin.Forms;

namespace Weather.Behaviors
{
    public class LabelTextFadeInBehavior : BehaviorBase<Label>
    {
        protected override void OnAttachedTo(Label bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.PropertyChanged += OnLabelPropertyChanged;
        }

        protected override void OnDetachingFrom(Label bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.PropertyChanged -= OnLabelPropertyChanged;
        }

        async void OnLabelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Label label)
            {
                if (e.PropertyName == nameof(Label.Text) && !string.IsNullOrEmpty(label.Text))
                {
                    await label.FadeTo(opacity: 1.0d, length: 1000, easing: Easing.CubicIn);
                }
            }
        }
    }
}
