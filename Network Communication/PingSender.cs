using ClausaComm.Network_Communication.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClausaComm.Network_Communication
{
    public class PingSender
    {
        public static readonly TimeSpan interval = TimeSpan.FromSeconds(90);
        public static bool IsRunning { get; private set; }

        private static readonly Timer Timer = new(interval.TotalMilliseconds);
        private static readonly RemoteObject Ping = new(new Ping());

        private readonly Action<string, RemoteObject> SendMethod;
        public HashSet<Contact> Destinations { get; set; } = new();



        public PingSender(Action<string, RemoteObject> sendMethod)
        {
            Timer.Elapsed += OnIntervalPassed;
            SendMethod = sendMethod;
        }

        private void OnIntervalPassed(object o, ElapsedEventArgs e)
        {
            foreach (Contact dest in Destinations)
            {
                SendMethod.Invoke(dest.Ip, Ping);
            }
        }

        // Do not mark as static. 
        public void Start()
        {
            if (IsRunning)
                throw new Exception($"One instance of {nameof(PingSender)} is already running.");

            Timer.Start();
            IsRunning = true;
        }
    }
}
