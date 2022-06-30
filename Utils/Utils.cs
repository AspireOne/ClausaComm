using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;

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
    
    public static void CreateIfDoesntExist(string path)
    {
        bool isDirectory = !Path.GetFileName(path).Contains('.');
            
        if (isDirectory && !Directory.Exists(path))
            Directory.CreateDirectory(path);
        else if (!isDirectory && !File.Exists(path))
        {
            if (path.EndsWith("xml"))
                CreateNewXml(path);
            else
                File.Create(path).Close();
        }
    }
    
    private static void CreateNewXml(string path) => new XDocument(new XElement("doc")).Save(path);
}