using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClausaComm.Network_Communication.Networking
{
    internal abstract class InterCommunication
    {
        public bool Running { get; private set; } = false;

        // TODO: Replace with System.Threading.Timer because it's more lightweight.
        protected static readonly Timer PollTimer = new(50) { Enabled = false };

        protected const int Port = 9524;
        protected readonly EventBasedNetListener Listener = new EventBasedNetListener();
        protected readonly NetManager Node;

        protected InterCommunication()
        {
            Node = new NetManager(Listener)
            {
                UnconnectedMessagesEnabled = true,
                BroadcastReceiveEnabled = false,
                AutoRecycle = true
            };

            Listener.ConnectionRequestEvent += request
                => throw new Exception($"Connection request received from {request.RemoteEndPoint.Address}. Data should only be sent without being connected.");
        }

        public void Start()
        {
            if (Running)
                return;

            Running = true;
            if (!PollTimer.Enabled)
                PollTimer.Enabled = true;

            PollTimer.Elapsed += OnPollTimerTick;
        }

        protected void OnPollTimerTick(object o, ElapsedEventArgs e)
        {
            Node.PollEvents();
        }
    }
}