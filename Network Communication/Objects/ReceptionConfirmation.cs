namespace ClausaComm.Network_Communication.Objects
{
    public readonly struct ReceptionConfirmation : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjectType => RemoteObject.ObjectType.DataReceiveConfirmation;
        public readonly string ConfirmedDataId;
        bool ISendable.Confirm => false;

        public ReceptionConfirmation(string confirmedDataId)
        {
            ConfirmedDataId = confirmedDataId;
        }
    }
}