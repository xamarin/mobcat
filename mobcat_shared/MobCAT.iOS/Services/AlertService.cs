using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.Services;
using Microsoft.MobCAT.iOS.Views;
using UIKit;

namespace Microsoft.MobCAT.iOS.Services
{
    public class AlertService : IAlertService
    {
        public Task<string> DisplayInputEntryAsync(
            string title,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            string hint = null,
            Func<string, bool> validator = null)
        {
            var tcs = new TaskCompletionSource<string>();

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alertController = UIAlertController.Create(title, message ?? string.Empty, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(cancelButton ?? "Cancel", UIAlertActionStyle.Cancel, (obj) => tcs.TrySetResult(null)));
                alertController.AddAction(UIAlertAction.Create(actionButton ?? "Ok", UIAlertActionStyle.Default, (obj) =>
                {
                    string value = null;
                    var alertTextField = alertController.TextFields[0];
                    value = alertTextField.Text;

                    if (string.IsNullOrEmpty(value))
                        value = hint;
                    else
                        value = value.Trim();

                    tcs.TrySetResult(value);
                }));
                alertController.AddTextField((UITextField textField) =>
                {
                    if (hint != null) textField.Placeholder = hint;

                    textField.AutocorrectionType = UITextAutocorrectionType.No;
                    textField.AutocapitalizationType = UITextAutocapitalizationType.None;

                    if (validator != null)
                    {
                        var acceptButton = alertController.Actions[1];
                        acceptButton.Enabled = false;

                        textField.AddTarget((sender, e) =>
                        {
                            string value = textField.Text;

                            if (string.IsNullOrEmpty(value))
                                value = textField.Placeholder;
                            else
                                value = value.Trim();

                            bool valid = validator(value);

                            textField.TextColor = valid ? UIColor.Black : UIColor.Red;

                            acceptButton.Enabled = valid;

                        }, UIControlEvent.EditingChanged);
                    }
                });

                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                vc.ShowDetailViewController(alertController, vc);
            });

            return tcs.Task;
        }

        /// <inheritdoc />
        public Task DisplayAsync(
            string title,
            string message = null,
            string cancelButton = null)
        {
            var tcs = new TaskCompletionSource<object>();

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alertController = UIAlertController.Create(title, message ?? string.Empty, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(cancelButton ?? "Cancel", UIAlertActionStyle.Cancel, (obj) => tcs.TrySetResult(null)));

                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                vc.ShowDetailViewController(alertController, vc);
            });

            return tcs.Task;
        }

        /// <inheritdoc />
        public Task<bool> DisplayConfirmationAsync(
            string title,
            string message = null,
            string cancelButton = null,
            string actionButton = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alertController = UIAlertController.Create(title, message ?? string.Empty, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(cancelButton ?? "Cancel", UIAlertActionStyle.Cancel, (obj) => tcs.TrySetResult(false)));
                alertController.AddAction(UIAlertAction.Create(actionButton ?? "Ok", UIAlertActionStyle.Default, (obj) => tcs.TrySetResult(true)));

                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                vc.ShowDetailViewController(alertController, vc);
            });

            return tcs.Task;
        }

        /// <inheritdoc />
        public Task<string> DisplayActionSheetAsync(
            string title,
            string message = null,
            string cancelButton = null,
            string destructionButton = null,
            string[] actions = null)
        {
            var tcs = new TaskCompletionSource<string>();

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);

                if (actions != null)
                {
                    foreach (var action in actions)
                    {
                        alertController.AddAction(UIAlertAction.Create(action, UIAlertActionStyle.Default, (obj) => tcs.TrySetResult(action)));
                    }
                }

                if (!string.IsNullOrWhiteSpace(cancelButton))
                    alertController.AddAction(UIAlertAction.Create(cancelButton ?? "Cancel", UIAlertActionStyle.Cancel, (obj) => tcs.TrySetResult(null)));
                if (!string.IsNullOrEmpty(destructionButton))
                    alertController.AddAction(UIAlertAction.Create(destructionButton ?? "Destroy", UIAlertActionStyle.Destructive, (obj) => tcs.TrySetResult(null)));

                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                vc.ShowDetailViewController(alertController, vc);
            });

            return tcs.Task;
        }

        /// <inheritdoc />
        public Task<DateTime?> DisplayDatePickerAsync(
            string title = null,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            DateTime? initialDate = null,
            DateTime? minDate = null,
            DateTime? maxDate = null)
        {
            var tcs = new TaskCompletionSource<DateTime?>();

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                var datePickerView = new DatePickerView(vc.View.Bounds, title, message, actionButton, cancelButton, initialDate, minDate, maxDate);

                datePickerView.DateSelected += (obj) =>
                {
                    tcs.TrySetResult(obj);
                    datePickerView.Hide();
                };

                datePickerView.CancelRequested += () =>
                {
                    tcs.TrySetResult(null);
                    datePickerView.Hide();
                };

                datePickerView.Show(vc.View);
            });

            return tcs.Task;
        }
    }
}
