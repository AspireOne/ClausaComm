using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausaComm.Components
{
    public partial class PanelOfContactPanels : Panel
    {
        public IEnumerable<ContactPanel> Panels
        {
            get
            {
                foreach (ContactPanel panel in Controls)
                    yield return panel;
            }
        }

        public PanelOfContactPanels()
        {
            InitializeComponent();
        }

        public PanelOfContactPanels(IContainer container) : this() => container.Add(this);

        public void RemovePanel(Contact contact)
        {
            ContactPanel panel = Panels.First(panel => panel.Contact.Ip == contact.Ip);
            if (panel is not null)
                Controls.Remove(panel);
        }

        public void SimulateClickOnFirstPanel()
        {
            if (Panels.Any())
                InvokeOnClick(Panels.Last(), EventArgs.Empty);
        }
    }
}
