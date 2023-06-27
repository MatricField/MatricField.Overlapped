using MatricField.ManagedOverlapped.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MatricField.ManagedOverlapped
{
    /// <summary>
    /// Dummy extension to pass <see cref="TaskCompletionSource"/> into <see cref="NativeOverlapped"/>
    /// </summary>
    public sealed class TaskCompletionSourceAsyncResult :
        TaskCompletionSource, IAsyncResult
    {
        /// <inheritdoc/>
        public TaskCompletionSourceAsyncResult()
        {
        }
        /// <inheritdoc/>
        public TaskCompletionSourceAsyncResult(object? state) : base(state)
        {
        }
        /// <inheritdoc/>
        public TaskCompletionSourceAsyncResult(TaskCreationOptions creationOptions) : base(creationOptions)
        {
        }
        /// <inheritdoc/>
        public TaskCompletionSourceAsyncResult(object? state, TaskCreationOptions creationOptions) : base(state, creationOptions)
        {
        }
        #region IAsyncResult
        private IAsyncResult TaskAsyncResult => Task;

        object? IAsyncResult.AsyncState => TaskAsyncResult.AsyncState;

        WaitHandle IAsyncResult.AsyncWaitHandle => TaskAsyncResult.AsyncWaitHandle;

        bool IAsyncResult.CompletedSynchronously => TaskAsyncResult.CompletedSynchronously;

        bool IAsyncResult.IsCompleted => TaskAsyncResult.IsCompleted;
        #endregion
    }
}
