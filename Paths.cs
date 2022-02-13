using System;
using System.IO;

namespace ClausaComm;

public static class Paths
{
    private static readonly string _FileSavePath
        = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ClausaComm Files");

    public static string FileSavePath
    {
        get
        {
            if (!Directory.Exists(_FileSavePath))
                Directory.CreateDirectory(_FileSavePath);
            
            return _FileSavePath;
        }
    }
}