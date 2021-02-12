using ClausaComm.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Network_Communication.Objects
{
    struct Ping : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjType => RemoteObject.ObjectType.Ping;
        bool ISendable.Confirm => false;
    }
}
