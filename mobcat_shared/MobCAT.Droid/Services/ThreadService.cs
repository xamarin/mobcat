using System;
using System.Threading.Tasks;
using Android.OS;
using Microsoft.MobCAT.Services;

namespace Microsoft.MobCAT.Droid.Services
{
    public class ThreadService : IThreadService
    {
        /// <inheritdoc />
        public bool IsMain => Looper.MainLooper?.Thread == Looper.MyLooper()?.Thread;

        /// <inheritdoc />
        public void RunOnMainThread(Action action)
        {
            MainApplication.CurrentActivity.RunOnUiThread(action);
        }

        /// <inheritdoc />
        public Task RunOnMainThreadAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            MainApplication.CurrentActivity.RunOnUiThread(() =>
            {
                try
                {
                    action.Invoke();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex);
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }
    }
}
