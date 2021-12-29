using ClausaComm.Contacts;

namespace ClausaComm.Network_Communication.Objects
{
    readonly struct RemoteStatusUpdate : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjectType => RemoteObject.ObjectType.StatusUpdate;
        bool ISendable.Confirm => false;
        public readonly Contact.Status Status;

        public RemoteStatusUpdate(Contact.Status status)
        {
            Status = status;
        }
    }
}
