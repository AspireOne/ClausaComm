using LiteNetLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using ClausaComm.Network_Communication.Objects;

namespace ClausaComm.Network_Communication.Networking
{
    internal abstract class NetworkNode
    {
        public delegate void ReceiveHandler(RemoteObject message, IPEndPoint endpoint);
        public delegate void ConnectionChangeHandler(IPEndPoint endpoint);
        private static readonly byte[] Buffer = new byte[8192]; // One buffer for all connections? Damn, let's see how this goes.
        public ReceiveHandler? OnReceive;
        public ConnectionChangeHandler? OnDisconnect;
        public ConnectionChangeHandler? OnConnect;

        protected static bool Send(TcpClient client, byte[] bytes) // RemoteObject
        {
            if (!client.Connected)
                return false;

            try
            {
                client.GetStream().Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                Debug.WriteLine("A handled error occured while sending a message (writing to a stream) of a TcpClient.");
                Debug.WriteLine(e);
                return false;
            }
            
            return true;
        }
        
        protected void StartReading(TcpClient client)
        {
            client.ReceiveBufferSize = Buffer.Length; // Important!
            NetworkStream stream = client.GetStream();
            while (true)
            {
                // Using ReadByte to wait before bytes are available and only THEN lock the Buffer and
                // write it all into it. Otherwise the buffer would have to be always locked.
                byte firstByte;
                try
                {
                    firstByte = (byte)stream.ReadByte();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("An error occured while reading (/ waiting for) the first byte in a TcpClient's stream.");
                    Debug.WriteLine(e);
                    break;
                }

                lock (Buffer)
                {
                    Buffer[0] = firstByte;
                    int bytesRead;
                    try
                    {
                        bytesRead = stream.Read(Buffer, 1, Buffer.Length);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("An error occured while reading TcpClient's stream to a buffer.");
                        Debug.WriteLine(e);
                        break;
                    }

                    ++bytesRead; // + the byte we wrote ourselves before the bulk Read operation.
                    byte[] readBytes = new byte[bytesRead];
                    Array.Copy(Buffer, readBytes, bytesRead);
                    OnReceive?.Invoke(RemoteObject.Deserialize(readBytes), (IPEndPoint)client.Client.RemoteEndPoint);
                }
            }
            
            client.Close();
            OnDisconnect?.Invoke((IPEndPoint)client.Client.RemoteEndPoint);
        }
    }
}