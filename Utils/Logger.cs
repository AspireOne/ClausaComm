using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ClausaComm.Utils;

public static class Logger
{
    public const int MaxTextLength = 500;
    public static void Log(string? str)
    {
#if !DEBUG
        return;
#endif
        if (str?.Length > MaxTextLength)
            str = $"{str[..MaxTextLength]}... ({str.Length - MaxTextLength} characters truncated)";

        Debug.WriteLine(str);
    }
    
    public static void Log(object? text) => Log(text?.ToString());

    public static void Log(Func<object> text, bool async = false)
    {
#if !DEBUG
        return;
#endif
        
        if (async)
            Task.Run(() => Log(text()));
        else
            Log(text());
    }
}