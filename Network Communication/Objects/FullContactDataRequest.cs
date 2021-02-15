using ClausaComm.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Network_Communication.Objects
{
    public struct FullContactDataRequest : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjType => RemoteObject.ObjectType.FullContactDataRequest;

        bool ISendable.Confirm => false;
    }
}
