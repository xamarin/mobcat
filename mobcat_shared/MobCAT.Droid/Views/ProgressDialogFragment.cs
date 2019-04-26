using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Microsoft.MobCAT.Droid.Views
{
    public class ProgressDialogFragment : DialogFragment
    {
        private WeakReference<Context> _contextWeak;
        private string _title;

        public ProgressDialogFragment(Context context, string title = null)
        {
            _contextWeak = new WeakReference<Context>(context);
            _title = title;
        }

        public override Dialog OnCreateDialog(Android.OS.Bundle savedInstanceState)
        {
            _contextWeak.TryGetTarget(out Context context);

            var dialog = new Dialog(context);
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);

            dialog.SetContentView(new ProgressView(context, _title));

            if (_title != null)
                dialog.SetTitle(_title);

            return dialog;
        }

        class ProgressView : LinearLayout
        {
            private int _paddingDp = 20;

            public ProgressView(Context context, string title)
                : base(context)
            {
                Orientation = Orientation.Vertical;
                LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

                var padding = (int)(_paddingDp * Resources.DisplayMetrics.Density);

                var progressBar = new ProgressBar(context);
                progressBar.SetPadding(padding, padding, padding, padding);

                AddView(progressBar);

                if (title != null)
                {
                    var titleText = new TextView(context)
                    {
                        Text = title,
                        TextAlignment = TextAlignment.Center,
                        TextSize = _paddingDp,
                    };
                    titleText.SetPadding(padding, 0, padding, padding);
                    titleText.Gravity = GravityFlags.Center;

                    AddView(titleText);
                }
            }
        }
    }
}
