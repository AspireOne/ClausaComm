using System.ComponentModel;

namespace ClausaComm.Components.Icons
{
    public partial class SendIcon : ImageIconBase
    {
        public SendIcon() : base(Properties.Resources.send_icon) { }

        public SendIcon(IContainer container) : this() => container.Add(this);
    }
}
