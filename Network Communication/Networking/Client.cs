using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClausaComm.Network;
using ClausaComm.Utils;
using LiteNetLib;
using LiteNetLib.Utils;

namespace ClausaComm.Network_Communication.Networking
{
    // The CLIENT takes care ONLY of SENDING.
    internal class Client : InterCommunication
    {
        public Client()
        {
            Listener.NetworkReceiveUnconnectedEvent += (IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) =>
            {
                throw new Exception("Sending data to client's endpoint is not allowed.");
            };
        }

        public bool Send(string ip, RemoteObject obj)
        {
            return Node.SendUnconnectedMessage(obj.SerializeToUtf8Bytes(), NetUtils.MakeEndPoint(ip, Port));
        }
    }
}
