﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ClausaComm.Components.ContactData;
using System.Windows.Forms;
using ClausaComm.Contacts;

namespace ClausaComm.Components
{
    public partial class PanelOfContactPanels : Panel
    {
        public IEnumerable<ContactPanel> Panels => Controls.Cast<ContactPanel>();

        public PanelOfContactPanels()
        {
            InitializeComponent();
        }

        public PanelOfContactPanels(IContainer container) : this() => container.Add(this);

        public void RemovePanel(Contact contact)
        {
            ContactPanel panel = Panels.First(contactPanel => contactPanel.Contact.Equals(contact));
            if (panel is not null)
                Controls.Remove(panel);
        }

        public void MovePanelToTop(Contact contact)
            => MovePanelToTop(Panels.First(panel => ReferenceEquals(panel.Contact, contact)));
        
        public void MovePanelToTop(ContactPanel panel) => Controls.SetChildIndex(panel, Controls.Count - 1);

        public void SelectFirstPanel()
        {
            if (Panels.Any())
                InvokeOnClick(Panels.Last(), EventArgs.Empty);
        }
    }
}