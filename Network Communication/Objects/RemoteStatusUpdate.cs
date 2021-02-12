using ClausaComm.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Network_Communication
{
    readonly struct RemoteStatusUpdate : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjType => RemoteObject.ObjectType.StatusUpdate;
        bool ISendable.Confirm => false;
        public readonly Contact.Status Status;

        public RemoteStatusUpdate(Contact.Status status)
        {
            Status = status;
        }
    }
}
