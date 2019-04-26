using System;
using CoreGraphics;
using UIKit;
namespace Microsoft.MobCAT.iOS.Views
{
    public class ProgressView : UIView
    {
        UIView loadingContainer;
        UIView backgroundView;
        UILabel loadingLabel;

        UIActivityIndicatorView activityIndicator;

        public ProgressView(CGRect frame, string text = null)
            : base(frame)
        {
            BackgroundColor = UIColor.Clear;

            AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

            backgroundView = new UIView();
            backgroundView.BackgroundColor = UIColor.LightGray;
            backgroundView.Alpha = 0.60f;
            backgroundView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            AddSubview(backgroundView);

            loadingContainer = new UIView();
            loadingContainer.BackgroundColor = UIColor.White;
            loadingContainer.Layer.CornerRadius = 5f;
            loadingContainer.ClipsToBounds = true;
            loadingContainer.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            AddSubview(loadingContainer);

            activityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            activityIndicator.Color = UIColor.Gray;
            loadingContainer.AddSubview(activityIndicator);

            loadingLabel = new UILabel();
            loadingLabel.Text = text ?? "Loading...";
            loadingLabel.TextColor = UIColor.Gray;
            loadingLabel.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            loadingLabel.TextAlignment = UITextAlignment.Center;
            loadingContainer.AddSubview(loadingLabel);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var bounds = Bounds;
            var width = (bounds.Width > bounds.Height ? bounds.Height : bounds.Width) * 0.30;

            backgroundView.Frame = bounds;

            loadingContainer.Frame = new CGRect(
                bounds.GetMidX() - (width / 2),
                bounds.GetMidY() - (width / 2),
                width,
                width
            );

            activityIndicator.Center = new CGPoint(
                loadingContainer.Bounds.GetMidX(),
                loadingContainer.Bounds.GetMidY() - (activityIndicator.Bounds.Height * 0.5f)
            );

            loadingLabel.Frame = new CGRect(
                0,
                loadingContainer.Bounds.Height * 0.5f,
                loadingContainer.Bounds.Width,
                loadingContainer.Bounds.Height * 0.5f
            );

            StartAnimating();
        }

        public void Show(UIView superview)
        {
            InvokeOnMainThread(() =>
            {
                Alpha = 0;

                superview.AddSubview(this);
                superview.BringSubviewToFront(this);

                StartAnimating();

                UIView.AnimateNotify(
                    0.2,
                    0.0,
                    UIViewAnimationOptions.CurveEaseIn,
                    () => Alpha = 1.0f,
                    (finished) => { });
            });
        }

        public void Hide()
        {
            InvokeOnMainThread(() =>
            {
                UIView.AnimateNotify(
                    0.2,
                    0.0,
                    UIViewAnimationOptions.CurveEaseOut,
                    () => Alpha = 0,
                    (complete) => { RemoveFromSuperview(); Alpha = 1; });
            });
        }

        public void StartAnimating()
        {
            activityIndicator?.StartAnimating();
        }

        public void StopAnimating()
        {
            activityIndicator?.StopAnimating();
        }
    }
}
