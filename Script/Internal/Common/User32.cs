#if UNITY_STANDALONE_WIN

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace XD.SDK.Common.PC.Internal {
    public class User32 {
        private const int SW_SHOWNORMAL = 1;

        private delegate bool EnumThreadDelegate(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        private static IntPtr GetWindowHandle()
        {
            IntPtr returnHwnd = IntPtr.Zero;
            var threadId = GetCurrentThreadId();
            Debug.Log("Current thread id: " + threadId);
            EnumThreadWindows(threadId,
                (hWnd, lParam) =>
                {
                    if (returnHwnd == IntPtr.Zero)
                        returnHwnd = hWnd;
                    return true;
                }, IntPtr.Zero);
            return returnHwnd;
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, IntPtr lpvParam, int fuWinIni);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int GetForegroundWindow();
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        [DllImport("User32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern int AttachThreadInput(int CurrentForegroundThread, int MakeThisThreadForegrouond, bool boolAttach);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetCurrentThreadId();
        [DllImport("user32", CharSet = CharSet.Auto)]
        static extern bool SystemParametersInfo(int uiAction, int uiParam, ref int pvParam, int fWinIni);
        [DllImport("user32", CharSet = CharSet.Auto)]
        static extern bool LockSetForegroundWindow(int a);

        [DllImport("user32", CharSet = CharSet.Auto)]
        static extern bool AllowSetForegroundWindow(int a);

        internal static void ForceWindowIntoForeground()
        {
            IntPtr window = GetWindowHandle();
            int currentThread = GetCurrentThreadId();
            int activeWindow = GetForegroundWindow();
            int activeThread = GetWindowThreadProcessId((IntPtr)activeWindow, out int activeProcess);
            int windowThread = GetWindowThreadProcessId(window, out int windowProcess);
            if (currentThread != activeThread)
                AttachThreadInput(currentThread, activeThread, true);
            if (windowThread != currentThread)
                AttachThreadInput(windowThread, currentThread, true);

            int oldTimeout = 0, newTimeout = 0;
            SystemParametersInfo(0x2000, 0, ref oldTimeout, 0);
            SystemParametersInfo(0x2000, 0, ref newTimeout, 0);
            LockSetForegroundWindow(2);
            AllowSetForegroundWindow(-1);

            SetForegroundWindow(window);
            ShowWindow(window, SW_SHOWNORMAL);

            SystemParametersInfo(0x2000, 0, ref oldTimeout, 0);

            if (currentThread != activeThread)
                AttachThreadInput(currentThread, activeThread, false);
            if (windowThread != currentThread)
                AttachThreadInput(windowThread, currentThread, false);
        }
    }
}

#endif