using System.ComponentModel;
using System.Windows.Forms;

namespace ClausaComm.Components
{
    public partial class MessageContainer : Panel
    {
        public MessageContainer()
        {
            InitializeComponent();
        }

        public MessageContainer(IContainer container) => container.Add(this);
    }
}