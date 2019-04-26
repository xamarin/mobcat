using System;
using System;
using CoreGraphics;
using Foundation;
using Microsoft.MobCAT.iOS.Extensions;
using UIKit;

namespace Microsoft.MobCAT.iOS.Views
{
    public class DatePickerView : UIView
    {
        UIView _loadingContainer;
        UIView _backgroundView;
        UILabel _titleLabel;
        UILabel _messageLabel;

        UIButton _cancelButton;
        UIButton _actionButton;

        UIDatePicker _datePicker;

        public event Action<DateTime> DateSelected;
        public event Action CancelRequested;

        public DatePickerView(
            CGRect frame,
            string title = null,
            string message = null,
            string actionText = null,
            string cancelText = null,
            DateTime? initialDate = null,
            DateTime? minDate = null,
            DateTime? maxDate = null)
            : base(frame)
        {
            BackgroundColor = UIColor.Clear;

            AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

            _backgroundView = new UIView();
            _backgroundView.BackgroundColor = UIColor.LightGray;
            _backgroundView.Alpha = 0.60f;
            _backgroundView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            AddSubview(_backgroundView);

            _loadingContainer = new UIView();
            _loadingContainer.BackgroundColor = UIColor.White;
            _loadingContainer.Layer.CornerRadius = 5f;
            _loadingContainer.ClipsToBounds = true;
            AddSubview(_loadingContainer);

            _datePicker = new UIDatePicker();

            if (initialDate != null) _datePicker.Date = initialDate?.ToNSDate();
            if (minDate != null) _datePicker.MinimumDate = minDate?.ToNSDate();
            if (maxDate != null) _datePicker.MaximumDate = maxDate?.ToNSDate();

            _loadingContainer.AddSubview(_datePicker);

            if (title != null)
            {
                _titleLabel = new UILabel();
                _titleLabel.Text = title;
                _titleLabel.TextColor = UIColor.Gray;
                _titleLabel.TextAlignment = UITextAlignment.Center;
                _loadingContainer.AddSubview(_titleLabel);
            }

            if (message != null)
            {
                _messageLabel = new UILabel();
                _messageLabel.Text = message;
                _messageLabel.TextColor = UIColor.Gray;
                _messageLabel.TextAlignment = UITextAlignment.Center;
                _messageLabel.Font = UIFont.SystemFontOfSize(UIFont.SmallSystemFontSize);
                _messageLabel.AdjustsFontSizeToFitWidth = true;
                _loadingContainer.AddSubview(_messageLabel);
            }

            _cancelButton = new UIButton();
            _cancelButton.SetTitle(cancelText ?? "Cancel", UIControlState.Normal);
            _cancelButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            _loadingContainer.AddSubview(_cancelButton);

            _actionButton = new UIButton();
            _actionButton.SetTitle(actionText ?? "Ok", UIControlState.Normal);
            _actionButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            _loadingContainer.AddSubview(_actionButton);

            _cancelButton.TouchUpInside += CancelButton_TouchUpInside;
            _actionButton.TouchUpInside += ActionButton_TouchUpInside;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var bounds = Bounds;
            var buttonHeight = 40;
            var width = (bounds.Width > bounds.Height ? bounds.Height : bounds.Width) * 0.90;

            var itemCount = 1;

            _backgroundView.Frame = bounds;

            _loadingContainer.Frame = new CGRect(
                bounds.GetMidX() - (width / 2),
                bounds.GetMidY() - (width / 2),
                width,
                width
            );

            if (_titleLabel != null)
            {
                _titleLabel.Frame = new CGRect(
                    0,
                    0,
                    width,
                    buttonHeight
                );

                itemCount++;
            }

            if (_messageLabel != null)
            {
                _messageLabel.Frame = new CGRect(
                    20,
                    (itemCount - 1) * buttonHeight,
                    width - 40,
                    buttonHeight
                );

                itemCount++;
            }

            _datePicker.Frame = new CGRect(
                0,
                (itemCount - 1) * buttonHeight,
                width,
                width - (itemCount * buttonHeight)
            );

            _cancelButton.Frame = new CGRect(
                0,
                _datePicker.Frame.GetMaxY(),
                width * 0.5f,
                buttonHeight
            );

            _actionButton.Frame = new CGRect(
                _cancelButton.Frame.GetMaxX(),
                _datePicker.Frame.GetMaxY(),
                width * 0.5f,
                buttonHeight
            );
        }

        public void Show(UIView superview)
        {
            InvokeOnMainThread(() =>
            {
                Alpha = 0;

                superview.AddSubview(this);
                superview.BringSubviewToFront(this);

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
                    0.20,
                    0.0,
                    UIViewAnimationOptions.CurveEaseOut,
                    () => Alpha = 0,
                    (complete) => RemoveFromSuperview());
            });
        }


        void CancelButton_TouchUpInside(object sender, EventArgs e)
        {
            CancelRequested?.Invoke();
        }

        void ActionButton_TouchUpInside(object sender, EventArgs e)
        {
            DateSelected?.Invoke(_datePicker.Date.ToDateTime());
        }
    }
}
