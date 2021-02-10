using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
