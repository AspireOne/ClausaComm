using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClausaComm.Extensions;
using ClausaComm.Messages;
using ClausaComm.Network_Communication.Objects;
using ClausaComm.Utils;

namespace ClausaComm.Network_Communication.Networking
{
    internal abstract class NetworkPeer
    {
        private static readonly object WriteLock = new();
        public delegate void ReceiveHandler(RemoteObject message, IPEndPoint endpoint);
        public delegate void ConnectionChangeHandler(IPEndPoint endpoint);
        private static readonly byte[] ReadBuffer = new byte[5 * 1_048_576]; // x mb * bytes.
        private const byte DataLengthLength = 8;
        private const int FileBufferSize = 5 * 1_048_576;
        public bool Running { get; protected set; }
        public event ReceiveHandler? OnReceive;
        public event ConnectionChangeHandler? OnDisconnect;
        public event ConnectionChangeHandler? OnConnect;
        
        // So that derived classes can raise the event.
        protected void RaiseConnect(IPEndPoint endpoint) => OnConnect?.Invoke(endpoint);

        protected static bool Send(TcpClient client, RemoteObject obj) // RemoteObject
        {
            if (!client.Connected)
            {
                Logger.Log($"{nameof(NetworkPeer)}: Send method was invoked to {(IPEndPoint)client.Client.RemoteEndPoint} but the client is not connected.");
                return false;
            }

            string dataStr = obj.Serialize();
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataStr);
            byte[] dataLength = BitConverter.GetBytes((long)dataBytes.Length);
            
            try
            {
                NetworkStream ns = client.GetStream();
                lock (WriteLock)
                {
                    Logger.Log($"{nameof(NetworkPeer)}: Sending {dataBytes.Length} bytes of {obj.Data.ObjectType.ToString()}.");
                    ns.Write(dataLength, 0, dataLength.Length);
                    ns.Write(dataBytes, 0, dataBytes.Length);
                    Logger.Log($"{nameof(NetworkPeer)}: {obj.Data.ObjectType.ToString()} Sent.");
                    if (obj.Data.ObjectType != RemoteObject.ObjectType.File)
                        return true;
                    
                    RemoteFile file = (RemoteFile)obj.Data;
                    SendFile(ns, file);
                }
            }
            catch (Exception e)
            {
                Logger.Log($"{nameof(NetworkPeer)}: A handled error occured while sending a message (writing to a stream) of a TcpClient.");
                Logger.Log(e);
                return false;
            }
            
            return true;
        }

        private static void SendFile(NetworkStream ns, RemoteFile file)
        {
            using FileStream fs = File.OpenRead(file.FilePath);
            byte[] dataLength = BitConverter.GetBytes(fs.Length);
            ns.Write(dataLength, 0, dataLength.Length);
            
            byte[] buffer = new byte[FileBufferSize];
            long remaining = fs.Length;
            
            while (remaining != 0)
            {
                int bytesRead = fs.Read(buffer, 0, buffer.Length);
                remaining -= bytesRead;
                ns.Write(buffer, 0, bytesRead);
            }
            
            fs.Flush();
            fs.Dispose();
        }
        
        protected void StartReading(TcpClient client)
        {
            Logger.Log($"{nameof(NetworkPeer)}: StartReading invoked on [{(IPEndPoint)client.Client.RemoteEndPoint}]. Connected: {client.Connected}");
            if (!client.Connected)
                return;
            
            // Save it here so that we can still retrieve it after the client disconnects (e.g. in OnDisconnect(IPEndPoint) event).
            var remoteHost = (IPEndPoint)client.Client.RemoteEndPoint;
            NetworkStream ns = client.GetStream();
            
            client.ReceiveBufferSize = ReadBuffer.Length; // Important!

            byte[] lengthBuffer = new byte[DataLengthLength];
            while (true)
            {
                // Using ReadByte to wait before bytes are available and only THEN lock the Buffer and
                // write it all into it. Otherwise the buffer would have to be always locked.
                try
                {
                    ns.Read(lengthBuffer, 0, lengthBuffer.Length);
                }
                catch (Exception e)
                {
                    Logger.Log($"{nameof(NetworkPeer)}: An error occured while reading (/ waiting for) the first byte in a TcpClient's stream (endpoint: {remoteHost}).");
                    Logger.Log(e);
                    break;
                }

                lock (ReadBuffer)
                {
                    try
                    {
                        RemoteObject obj = ReadObject((int)BitConverter.ToInt64(lengthBuffer), client);
                        Logger.Log($"read object ({obj.Data.ObjectType}).");
                        if (obj.Data.ObjectType != RemoteObject.ObjectType.File)
                        {
                            Task.Run(() => OnReceive?.Invoke(obj, remoteHost));
                            continue;
                        }
                        
                        Logger.Log($"Received a file, reading data length.");
                        var file = (RemoteFile)obj.Data;
                        string path = Path.Combine(ProgramDirectory.FileSavePath, file.FileName);
                        ns.Read(lengthBuffer, 0, lengthBuffer.Length);
                        long dataLength = BitConverter.ToInt64(lengthBuffer);
                        Logger.Log($"Got data length, reading " + dataLength + " bytes of data.");
                        ReadFile(dataLength, path, ns);
                        Logger.Log($"Data read.");
                    }
                    catch (Exception e)
                    {
                        Logger.Log($"{nameof(NetworkPeer)}: An error occured while reading TcpClient's stream (endpoint: {remoteHost}).");
                        Logger.Log(e);
                        break;
                    }
                }
            }
            
            Logger.Log($"{nameof(NetworkPeer)}: Stopping reading. (endpoint: {remoteHost}).");
            client.Close();
            OnDisconnect?.Invoke(remoteHost);
        }

        private static void ReadFile(long fileLength, string filepath, NetworkStream ns)
        {
            // TODO: Just rename.
            if (File.Exists(filepath))
                File.Delete(filepath);
            
            using var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
            long remaining = fileLength;
            
            while (remaining != 0)
            {
                int bytesRead = ns.Read(ReadBuffer, 0, ReadBuffer.Length);
                fs.Write(ReadBuffer, 0, bytesRead);
                remaining -= bytesRead;
            }
            
            fs.Flush();
            fs.Dispose();
        }

        private static RemoteObject ReadObject(int objLength, TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            int remaining = objLength;
            
            while (remaining != 0)
            {
                int bytesRead = ns.Read(ReadBuffer, objLength - remaining, remaining);
                remaining -= bytesRead;
            }

            byte[] objBytes = new byte[objLength];
            Array.Copy(ReadBuffer, objBytes, objLength);
            return RemoteObject.Deserialize(objBytes);
        }
    }
}