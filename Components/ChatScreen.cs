﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ClausaComm.Components.Icons;
using ClausaComm.Contacts;
using ClausaComm.Extensions;
using ClausaComm.Messages;
using ClausaComm.Utils;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace ClausaComm.Components
{
    public partial class ChatScreen : Panel
    {
        private SendIcon _sendIcon;
        private ChatTextBox _textbox;
        private const int InitialMessages = 15;
        //private const int MaxMessages = 15;
        private readonly Dictionary<Contact, MessageCollection> CachedChats = new();

        public delegate void SendPressedHandler(ChatMessage message, Contact contact);

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
            //ForeColor = Constants.UiConstants.ChatTextColor, // Not needed because it's defined in ChatMessagePanel.
            BackColor = Constants.UiConstants.ChatBackColor,
            Location = new Point(248, 293),
            Dock = DockStyle.Fill,
            HorizontalScroll = { Enabled = false, Visible = false, Maximum = 0 },
            VerticalScroll = {Enabled = true},
            AutoScroll = true,
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
        public Contact Contact { get; private set; }

        public ChatScreen()
        {
            InitializeComponent();
            NoContactLabel.Parent = this;
            ChatPanel.Parent = this;
        }

        private void AddMessage(Contact contact, ChatMessage message, bool loading = false, bool addToChat = true)
        {
            if (!CachedChats.TryGetValue(contact, out var messages))
                CachedChats.Add(contact, messages = new MessageCollection());
            
            messages.Add(message);

            if (ReferenceEquals(Contact, contact) && addToChat)
                AddMessageToChat();

            void AddMessageToChat()
            {
                var panel = new ChatMessagePanel(message, message.Way == ChatMessage.Ways.Out ? Contact.UserContact : contact);
                panel.Dock = DockStyle.Top;
                panel.Parent = ChatPanel;
                ChatPanel.Controls.Add(panel);
                if (loading || message.Way == ChatMessage.Ways.In) 
                    panel.MarkDelivered();

                if (!loading)
                {
                    ChatPanel.Controls.SetChildIndex(panel, 0);
                    ChatPanel.ScrollControlIntoView(panel);   
                }
            }
        }

        public void HandleMessageReceived(Contact contact, ChatMessage message) => AddMessage(contact, message);

        // This is to change the message's status from "sending" or "not sent" to "sent".
        public void HandleMessageDelivered(Contact contact, ChatMessage message)
        {
            // Iterates backwards, so that the first (latest) message is likely to be the one.
            for (int i = ChatPanel.Controls.Count - 1; i >= 0; --i)
            {
                if (ChatPanel.Controls[i] is not ChatMessagePanel panel || panel.Message != message)
                    continue;
                
                panel.MarkDelivered();
                break;
            }
        }
        
        public void SetContact(Contact contact)
        {
            if (ReferenceEquals(Contact, contact))
                return;
            
            ChangeContactSpecificElementsVisibility(contact is not null);
            NoContactLabel.Visible = contact is null;
            Contact = contact;

            if (ActionPanel is not null)
                ActionPanel.Contact = contact;

            // When the user switches to another contact and then back to this one check if it's cached and use that.
            // When a message is sent or received, add it to the array. If the array doesn't exist, don't do anything -
            //   it's not created yet because the user didn't open the chat with this contact yet - when 
            //   they will do, we will create the array from the messages saved in xml, which will include
            //   the new messages.

            // Remove messages from the previous contact.
            ChatPanel.Controls.Clear();
            
            if (contact?.Id is null)
                return;
            
            // The XML takes it from the oldest.
            MessageCollection messages;
            if (!CachedChats.TryGetValue(contact, out messages))
            {
                MessagesXml.GetMessages(contact.Id).Reverse().ForEach(message => AddMessage(contact, message, true, false));
                if (!CachedChats.TryGetValue(contact, out messages))
                    return;
            }

            messages.GetAmount(InitialMessages).ForEach(message => AddMessage(contact, message, true, true));

            if (ChatPanel.Controls.Count > 1)
                ChatPanel.ScrollControlIntoView(ChatPanel.Controls[0]);
        }

        private void HandleSendPressed()
        {
            if (Contact.Id is null)
            {
                Logger.Log("Send pressed but contact id is null");
                return;
            }

            string textTrimmed = Textbox.Text.Trim(' ', '\r', '\n');
            Textbox.Text = "";
            if (textTrimmed == "")
                return;
            
            ChatMessage msg = new(textTrimmed);
            AddMessage(Contact, msg);
            OnSendPressed?.Invoke(msg, Contact);
        }

        private void ChangeContactSpecificElementsVisibility(bool visible)
        {
            if (SendIcon is not null)
                SendIcon.Visible = visible;
            if (Textbox is not null)
                Textbox.Visible = visible;
        }

        public ChatScreen(IContainer container) : this() => container.Add(this);
    }
}