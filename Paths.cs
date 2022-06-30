using System;
using System.IO;
using ClausaComm.Utils;
using Microsoft.Win32;

namespace ClausaComm;

public static class Paths
{
    //public const string RegistryUninstallPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ClausaComm";
    private static readonly string _fileSavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ClausaComm Files");
    public static string FileSavePath
    {
        get
        {
            Utils.Utils.CreateIfDoesntExist(_fileSavePath);
            return _fileSavePath;
        }
    }

    /*private static string? InstallationDirCached;
    public static string? InstallationDir
    {
        get
        {
            if (InstallationDirCached is not null)
                return InstallationDirCached;
            
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryUninstallPath);
            string? dir = key?.GetValue("InstallLocation")?.ToString()?.Replace("\"", "");
            if (dir is null or "" or " " || dir.Length < 5)
                return null;
            InstallationDirCached = dir;
            return InstallationDirCached;
        }
    }

    public static string? InstallationExe
        => InstallationDir is null ? null : Path.Combine(InstallationDir, $"{Program.Name}.exe");*/
}