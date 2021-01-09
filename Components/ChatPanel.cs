using ClausaComm.Components.Icons;
using System.ComponentModel;
using System.Windows.Forms;

namespace ClausaComm.Components
{
    public partial class ChatPanel : Panel, IContactUsable
    {
        private SendIcon _sendIcon;
        private ChatTextBox _textbox;
        private readonly Label NoContactLabel = new()
        {
            Name = "NoContactLabel",
            Anchor = AnchorStyles.None,
            Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
            ForeColor = System.Drawing.Color.FromArgb(216, 216, 216),
            Location = new System.Drawing.Point(248, 293),
            Size = new System.Drawing.Size(399, 62),
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
