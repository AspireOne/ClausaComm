namespace ClausaComm.Network_Communication.Objects
{
    internal struct Ping : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjectType => RemoteObject.ObjectType.Ping;
    }
}