using ClausaComm.Contacts;

namespace ClausaComm.Network_Communication.Objects
{
    internal readonly struct RemoteStatusUpdate : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjectType => RemoteObject.ObjectType.StatusUpdate;
        public readonly Contact.Status Status;

        public RemoteStatusUpdate(Contact.Status status) => Status = status;
    }
}
