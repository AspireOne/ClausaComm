using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Network_Communication.Networking
{
    internal abstract class InterCommunication
    {
        protected const int Port = 9524;
        protected readonly EventBasedNetListener Listener = new EventBasedNetListener();
        protected readonly NetManager Node;

        protected InterCommunication()
        {
            Node = new NetManager(Listener)
            {
                UnconnectedMessagesEnabled = true,
                BroadcastReceiveEnabled = false,
                AutoRecycle = true // TODO: Maybe change
            };
        }
    }
}
