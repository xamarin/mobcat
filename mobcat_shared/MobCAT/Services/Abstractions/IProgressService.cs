using System;
namespace Microsoft.MobCAT.Services

{
    public interface IProgressService
    {
        /// <summary>
        /// Displays the progress.
        /// </summary>
        /// <param name="title">Title.</param>
        void DisplayProgress(string title = null);

        /// <summary>
        /// Hides the progress.
        /// </summary>
        void HideProgress();
    }
}
