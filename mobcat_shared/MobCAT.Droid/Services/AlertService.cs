using System;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Microsoft.MobCAT.Droid.Views;
using Microsoft.MobCAT.Services;
using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Microsoft.MobCAT.Droid.Extensions;

namespace Microsoft.MobCAT.Droid
{
    public class AlertService : IAlertService
    {
        public Task<string> DisplayActionSheetAsync(
            string title,
            string message = null,
            string cancelButton = null,
            string destructionButton = null,
            params string[] actions)
        {
            var tcs = new TaskCompletionSource<string>();

            var context = MainApplication.CurrentActivity;

            var alertBuilder = new AlertDialog.Builder(context);
            alertBuilder.SetTitle(title);

            if (message != null)
                alertBuilder.SetMessage(message);

            alertBuilder.SetCancelable(false);

            if (destructionButton != null)
            {
                alertBuilder.SetNegativeButton(destructionButton, delegate
                {
                    tcs.TrySetResult(null);
                });
            }

            alertBuilder.SetPositiveButton(cancelButton ?? "Cancel", delegate
            {
                tcs.TrySetResult(null);
            });

            var listview = new ListView(context)
            {
                Adapter = new ActionSheetAdapter(actions),
                DividerHeight = 0,
                Divider = null
            };
            alertBuilder.SetView(listview);

            context.RunOnUiThread(() =>
            {
                var alert = alertBuilder.Create();

                listview.ItemClick += (sender, e) =>
                {
                    tcs.TrySetResult(actions[e.Position]);
                    alert.Dismiss();
                };

                alert.Show();
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

            var context = MainApplication.CurrentActivity;

            var alertBuilder = new AlertDialog.Builder(context);
            alertBuilder.SetTitle(title);

            if (message != null)
                alertBuilder.SetMessage(message);

            alertBuilder.SetCancelable(false);

            alertBuilder.SetPositiveButton(cancelButton ?? "Cancel", delegate
            {
                tcs.TrySetResult(null);
            });

            context.RunOnUiThread(() =>
            {
                alertBuilder.Show();
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

            var context = MainApplication.CurrentActivity;

            var alertBuilder = new AlertDialog.Builder(context);
            alertBuilder.SetTitle(title);

            if (message != null)
                alertBuilder.SetMessage(message);

            alertBuilder.SetCancelable(false);

            alertBuilder.SetNegativeButton(cancelButton ?? "Cancel", delegate
            {
                tcs.TrySetResult(false);
            });

            alertBuilder.SetPositiveButton(actionButton ?? "Ok", delegate
            {
                tcs.TrySetResult(true);
            });

            context.RunOnUiThread(() =>
            {
                alertBuilder.Show();
            });

            return tcs.Task;
        }

        /// <inheritdoc />
        public Task<string> DisplayInputEntryAsync(
            string title,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            string hint = null,
            Func<string, bool> validator = null)
        {
            var context = MainApplication.CurrentActivity;

            var textInputDialog = new InputDialogFragment(
                context,
                title,
                message,
                actionButton,
                cancelButton,
                hint,
                validator
            );

            context.RunOnUiThread(() =>
            {
                textInputDialog.Show(MainApplication.CurrentActivity.FragmentManager, "dialog");
            });

            return textInputDialog.Tcs.Task;
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

            MainApplication.CurrentActivity.RunOnUiThread(() =>
            {
                var dateDialog = new Android.App.DatePickerDialog(MainApplication.CurrentActivity);

                dateDialog.SetCancelable(false);
                dateDialog.SetCanceledOnTouchOutside(false);

                if (minDate != null) dateDialog.DatePicker.MinDate = minDate.Value.ToLongDate();
                if (maxDate != null) dateDialog.DatePicker.MaxDate = maxDate.Value.ToLongDate();
                if (initialDate != null) dateDialog.DatePicker.DateTime = initialDate.Value;

                if (title != null)
                    dateDialog.SetTitle(title);

                if (message != null)
                    dateDialog.SetMessage(message);

                dateDialog.SetButton(
                    (int)DialogButtonType.Negative,
                    cancelButton ?? "Cancel",
                    delegate
                    {
                        tcs.TrySetResult(null);
                        dateDialog.Hide();
                    }
                );

                dateDialog.SetButton(
                    (int)DialogButtonType.Positive,
                    actionButton ?? "Ok",
                    delegate
                    {
                        tcs.TrySetResult(dateDialog.DatePicker.DateTime);
                        dateDialog.Hide();
                    }
                );

                dateDialog.Show();
            });

            return tcs.Task;
        }

        class ActionSheetAdapter : BaseAdapter<string>
        {
            string[] actions;
            public ActionSheetAdapter(string[] actions)
            {
                this.actions = actions;
            }

            public override string this[int position] => this.actions[position];

            public override int Count => this.actions.Length;

            public override long GetItemId(int position)
            {
                return position;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var view = convertView ?? MainApplication.CurrentActivity.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
                view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = actions[position];

                return view;
            }
        }

    }
}
