using System;
using System.Threading.Tasks;

namespace Microsoft.MobCAT.Services
{
    public interface IThreadService
    {
        /// <summary>
        /// Gets a value indicating whether the current thread context is the Main Thread.
        /// </summary>
        /// <value><c>true</c> if it is Main thread; otherwise, <c>false</c>.</value>
        bool IsMain { get; }

        /// <summary>
        /// Queues the action to be executed on the Main Thread.
        /// </summary>
        /// <param name="action">Action to run on Main Thead.</param>
        void RunOnMainThread(Action action);

        /// <summary>
        /// Queues the action to be executed on the Main Thread and awaits it's execution.
        /// </summary>
        /// <returns>Task that is completed when the action has completed.</returns>
        /// <param name="action">Action to run on Main Thread.</param>
        Task RunOnMainThreadAsync(Action action);
    }
}
