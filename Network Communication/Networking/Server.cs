using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClausaComm.Network_Communication.Networking;
using ClausaComm.Utils;
using LiteNetLib;
using LiteNetLib.Utils;

namespace ClausaComm.Network
{
    // TODO: Maybe make static?
    // TODO: Remove LiteNetLib and implement it yourself.
    // The SERVER takes care ONLY of RECEIVING.
    class Server : InterCommunication
    {
        private readonly byte[] Buffer = new byte[8192];
        private Action<RemoteObject, string> OnReceiveCallback;

        public Server(Action<RemoteObject, string> onReceiveCallback)
        {
            OnReceiveCallback = onReceiveCallback;

            Listener.NetworkReceiveUnconnectedEvent += (IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) =>
            {
                reader.GetBytes(Buffer, reader.AvailableBytes);
                RemoteObject obj = RemoteObject.Deserialize(Buffer);
                // If the IP is Ipv6, try to use remoteEndPoint.Address.MapToIpv4();
                if (remoteEndPoint.Address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    throw new Exception("The received address is IPv6. The code to handle this was not written yet, because" +
                        " you thought, that it won't be in IPv6. So better fix it.");
                }
                OnReceiveCallback(obj, remoteEndPoint.Address.ToString());
            };
        }
    }
}
