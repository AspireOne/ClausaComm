using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ClausaComm.Utils;

public static class Utils
{
    [DllImport("User32.dll")] private static extern bool SetForegroundWindow(IntPtr handle);
    [DllImport("User32.dll")] private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
    [DllImport("User32.dll")] private static extern bool IsIconic(IntPtr handle);

    public static void BringToFront(Process p) {
        IntPtr handle = p.MainWindowHandle;
            
        if (IsIconic(handle))
            ShowWindow(handle, 9 /* SW_RESTORE*/);
            
        SetForegroundWindow(handle);
    }
}