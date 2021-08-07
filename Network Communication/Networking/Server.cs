using System;
using System.Net;
using System.Net.Sockets;
using ClausaComm.Network_Communication.Objects;
using LiteNetLib;

namespace ClausaComm.Network_Communication.Networking
{
    // TODO: Maybe make static?
    // TODO: Remove LiteNetLib and implement it yourself.
    // The SERVER takes care ONLY of RECEIVING.
    class Server : InterCommunication
    {
        private readonly byte[] Buffer = new byte[8192];
        private readonly Action<RemoteObject, string> OnReceiveCallback;

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
