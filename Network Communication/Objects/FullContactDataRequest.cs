namespace ClausaComm.Network_Communication.Objects
{
    public struct FullContactDataRequest : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjType => RemoteObject.ObjectType.FullContactDataRequest;

        bool ISendable.Confirm => false;
    }
}