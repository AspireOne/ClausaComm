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
    class Client : InterCommunication
    {
        public Client()
        {
            Listener.NetworkReceiveUnconnectedEvent += (IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) =>
            {
                Debug.WriteLine($"Client received unconnected message from {remoteEndPoint}. Message: {reader.GetString()}");
            };

            RunAsync();
            ThreadUtils.RunThread(() =>
            {
                //Node.Connect("192.168.1.236", Port, "");
                while (true)
                {
                    //bool sent = Node.SendUnconnectedMessage(writer, NetUtils.MakeEndPoint("192.168.1.236", Port));     
                    //Debug.WriteLine("sent: " + sent);
                    Thread.Sleep(600);
                }
            });
        }

        public bool Send(string ip, RemoteObject obj)
        {
            return Node.SendUnconnectedMessage(obj.SerializeToUtf8Bytes(), NetUtils.MakeEndPoint(ip, Port));
        }

        public void RunAsync()
        {
            if (Node.IsRunning)
                return;

            Node.Start();
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
