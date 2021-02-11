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
    // The SERVER takes care ONLY of RECEIVING.
    class Server : InterCommunication
    {
        private readonly byte[] Buffer = new byte[8192];

        public Server()
        {
            Listener.ConnectionRequestEvent += request =>
            {
                throw new Exception("The data should be sent without being connected.");
                //request.Accept();
                //request.Data
            };

            Listener.NetworkReceiveUnconnectedEvent += (IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) =>
            {
                reader.GetBytes(Buffer, reader.AvailableBytes);
                string msg = Encoding.UTF8.GetString(Buffer);
                Debug.WriteLine($"Server received unconnected message from {remoteEndPoint.Address}. Message: " + msg);
                NetDataWriter writer = new NetDataWriter();
                writer.Put("Hello client!");
                Node.SendUnconnectedMessage(writer, remoteEndPoint);
            };
        }

        //public void NetworkReceive

        public void RunAsync()
        {
            Node.Start(Port);

            ThreadUtils.RunThread(() =>
            {
                while (true)
                {
                    Node.PollEvents();
                    Thread.Sleep(15);
                }
            });
        }
    }
}
