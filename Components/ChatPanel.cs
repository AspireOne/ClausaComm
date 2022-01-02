using System;
using ClausaComm.Components.Icons;
using System.ComponentModel;
using System.Windows.Forms;
using ClausaComm.Contacts;

namespace ClausaComm.Components
{
    public partial class ChatPanel : Panel, IContactUsable
    {
        private SendIcon _sendIcon;
        private ChatTextBox _textbox;
        
        public delegate void SendPressedHandler(string message);

        public SendPressedHandler OnSendPressed;

        private readonly Label NoContactLabel = new()
        {
            Name = "NoContactLabel",
            Anchor = AnchorStyles.None,
            Font = new("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
            ForeColor = System.Drawing.Color.FromArgb(216, 216, 216),
            Location = new(248, 293),
            Size = new(399, 62),
            TabIndex = 10,
            Text = "Oops, it seems like you don't have any contacts yet. Let's add some!",
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
        };

        public SendIcon SendIcon
        {
            private get => _sendIcon;
            set
            {
                _sendIcon = value;
                if (Contact is null)
                    ChangeContactSpecificElementsVisibility(false);
                
                if (value is not null)
                    SendIcon.Click += (_, _) => HandleSendPressed();
            }
        }

        public ChatTextBox Textbox
        {
            get => _textbox;
            set
            {
                _textbox = value;
                if (Contact is null)
                    ChangeContactSpecificElementsVisibility(false);

                if (value is not null)
                {
                    Textbox.KeyDown += (_, e) =>
                    {
                        if (e.KeyCode == Keys.Enter)
                            HandleSendPressed();
                    };   
                }
            }
        }

        public ActionPanel ActionPanel { get; set; }

        private Contact _contact;

        public Contact Contact
        {
            get => _contact;
            set
            {
                if (value is null)
                {
                    ChangeContactSpecificElementsVisibility(false);
                    NoContactLabel.Visible = true;
                }
                else
                {
                    NoContactLabel.Visible = false;

                    if (_contact is null)
                        ChangeContactSpecificElementsVisibility(true);
                }

                _contact = value;

                if (ActionPanel is not null)
                    ActionPanel.Contact = value;
            }
        }

        public ChatPanel()
        {
            InitializeComponent();
            NoContactLabel.Parent = this;
        }
        
        private void HandleSendPressed()
        {
            if (Textbox.Text != "")
                OnSendPressed?.Invoke(Textbox.Text);
            Textbox.Text = "";
        }

        private void ChangeContactSpecificElementsVisibility(bool visible)
        {
            if (SendIcon is not null)
                SendIcon.Visible = visible;
            if (Textbox is not null)
                Textbox.Visible = visible;
        }

        public ChatPanel(Contact contact) : this() => Contact = contact;

        public ChatPanel(IContainer container) : this() => container.Add(this);
    }
}