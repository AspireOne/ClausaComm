using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ClausaComm.Network_Communication.Objects;

namespace ClausaComm.Network_Communication.Networking
{
    internal abstract class NetworkNode
    {
        public delegate void ReceiveHandler(RemoteObject message, IPEndPoint endpoint);
        public delegate void ConnectionChangeHandler(IPEndPoint endpoint);
        private static readonly byte[] ReadBuffer = new byte[10485760]; // One buffer for all connections? Damn, let's see how this goes.
        public bool Running { get; protected set; }
        public ReceiveHandler? OnReceive;
        public ConnectionChangeHandler? OnDisconnect;
        public ConnectionChangeHandler? OnConnect;

        protected static bool Send(TcpClient client, byte[] bytes) // RemoteObject
        {
            if (!client.Connected)
            {
                Debug.WriteLine($"{nameof(NetworkNode)}: Send method was invoked but the client is not connected.");
                return false;
            }
            
            Debug.WriteLine($"{nameof(NetworkNode)}: Send method invoked to endpoint {(IPEndPoint)client.Client.RemoteEndPoint}");

            try
            {
                lock (client.GetStream())
                {
                    Debug.WriteLine($"{nameof(NetworkNode)} sending {Encoding.UTF8.GetString(bytes)}");
                    var data = bytes.Concat(new byte[] { 255 }).ToArray();
                    client.GetStream().Write(data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(NetworkNode)}: A handled error occured while sending a message (writing to a stream) of a TcpClient.");
                Debug.WriteLine(e);
                return false;
            }
            
            return true;
        }
        
        protected void StartReading(TcpClient client)
        {
            Debug.WriteLine($"{nameof(NetworkNode)}: StartReading invoked (endpoint {(IPEndPoint)client.Client.RemoteEndPoint}). Connected: {client.Connected}");
            if (!client.Connected)
                return;
            
            // Save it here so that we can still retrieve it after the client disconnects (e.g. in OnDisconnect(IPEndPoint) event).
            var remoteHost = (IPEndPoint)client.Client.RemoteEndPoint;
            client.ReceiveBufferSize = ReadBuffer.Length; // Important!

            NetworkStream stream = client.GetStream();
            while (true)
            {
                // Using ReadByte to wait before bytes are available and only THEN lock the Buffer and
                // write it all into it. Otherwise the buffer would have to be always locked.
                Debug.WriteLine($"{nameof(NetworkNode)}: Waiting for the first byte {remoteHost}");
                byte firstByte;
                try { firstByte = (byte)stream.ReadByte(); }
                catch (Exception e)
                {
                    Debug.WriteLine($"{nameof(NetworkNode)}: An error occured while reading (/ waiting for) the first byte in a TcpClient's stream (endpoint: {remoteHost}).");
                    Debug.WriteLine(e);
                    break;
                }

                Debug.WriteLine($"{nameof(NetworkNode)}: Reading everything in stream {remoteHost}");
                lock (ReadBuffer)
                {
                    ReadBuffer[0] = firstByte;
                    int readBytesAmount = 1;
                    try
                    {
                        while (true)
                        {
                            int nextByte = stream.ReadByte();
                            
                            if (nextByte == -1)
                            {
                                Debug.WriteLine($"{nameof(NetworkNode)}: Reached end of stream (-1 returned) but didn't get the whole message yet. Waiting. {remoteHost}");
                                continue;
                            }
                            
                            if (nextByte == 255) //0xFF
                            {
                                Debug.WriteLine($"{nameof(NetworkNode)}: Reached the end of data. Bytes read: {readBytesAmount} {remoteHost}");
                                break;
                            }

                            ReadBuffer[readBytesAmount++] = (byte)nextByte;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"{nameof(NetworkNode)}: An error occured while reading TcpClient's stream (endpoint: {remoteHost}).");
                        Debug.WriteLine(e);
                        break;
                    }
                    
                    Debug.WriteLine($"{nameof(NetworkNode)}: Succesfully read data from a stream (endpoint: {remoteHost}).");
                    byte[] readBytes = new byte[readBytesAmount];
                    Array.Copy(ReadBuffer, readBytes, readBytesAmount);
                    Debug.WriteLine("the gotten data: " + Encoding.UTF8.GetString(RemoteObject.Deserialize(readBytes).SerializeToUtf8Bytes()));
                    OnReceive?.Invoke(RemoteObject.Deserialize(readBytes), remoteHost);
                }
                
                Debug.WriteLine($"{nameof(NetworkNode)}: Everything read {remoteHost}");
            }
            
            Debug.WriteLine($"{nameof(NetworkNode)}: Stopping reading. (endpoint: {remoteHost}).");
            client.Close();
            OnDisconnect?.Invoke(remoteHost);
        }
    }
}