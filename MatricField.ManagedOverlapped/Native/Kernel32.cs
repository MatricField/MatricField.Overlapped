using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MatricField.ManagedOverlapped.Native
{
    internal static partial class Kernel32
    {
        [LibraryImport("kernel32.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public unsafe static partial bool DeviceIoControl(
            SafeHandle hDevice,
            uint dwIoControlCode,
            [Optional] nint lpInBuffer,
            int nInBufferSize,
            [Optional] nint lpOutBuffer,
            int nOutBufferSize,
            [Optional] uint* lpBytesReturned,
            [Optional] NativeOverlapped* overlapped
            );

        [LibraryImport("kernel32.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public unsafe static partial bool DeviceIoControl(
            SafeHandle hDevice,
            uint dwIoControlCode,
            [Optional] byte[]? lpInBuffer,
            int nInBufferSize,
            [Optional] byte[]? lpOutBuffer,
            int nOutBufferSize,
            [Optional] uint* lpBytesReturned,
            [Optional] NativeOverlapped* overlapped
            );

        [LibraryImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public unsafe static partial bool CancelIoEx(SafeHandle hFile, NativeOverlapped* lpOverlapped);
    }
}
