namespace ClausaComm.Network_Communication.Objects
{
    public readonly struct DataReceiveConfirmation : ISendable
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