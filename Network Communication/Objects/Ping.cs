namespace ClausaComm.Network_Communication.Objects
{
    internal struct Ping : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjType => RemoteObject.ObjectType.Ping;
        bool ISendable.Confirm => false;
    }
}