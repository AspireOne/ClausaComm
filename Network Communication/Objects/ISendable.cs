namespace ClausaComm.Network_Communication.Objects
{
    public interface ISendable
    {
        public RemoteObject.ObjectType ObjType { get; }
        public bool Confirm { get; }
    }
}
