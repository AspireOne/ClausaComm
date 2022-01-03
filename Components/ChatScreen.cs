using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Components.Icons;
using ClausaComm.Contacts;
using ClausaComm.Messages;

namespace ClausaComm.Components
{
    public partial class ChatScreen : Panel, IContactUsable
    {
        private SendIcon _sendIcon;
        private ChatTextBox _textbox;
        
        public delegate void SendPressedHandler(ChatMessage message);

        public event SendPressedHandler OnSendPressed;

        private readonly Label NoContactLabel = new()
        {
            Name = "NoContactLabel",
            Anchor = AnchorStyles.None,
            Font = new("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(216, 216, 216),
            Location = new(248, 293),
            Size = new(399, 62),
            TabIndex = 10,
            Text = "Oops, it seems like you don't have any contacts yet. Let's add some!",
            TextAlign = ContentAlignment.MiddleCenter,
        };

        private readonly Panel ChatPanel = new()
        {
            Name = "ChatPanel",
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right,
            ForeColor = Color.FromArgb(216, 216, 216),
            Location = new Point(248, 293),
            Dock = DockStyle.Fill,
            HorizontalScroll = { Enabled = false, Visible = false, Maximum = 0 },
            VerticalScroll = {Enabled = true},
            AutoScroll = true,
            BackColor = Color.FromArgb(116, 116, 116),
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
                    bool handleNextEnter = false;
                    Textbox.KeyDown += (_, e) =>
                    {
                        if (e.KeyCode == Keys.Enter && !e.Shift)
                        {
                            handleNextEnter = true;
                            HandleSendPressed();
                        }
                    };
                    Textbox.KeyPress += (_, e) =>
                    {
                        if (handleNextEnter && e.KeyChar == (char)Keys.Enter)
                        {
                            handleNextEnter = false;
                            e.Handled = true;
                        }
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

        public ChatScreen()
        {
            InitializeComponent();
            NoContactLabel.Parent = this;
            ChatPanel.Parent = this;

            for (int i = 0; i < 5; ++i)
            {
                var a = new ChatMessagePanel(new ChatMessage("tohle je nějakej text", ChatMessage.Ways.Out), Contact.UserContact);
                a.Dock = DockStyle.Top;
                a.Parent = ChatPanel;
                ChatPanel.Controls.Add(a);    
            }
            
            /*for (int i = 0; i < 15; ++i)
            {
                var textnox = new RoundTextBox();
                textnox.Parent = ChatPanel;
                textnox.Dock = DockStyle.Top;
                ChatPanel.Controls.Add(textnox);   
            }*/
        }
        
        private void HandleSendPressed()
        {
            if (Textbox.Text != "")
            {
                ChatMessage msg = new(Textbox.Text);
                OnSendPressed?.Invoke(msg);
            }
            Textbox.Text = "";
        }

        private void ChangeContactSpecificElementsVisibility(bool visible)
        {
            if (SendIcon is not null)
                SendIcon.Visible = visible;
            if (Textbox is not null)
                Textbox.Visible = visible;
        }

        public ChatScreen(Contact contact) : this() => Contact = contact;

        public ChatScreen(IContainer container) : this() => container.Add(this);
    }
}