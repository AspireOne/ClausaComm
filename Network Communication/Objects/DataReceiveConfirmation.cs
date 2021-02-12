using ClausaComm.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Network_Communication.Objects
{
    public struct DataReceiveConfirmation : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjType => RemoteObject.ObjectType.DataReceiveConfirmation;
        public readonly string ConfirmedDataId;
        bool ISendable.Confirm => false;

        public DataReceiveConfirmation(string confirmedDataId)
        {
            ConfirmedDataId = confirmedDataId;
        }
    }
}
