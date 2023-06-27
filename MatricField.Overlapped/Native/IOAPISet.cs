using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MatricField.Overlapped.Native
{
    internal static partial class IOAPISet
    {
        [LibraryImport("kernel32.dll", EntryPoint = "DeviceIoControl")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool DeviceIoControl(
            SafeHandle hDevice,
            uint dwIoControlCode,
            byte[]? lpInBuffer,
            int nInBufferSize,
            byte[]? lpOutBuffer,
                int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr overlapped
            );
    }
}
