using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Utils
{
    public static class ConsoleUtils
    {

        public static ProcessStartInfo GetProcessStartInfo(string argument, bool fromSystem32, bool admin)
        {
            return new()
            {
                UseShellExecute = admin,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = !admin,
                FileName = "cmd.exe",
                Arguments = "/C " + (fromSystem32 ? "cd " + Environment.SystemDirectory + " & " : "") + argument,
                Verb = admin ? "runas" : ""
            };
        }

        public static string GetDelay(int secs) => "ping 127.0.0.1 -n " + (secs + 1) + " > nul";
    }
}
