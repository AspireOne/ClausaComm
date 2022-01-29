using System;
using ClausaComm.Contacts;

namespace ClausaComm.Network_Communication.Objects
{
    [Serializable]
    public readonly struct RemoteStatusUpdate : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjectType => RemoteObject.ObjectType.StatusUpdate;
        public Contact.Status Status { get; init; }

        public RemoteStatusUpdate(Contact.Status status) => Status = status;
    }
}
