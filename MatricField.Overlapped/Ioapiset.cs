using MatricField.ManagedOverlapped.ExtendedOverlapped;
using MatricField.ManagedOverlapped.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MatricField.ManagedOverlapped
{
    /// <summary>
    /// Provides managed wrappers of Ioapiset.h
    /// </summary>
    public static class Ioapiset
    {
        //TODO: Add Doc
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDevice"></param>
        /// <param name="dwIoControlCode"></param>
        /// <param name="lpInBuffer"></param>
        /// <param name="nInBufferSize"></param>
        /// <param name="lpOutBuffer"></param>
        /// <param name="nOutBufferSize"></param>
        /// <param name="token"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static unsafe Task DeviceIoControlAsync(
            this SafeHandle hDevice,
            uint dwIoControlCode,
            byte[]? lpInBuffer,
            int nInBufferSize,
            byte[]? lpOutBuffer,
            int nOutBufferSize,
            CancellationToken token,
            ulong? offset = null)
        {
            ArgumentNullException.ThrowIfNull(hDevice, nameof(hDevice));
            var completionSource = new TaskCompletionSourceAsyncResult();
            Overlapped overlapped = InitializeOverlapped();
            var userData = PackBuffers(lpInBuffer, lpOutBuffer);
            var managedOverlapped = overlapped.PackEx(OverlappedOperationCallback, userData);

            using (token.Register(CancellationCallBack))
            {
                if (!Kernel32.DeviceIoControl(
                    hDevice,
                    dwIoControlCode,
                    lpInBuffer,
                    nInBufferSize,
                    lpOutBuffer,
                    nOutBufferSize,
                    null,
                    managedOverlapped
                    ))
                {
                    var error = Marshal.GetLastWin32Error();
                    if (error != 0 && error != Win32.ErrorIOPending)
                    {
                        var ex = Win32.GetExceptionForWin32(error);
                        if (ex is not null)
                        {
                            throw ex;
                        }
                    }
                }
                return completionSource.Task;
            }

            void CancellationCallBack()
            {
                if (!Kernel32.CancelIoEx(hDevice, managedOverlapped))
                {
                    var win32Ex = new Win32Exception(Marshal.GetLastWin32Error());
                    completionSource.SetException(new OperationCanceledException(null, win32Ex, token));
                }
                else
                {
                    completionSource.SetException(new OperationCanceledException(null, token));
                }
            }
            Overlapped InitializeOverlapped()
            {
                var overlapped = new Overlapped()
                {
                    AsyncResult = completionSource,
                };
                if (offset is not null)
                {
                    overlapped.OffsetLow = (int)(uint)offset;
                    overlapped.OffsetHigh = (int)(uint)(offset >> 32);
                }

                return overlapped;
            }
        }

        private static object? PackBuffers(params object?[] objects)
        {
            var userData = 
                objects
                .Where(obj => obj is not null)
                .ToArray();
            return userData.Length > 0 ? userData : null;
        }

        const string ASYNC_CALLBACK_FAIL_MESSAGE = "unable to complete async callback";

        private static unsafe void OverlappedOperationCallback(
            uint errorCode,
            uint numBytes,
            NativeOverlapped* pOverlapped)
        {
            var nativeOverlapped = Overlapped.Unpack(pOverlapped);
            var completionSource = nativeOverlapped.AsyncResult as TaskCompletionSource<uint>;
            if (0 != errorCode)
            {
                var ex = Win32.GetExceptionForWin32((int)errorCode);
                if (ex is not null)
                {
                    if (completionSource is not null)
                    {
                        completionSource?.SetException(ex);
                    }
                    else
                    {
                        Environment.FailFast(ASYNC_CALLBACK_FAIL_MESSAGE, new InvalidOperationException(ASYNC_CALLBACK_FAIL_MESSAGE, ex));
                    }
                }
            }
            else
            {
                if (completionSource is not null)
                {
                    completionSource.SetResult(numBytes);
                }
                else
                {
                    Environment.FailFast(ASYNC_CALLBACK_FAIL_MESSAGE, new InvalidOperationException(ASYNC_CALLBACK_FAIL_MESSAGE));
                }
            }
        }
    }
}
