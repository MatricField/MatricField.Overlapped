using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Th = System.Threading;
namespace MatricField.ManagedOverlapped.ExtendedOverlapped
{
    /// <summary>
    /// Managed version of <see cref="ManagedInstance"/>
    /// </summary>
    public unsafe class ManagedNativeOverlapped :
        IDisposable
    {
        #region Members
        private NativeOverlapped* pOverlapped;
        #endregion

        /// <summary>
        /// Implicit convertion to the <see cref="NativeOverlapped"/> pointer
        /// owned by <paramref name="this"/> <see cref="ManagedNativeOverlapped"/>.
        /// </summary>
        public static unsafe implicit operator NativeOverlapped*(ManagedNativeOverlapped @this)
        {
            return @this.ManagedInstance;
        }

        #region IDisposable
        private bool _DisposedValue;

        /// <summary>
        /// Implementing dispose pattern
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_DisposedValue)
            {
                if(pOverlapped != null)
                {
                    Overlapped.Free(pOverlapped);
                }
                _DisposedValue = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        ~ManagedNativeOverlapped()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Protected Interface
        /// <summary>
        /// Initialize new instance
        /// </summary>
        /// <param name="pOverlapped">The instance of <see cref="NativeOverlapped"/> to be managed</param>
        internal protected ManagedNativeOverlapped(NativeOverlapped* pOverlapped)
        {
            this.pOverlapped = pOverlapped;
        }
        /// <summary>
        /// Get a pointer to the managed native overlapped instance
        /// </summary>
        protected NativeOverlapped* ManagedInstance => pOverlapped;
        #endregion
    }
}
