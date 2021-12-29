namespace ClausaComm.Network_Communication.Objects
{
    public struct FullContactDataRequest : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjectType => RemoteObject.ObjectType.FullContactDataRequest;
    }
}