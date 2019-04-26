using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
namespace Microsoft.MobCAT.Droid.Views
{
    public class InputDialogFragment : DialogFragment, ITextWatcher, IDialogInterfaceOnShowListener, View.IOnClickListener
    {
        private WeakReference<Context> _contextWeak;

        private readonly string _title, _message, _actionButton, _cancelButton, _hint;
        private readonly Func<string, bool> _validator;

        private IDialogInterface _dialog;
        private EditText _textInput;

        public TaskCompletionSource<string> Tcs { get; }

        public InputDialogFragment(
            Context context,
            string title,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            string hint = null,
            Func<string, bool> validator = null)
        {
            _contextWeak = new WeakReference<Context>(context);

            _title = title;
            _message = message;
            _actionButton = actionButton;
            _cancelButton = cancelButton;
            _hint = hint;
            _validator = validator;

            Tcs = new TaskCompletionSource<string>();
        }

        public override Dialog OnCreateDialog(Android.OS.Bundle savedInstanceState)
        {
            _contextWeak.TryGetTarget(out Context context);

            var alertBuilder = new Android.Support.V7.App.AlertDialog.Builder(context);
            alertBuilder.SetTitle(_title);

            if (_message != null)
                alertBuilder.SetMessage(_message);

            _textInput = new EditText(context);
            _textInput.AddTextChangedListener(this);
            alertBuilder.SetView(_textInput);

            if (_hint != null)
            {
                _textInput.Text = _hint;
                _textInput.SelectAll();
            }

            alertBuilder.SetCancelable(false);

            alertBuilder.SetNegativeButton(_cancelButton ?? "Cancel", delegate
            {
                Tcs.TrySetResult(null);
            });

            alertBuilder.SetPositiveButton(_actionButton ?? "Ok", (EventHandler<DialogClickEventArgs>)null);

            var alert = alertBuilder.Create();

            alert.SetOnShowListener(this);

            return alert;
        }

        public void AfterTextChanged(IEditable s)
        {
            if (_validator == null) return;

            if (_validator(s.ToString()))
                _textInput.SetTextColor(Android.Graphics.Color.Black);
            else
                _textInput.SetTextColor(Android.Graphics.Color.Red);
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
        }

        public void OnShow(IDialogInterface dialog)
        {
            _dialog = dialog;
            var button = ((Android.Support.V7.App.AlertDialog)dialog).GetButton((int)DialogButtonType.Positive);
            button.SetOnClickListener(this);
        }

        public void OnClick(View v)
        {
            string value = _textInput?.Text;
            if (!string.IsNullOrEmpty(value))
                value = value.Trim();

            if (_validator != null)
            {
                if (_validator(value))
                {
                    Tcs.TrySetResult(value);
                    _dialog.Dismiss();
                    return;
                }

                System.Diagnostics.Debug.WriteLine("Not Valid");
                return;
            }

            Tcs.TrySetResult(value);
            _dialog.Dismiss();

            return;
        }
    }
}
