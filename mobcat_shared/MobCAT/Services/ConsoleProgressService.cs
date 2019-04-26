using System;
using MobCAT;

namespace Microsoft.MobCAT.Services
{
    public class ConsoleProgressService : IProgressService
    {
        /// <inheritdoc />
        public void DisplayProgress(string title = null)
        {
            Logger.Debug($"[Progress] Displaying progress - with title {title}");
        }

        /// <inheritdoc />
        public void HideProgress()
        {
            Logger.Debug($"[Progress] Hiding progress");
        }
    }
}
