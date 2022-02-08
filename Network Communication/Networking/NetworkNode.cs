using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using ClausaComm.Extensions;
using ClausaComm.Network_Communication.Objects;
using ClausaComm.Utils;

namespace ClausaComm.Network_Communication.Networking
{
    internal abstract class NetworkNode
    {
        public delegate void ReceiveHandler(RemoteObject message, IPEndPoint endpoint);
        public delegate void ConnectionChangeHandler(IPEndPoint endpoint);
        // TODO: Change the implementation so that > 10mb (pictures, files etc.) fit.
        private static readonly byte[] ReadBuffer = new byte[10 * 1048576]; // x mb * bytes.
        // End Of Data byte - signalises the end of data.
        private const byte EodByte = 255; // End Of Data Byte: 0xFF - forbidden by UTF-8.
        private const byte EopdByte = 254; // End Of Partial Data Byte: 0xFE - forbidden by UTF-8.
        public bool Running { get; protected set; }
        public ReceiveHandler? OnReceive;
        public ConnectionChangeHandler? OnDisconnect;
        public ConnectionChangeHandler? OnConnect;
        
        protected static bool Send(TcpClient client, RemoteObject obj) // RemoteObject
        {
            if (!client.Connected)
            {
                Logger.Log($"{nameof(NetworkNode)}: Send method was invoked to {(IPEndPoint)client.Client.RemoteEndPoint} but the client is not connected.");
                return false;
            }
            
            Logger.Log($"{nameof(NetworkNode)}: Send method invoked to endpoint {(IPEndPoint)client.Client.RemoteEndPoint}");

            string dataStr = obj.Serialize();
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataStr);
            try
            {
                lock (client.GetStream())
                {
                    Logger.Log($"{nameof(NetworkNode)} sending ({dataBytes.Length} bytes): {dataStr}");
                    byte[] data = dataBytes.Concat(new[] { EodByte }).ToArray();
                    client.GetStream().Write(data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                Logger.Log($"{nameof(NetworkNode)}: A handled error occured while sending a message (writing to a stream) of a TcpClient.");
                Logger.Log(e);
                return false;
            }
            
            return true;
        }
        
        protected void StartReading(TcpClient client)
        {
            Logger.Log($"{nameof(NetworkNode)}: StartReading invoked (endpoint {(IPEndPoint)client.Client.RemoteEndPoint}). Connected: {client.Connected}");
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
                Logger.Log($"{nameof(NetworkNode)}: Waiting for the first byte {remoteHost}");
                byte firstByte;
                try { firstByte = (byte)stream.ReadByte(); }
                catch (Exception e)
                {
                    Logger.Log($"{nameof(NetworkNode)}: An error occured while reading (/ waiting for) the first byte in a TcpClient's stream (endpoint: {remoteHost}).");
                    Logger.Log(e);
                    break;
                }

                Logger.Log($"{nameof(NetworkNode)}: Reading everything in stream {remoteHost}");
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
                                Logger.Log($"{nameof(NetworkNode)}: Reached end of stream (-1 returned) but didn't get the whole message yet. Waiting. {remoteHost}");
                                continue;
                            }
                            
                            if (nextByte == EodByte)
                            {
                                Logger.Log($"{nameof(NetworkNode)}: Reached the end of data. Bytes read: {readBytesAmount} {remoteHost}");
                                break;
                            }

                            ReadBuffer[readBytesAmount++] = (byte)nextByte;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log($"{nameof(NetworkNode)}: An error occured while reading TcpClient's stream (endpoint: {remoteHost}).");
                        Logger.Log(e);
                        break;
                    }
                    
                    Logger.Log($"{nameof(NetworkNode)}: Succesfully read data from a stream (endpoint: {remoteHost}).");
                    byte[] readBytes = new byte[readBytesAmount];
                    Array.Copy(ReadBuffer, readBytes, readBytesAmount);
                    Logger.Log(() =>
                    {
                        string data = Encoding.UTF8.GetString(readBytes);
                        return "the gotten data: " + data;
                    }, true);
                    /*new Thread(() =>
                    {
                        Thread.Sleep(1000);
                        OnReceive?.Invoke(RemoteObject.Deserialize(readBytes), remoteHost);
                    }).Start();*/
                    OnReceive?.Invoke(RemoteObject.Deserialize(readBytes), remoteHost);
                }
                
                Logger.Log($"{nameof(NetworkNode)}: Everything read {remoteHost}");
            }
            
            Logger.Log($"{nameof(NetworkNode)}: Stopping reading. (endpoint: {remoteHost}).");
            client.Close();
            OnDisconnect?.Invoke(remoteHost);
        }
    }
}