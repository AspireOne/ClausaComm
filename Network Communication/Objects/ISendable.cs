namespace ClausaComm.Network_Communication.Objects
{
    public interface ISendable
    {
        public RemoteObject.ObjectType ObjectType { get; }
        public bool Confirm { get; }
    }
}
