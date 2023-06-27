using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MatricField.ManagedOverlapped.Native
{
    internal static class Win32
    {
        public const int FACILITY_WIN32 = 7;

        public const int ErrorIOPending = 997;
        public static int GetHResultForWin32(int errorCode)
        {
            if(errorCode < 0)
            {
                return errorCode;
            }
            else
            {
                return (int)(((uint)errorCode & 0x0000FFFF) | (FACILITY_WIN32 << 16) | 0x80000000);
            }
        }

        public static Exception? GetExceptionForWin32(int errcode) =>
            Marshal.GetExceptionForHR(GetHResultForWin32(errcode));

        public static Exception? GetLastWin32Exception() =>
            GetExceptionForWin32(Marshal.GetLastWin32Error());
    }
}
