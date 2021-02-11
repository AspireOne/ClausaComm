using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ClausaComm.Utils
{
    public static class ThreadUtils
    {
        public static void RunThread(Action action)
        {
            new Thread(new ThreadStart(action))
            {
                Name = "ClausaComm Thread",
                IsBackground = true
            }.Start();
        }
    }
}
