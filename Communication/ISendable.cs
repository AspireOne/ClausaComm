using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Communication
{
    public interface ISendable
    {
        public RemoteObject.ObjectType ObjType { get; }
    }
}
