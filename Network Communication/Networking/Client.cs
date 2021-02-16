using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using ClausaComm.Network_Communication.Objects;
using LiteNetLib;

namespace ClausaComm.Network_Communication.Networking
{
    // The CLIENT takes care ONLY of SENDING.
    internal class Client : InterCommunication
    {
        public Client()
        {
            Listener.NetworkReceiveUnconnectedEvent += (IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) =>
            {
                throw new("Sending data to client's endpoint is not allowed.");
            };
        }

        public bool Send(string ip, RemoteObject obj)
        {
            try
            {
                return Node.SendUnconnectedMessage(obj.SerializeToUtf8Bytes(), NetUtils.MakeEndPoint(ip, Port));
            }
            catch (SocketException e)
            {
                Debug.WriteLine($"Error uccured while sending. Ip: {ip}. Error: {e}");
                return false;
            }
        }
    }
}