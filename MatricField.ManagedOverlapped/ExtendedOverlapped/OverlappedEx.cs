/* MIT License
 * 
 * Copyright (c) 2023 Mingxi "MatricField" Du
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using MatricField.ManagedOverlapped.Native;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MatricField.ManagedOverlapped.ExtendedOverlapped
{
    /// <summary>
    /// Provide extension methods that wrap <see cref="NativeOverlapped"/> instances
    /// into managed object
    /// </summary>
    public static class OverlappedEx
    {
        /// <summary>
        /// Packs the current instance into a <see cref="NativeOverlapped"/> structure,
        /// specifying a delegate that is invoked
        /// when the asynchronous I/O operation is complete and a managed object that serves as a buffer.
        /// </summary>
        /// <param name="this">An <see cref="Overlapped"/> object to be packed.</param>
        /// <param name="iocb">
        /// An <see cref="IOCompletionCallback"/> delegate
        /// that represents the callback method invoked when the asynchronous I/O operation completes.
        /// </param>
        /// <param name="userData">
        /// An object or array of objects representing the input or output buffer for the operation.
        /// Each object represents a buffer, for example an array of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="ManagedNativeOverlapped"/> object
        /// containing a pointer to a <see cref="NativeOverlapped"/> structure.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The provided <see cref="Overlapped"/> <paramref name="this"/> has already been packed.
        /// </exception>
        /// <remarks>
        /// The <see cref="ManagedNativeOverlapped"/> returned by this method can be
        /// implicitly converted to a pointer to the <see cref="NativeOverlapped"/> structure it contains,
        /// and the pointer can be passed to the operating system in overlapped I/O operations.
        /// The contained <see cref="NativeOverlapped"/> structure is fixed in physical memory until the containing 
        /// <see cref="ManagedNativeOverlapped"/> is disposed.
        /// </remarks>
        public static unsafe ManagedNativeOverlapped PackEx(this Overlapped @this, IOCompletionCallback iocb, object? userData = null)
        {
            NativeOverlapped* nativeOverlapped = default;
            try
            {
                nativeOverlapped = @this.Pack(iocb, userData);
                var ret = new ManagedNativeOverlapped(nativeOverlapped);
                nativeOverlapped = null;
                return ret;
            }
            finally
            {
                if(nativeOverlapped != null)
                {
                    Overlapped.Free(nativeOverlapped);
                }
            }
        }
    }
}
