using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RFIDAttendance.Common;

namespace RFIDAttendance.WinAPI
{
    class WindowAPI
    {
        public delegate bool EnumDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern bool EnumThreadWindows(int dwThreadId, EnumDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out System.Windows.Rect lpRect);

        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_RESTORE = 9;

        public static void MouseLeftClick(IntPtr hWnd, POINT location)
        {
            PostMessage(hWnd, (UInt32)GlobalVariables.MOUSE_LEFTDOWN, (Int32)GlobalVariables.MOUSE_PARAM_LBUTTON, location.ToPostParam());
            PostMessage(hWnd, (UInt32)GlobalVariables.MOUSE_LEFTUP, 0, location.ToPostParam());
        }

        public delegate bool WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        public static bool EnumerateChildWindow(IntPtr hwnd, IntPtr lParam)
        {
            bool result = false;
            GCHandle gch = GCHandle.FromIntPtr(lParam);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list != null)
            {
                list.Add(hwnd);
                result = true; // return true as long as children are found
            }
            return result;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc callback, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern Int32 GetWindowText(IntPtr hWnd, StringBuilder s, int nMaxCount);

        [DllImport("User32.dll", EntryPoint = "RegisterWindowMessage")]
        public static extern int RegisterWindowMessage(string lpString);


        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "ReplyMessage")]
        public static extern bool ReplyMessage(IntPtr lResult);

        [DllImport("user32", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr zeroHandle, string windowName);
    }
}
