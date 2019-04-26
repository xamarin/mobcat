using System;
using System.Threading.Tasks;
using Foundation;
using Microsoft.MobCAT.Services;
using UIKit;

namespace Microsoft.MobCAT.iOS.Services
{
    public class ThreadService : IThreadService
    {
        /// <inheritdoc />
        public bool IsMain => NSThread.Current.IsMainThread;

        /// <inheritdoc />
        public void RunOnMainThread(Action action)
        {
            UIApplication.SharedApplication.BeginInvokeOnMainThread(action);
        }

        /// <inheritdoc />
        public Task RunOnMainThreadAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
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
