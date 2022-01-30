
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Messages;

namespace ClausaComm.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.IpLbl = new System.Windows.Forms.Label();
            this.Status = new ClausaComm.Components.ContactData.ContactStatus(this.components);
            this.NameLbl = new ClausaComm.Components.ContactData.ContactName(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CallContactIcon = new ClausaComm.Components.Icons.PhoneIcon(this.components);
            this.ProfilePicture = new ClausaComm.Components.ContactData.ContactProfilePicture(this.components);
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ChatScreen = new ClausaComm.Components.ChatScreen(this.components);
            this.NotificationPanel = new ClausaComm.Components.NotificationPanel(this.components);
            this.ActionPanel1 = new ClausaComm.Components.ActionPanel(this.components);
            this.ChatTextBoxContainer = new System.Windows.Forms.Panel();
            this.ChatTextBox1 = new ClausaComm.Components.ChatTextBox();
            this.SendIcon1 = new ClausaComm.Components.Icons.SendIcon(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.AddContactIcon = new ClausaComm.Components.Icons.PlusIcon(this.components);
            this.PanelOfContactPanels = new ClausaComm.Components.PanelOfContactPanels(this.components);
            this.ContactSearchBox = new ClausaComm.Components.RoundTextBox();
            this.OwnProfilePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CallContactIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePicture)).BeginInit();
            this.ChatScreen.SuspendLayout();
            this.ChatTextBoxContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SendIcon1)).BeginInit();
            this.LeftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddContactIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // IpLbl
            // 
            this.IpLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.IpLbl.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IpLbl.Location = new System.Drawing.Point(355, 1);
            this.IpLbl.Name = "IpLbl";
            this.IpLbl.Size = new System.Drawing.Size(148, 65);
            this.IpLbl.TabIndex = 9;
            this.IpLbl.Text = "192.168.125.151";
            this.IpLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Status
            // 
            this.Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Status.BackColor = System.Drawing.Color.Transparent;
            this.Status.Contact = null;
            this.Status.Location = new System.Drawing.Point(855, 29);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(13, 13);
            this.Status.TabIndex = 7;
            this.Status.TabStop = false;
            // 
            // NameLbl
            // 
            this.NameLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NameLbl.Contact = null;
            this.NameLbl.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NameLbl.Location = new System.Drawing.Point(543, 1);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(226, 64);
            this.NameLbl.TabIndex = 6;
            this.NameLbl.Text = "Samalama dumaluma gertruda";
            this.NameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // CallContactIcon
            // 
            this.CallContactIcon.ClickIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            this.CallContactIcon.ColorBoxOnHover = false;
            this.CallContactIcon.ColorIconOnClick = true;
            this.CallContactIcon.ColorIconOnHover = false;
            this.CallContactIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CallContactIcon.HoverIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(172)))), ((int)(((byte)(10)))));
            this.CallContactIcon.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.CallContactIcon.Image = ((System.Drawing.Image)(resources.GetObject("CallContactIcon.Image")));
            this.CallContactIcon.Location = new System.Drawing.Point(65, 12);
            this.CallContactIcon.Name = "CallContactIcon";
            this.CallContactIcon.Padding = new System.Windows.Forms.Padding(6);
            this.CallContactIcon.Size = new System.Drawing.Size(43, 43);
            this.CallContactIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CallContactIcon.TabIndex = 4;
            this.CallContactIcon.TabStop = false;
            this.CallContactIcon.UnderlineOnHover = true;
            // 
            // ProfilePicture
            // 
            this.ProfilePicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ProfilePicture.Contact = null;
            this.ProfilePicture.Image = null;
            this.ProfilePicture.Location = new System.Drawing.Point(775, 1);
            this.ProfilePicture.Name = "ProfilePicture";
            this.ProfilePicture.Padding = new System.Windows.Forms.Padding(2);
            this.ProfilePicture.Size = new System.Drawing.Size(64, 64);
            this.ProfilePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ProfilePicture.TabIndex = 0;
            this.ProfilePicture.TabStop = false;
            // 
            // ChatScreen
            // 
            this.ChatScreen.ActionPanel = null;
            this.ChatScreen.Controls.Add(this.NotificationPanel);
            this.ChatScreen.Controls.Add(this.ActionPanel1);
            this.ChatScreen.Controls.Add(this.ChatTextBoxContainer);
            this.ChatScreen.Controls.Add(this.panel2);
            this.ChatScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChatScreen.Location = new System.Drawing.Point(184, 26);
            this.ChatScreen.Name = "ChatScreen";
            this.ChatScreen.Size = new System.Drawing.Size(882, 680);
            this.ChatScreen.TabIndex = 28;
            this.ChatScreen.Textbox = null;
            this.ChatScreen.Paint += new System.Windows.Forms.PaintEventHandler(this.ChatPanel1_Paint);
            // 
            // NotificationPanel
            // 
            this.NotificationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NotificationPanel.AutoSize = true;
            this.NotificationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(33)))));
            this.NotificationPanel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NotificationPanel.Form = null;
            this.NotificationPanel.Location = new System.Drawing.Point(609, 504);
            this.NotificationPanel.Name = "NotificationPanel";
            this.NotificationPanel.Padding = new System.Windows.Forms.Padding(2);
            this.NotificationPanel.Size = new System.Drawing.Size(270, 90);
            this.NotificationPanel.TabIndex = 11;
            this.NotificationPanel.Visible = false;
            // 
            // ActionPanel1
            // 
            this.ActionPanel1.BackColor = Constants.UiConstants.UiColor;
            this.ActionPanel1.Contact = null;
            this.ActionPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ActionPanel1.Location = new System.Drawing.Point(0, 0);
            this.ActionPanel1.MainForm = null;
            this.ActionPanel1.Name = "ActionPanel1";
            this.ActionPanel1.Size = new System.Drawing.Size(882, 66);
            this.ActionPanel1.TabIndex = 9;
            // 
            // ChatTextBoxContainer
            // 
            this.ChatTextBoxContainer.Controls.Add(this.ChatTextBox1);
            this.ChatTextBoxContainer.Controls.Add(this.SendIcon1);
            this.ChatTextBoxContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ChatTextBoxContainer.Location = new System.Drawing.Point(0, 609);
            this.ChatTextBoxContainer.Name = "ChatTextBoxContainer";
            this.ChatTextBoxContainer.Size = new System.Drawing.Size(882, 75);
            this.ChatTextBoxContainer.TabIndex = 2;
            this.ChatTextBoxContainer.BackColor = Constants.UiConstants.ChatColor;
            // 
            // chatTextBox1
            // 
            this.ChatTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ChatTextBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ChatTextBox1.MaxLength = ChatMessage.MaxTextLength;
            this.ChatTextBox1.Multiline = true;
            this.ChatTextBox1.Name = "ChatTextBox1";
            this.ChatTextBox1.PlaceholderText = " Write a message...";
            this.ChatTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChatTextBox1.Location = new System.Drawing.Point(4, 1);
            this.ChatTextBox1.Size = new System.Drawing.Size(820, 70);
            this.ChatTextBox1.Padding = new Padding(3);
            this.ChatTextBox1.TabIndex = 3;
            this.ChatTextBox1.Visible = false;
            this.ChatTextBox1.BackColor = Constants.UiConstants.ChatTextBoxColor;
            // 
            // SendIcon1
            // 
            this.SendIcon1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SendIcon1.ClickIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            this.SendIcon1.ColorBoxOnHover = false;
            this.SendIcon1.ColorIconOnClick = true;
            this.SendIcon1.ColorIconOnHover = true;
            this.SendIcon1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SendIcon1.HoverIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(172)))), ((int)(((byte)(10)))));
            this.SendIcon1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.SendIcon1.Image = ((System.Drawing.Image)(resources.GetObject("SendIcon1.Image")));
            this.SendIcon1.Location = new System.Drawing.Point(830, 19);
            this.SendIcon1.Name = "SendIcon1";
            this.SendIcon1.Padding = new System.Windows.Forms.Padding(3);
            this.SendIcon1.Size = new System.Drawing.Size(41, 37);
            this.SendIcon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SendIcon1.TabIndex = 2;
            this.SendIcon1.TabStop = false;
            this.SendIcon1.UnderlineOnHover = false;
            this.SendIcon1.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(871, 76);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1, 50);
            this.panel2.TabIndex = 8;
            this.panel2.Visible = false;
            // 
            // LeftPanel
            // 
            this.LeftPanel.BackColor = Constants.UiConstants.UiColor;
            this.LeftPanel.Controls.Add(this.AddContactIcon);
            this.LeftPanel.Controls.Add(this.PanelOfContactPanels);
            this.LeftPanel.Controls.Add(this.ContactSearchBox);
            this.LeftPanel.Controls.Add(this.OwnProfilePanel);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(1, 26);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(183, 680);
            this.LeftPanel.TabIndex = 27;
            // 
            // AddContactIcon
            // 
            this.AddContactIcon.CircleColor = System.Drawing.Color.White;
            this.AddContactIcon.ClickCircleColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            this.AddContactIcon.ClickLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.AddContactIcon.ColorBoxOnHover = false;
            this.AddContactIcon.ColorCircleOnClick = true;
            this.AddContactIcon.ColorCircleOnHover = true;
            this.AddContactIcon.ColorIconOnClick = false;
            this.AddContactIcon.ColorIconOnHover = false;
            this.AddContactIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddContactIcon.HoverCircleColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(172)))), ((int)(((byte)(10)))));
            this.AddContactIcon.HoverLineColor = System.Drawing.Color.Gray;
            this.AddContactIcon.IconPaddingFactor = 3.4F;
            this.AddContactIcon.Image = null;
            this.AddContactIcon.LineColor = System.Drawing.Color.Gray;
            this.AddContactIcon.LineWidth = 2F;
            this.AddContactIcon.Location = new System.Drawing.Point(144, 76);
            this.AddContactIcon.Name = "AddContactIcon";
            this.AddContactIcon.ShowCircle = true;
            this.AddContactIcon.Size = new System.Drawing.Size(32, 33);
            this.AddContactIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AddContactIcon.TabIndex = 8;
            this.AddContactIcon.TabStop = false;
            this.AddContactIcon.UnderlineOnHover = false;
            // 
            // PanelOfContactPanels
            // 
            this.PanelOfContactPanels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelOfContactPanels.AutoScroll = true;
            this.PanelOfContactPanels.Location = new System.Drawing.Point(0, 125);
            this.PanelOfContactPanels.Name = "PanelOfContactPanels";
            this.PanelOfContactPanels.Size = new System.Drawing.Size(183, 555);
            this.PanelOfContactPanels.TabIndex = 7;
            // 
            // ContactSearchBox
            // 
            this.ContactSearchBox.BorderColor = System.Drawing.Color.Transparent;
            this.ContactSearchBox.BorderSize = 1;
            this.ContactSearchBox.ColorBorderOnHover = true;
            this.ContactSearchBox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ContactSearchBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.ContactSearchBox.Location = new System.Drawing.Point(5, 76);
            this.ContactSearchBox.MaxCharacters = 32767;
            this.ContactSearchBox.Multiline = false;
            this.ContactSearchBox.Name = "ContactSearchBox";
            this.ContactSearchBox.OnHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(196)))), ((int)(((byte)(11)))));
            this.ContactSearchBox.ReadOnly = false;
            this.ContactSearchBox.Size = new System.Drawing.Size(133, 33);
            this.ContactSearchBox.TabIndex = 5;
            this.ContactSearchBox.TextboxRadius = 15;
            // 
            // OwnProfilePanel
            // 
            this.OwnProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.OwnProfilePanel.Location = new System.Drawing.Point(0, 0);
            this.OwnProfilePanel.Name = "OwnProfilePanel";
            this.OwnProfilePanel.Size = new System.Drawing.Size(183, 66);
            this.OwnProfilePanel.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(68)))), ((int)(((byte)(90)))));
            this.ClientSize = new System.Drawing.Size(1067, 707);
            this.Controls.Add(this.ChatScreen);
            this.Controls.Add(this.LeftPanel);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Text = "ClausaComm";
            ((System.ComponentModel.ISupportInitialize)(this.Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CallContactIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePicture)).EndInit();
            this.ChatScreen.ResumeLayout(false);
            this.ChatScreen.PerformLayout();
            this.ChatTextBoxContainer.ResumeLayout(false);
            this.ChatTextBoxContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SendIcon1)).EndInit();
            this.LeftPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AddContactIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Components.ContactData.ContactProfilePicture ProfilePicture;
        private Components.Icons.PhoneIcon CallContactIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.ToolTip ToolTip1;
        private Components.ContactData.ContactName NameLbl;
        private Components.ContactData.ContactStatus Status;
        private System.Windows.Forms.Label IpLbl;
        private Components.ChatScreen ChatScreen;
        private Components.ActionPanel ActionPanel1;
        private System.Windows.Forms.Panel ChatTextBoxContainer;
        private Components.ChatTextBox ChatTextBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel LeftPanel;
        private Components.Icons.PlusIcon AddContactIcon;
        private Components.PanelOfContactPanels PanelOfContactPanels;
        private Components.RoundTextBox ContactSearchBox;
        private System.Windows.Forms.Panel OwnProfilePanel;
        private Components.Icons.SendIcon SendIcon1;
        public Components.NotificationPanel NotificationPanel;
    }
}

