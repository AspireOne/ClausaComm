
namespace ClausaComm
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
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.AddContactIcon = new ClausaComm.Components.AddCloseIcon(this.components);
            this.PanelOfContactPanels = new ClausaComm.Components.PanelOfContactPanels(this.components);
            this.ContactSearchBox = new ClausaComm.Components.RoundTextBox();
            this.OwnProfilePanel = new System.Windows.Forms.Panel();
            this.ChatPanel1 = new ClausaComm.Components.ChatPanel(this.components);
            this.ActionPanel1 = new ClausaComm.Components.ActionPanel(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.chatTextBox1 = new ClausaComm.Components.ChatTextBox();
            this.SendIcon1 = new ClausaComm.Components.Icons.SendIcon(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.IpLbl = new System.Windows.Forms.Label();
            this.Status = new ClausaComm.Components.ContactData.ContactStatus(this.components);
            this.NameLbl = new ClausaComm.Components.ContactData.ContactName(this.components);
            this.RemoveContactIcon = new ClausaComm.Components.AddCloseIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CallContactIcon = new ClausaComm.Components.PhoneIcon(this.components);
            this.ProfilePicture = new ClausaComm.Components.ContactProfilePicture(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.LeftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddContactIcon)).BeginInit();
            this.ChatPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SendIcon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoveContactIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CallContactIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // LeftPanel
            // 
            this.LeftPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(31)))));
            this.LeftPanel.Controls.Add(this.AddContactIcon);
            this.LeftPanel.Controls.Add(this.PanelOfContactPanels);
            this.LeftPanel.Controls.Add(this.ContactSearchBox);
            this.LeftPanel.Controls.Add(this.OwnProfilePanel);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(183, 707);
            this.LeftPanel.TabIndex = 24;
            // 
            // AddContactIcon
            // 
            this.AddContactIcon.CircleColor = System.Drawing.Color.White;
            this.AddContactIcon.ClickCircleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(79)))), ((int)(((byte)(25)))));
            this.AddContactIcon.ClickLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.AddContactIcon.ColorCircleOnClick = true;
            this.AddContactIcon.ColorCircleOnHover = true;
            this.AddContactIcon.ColorLinesOnClick = false;
            this.AddContactIcon.ColorLinesOnHover = false;
            this.AddContactIcon.CurrentIcon = ClausaComm.Components.AddCloseIcon.Icon.Plus;
            this.AddContactIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddContactIcon.HoverCircleColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(182)))), ((int)(((byte)(62)))));
            this.AddContactIcon.HoverLineColor = System.Drawing.Color.Gray;
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
            this.AddContactIcon.Click += new System.EventHandler(this.AddContactPictureBox_Click);
            // 
            // PanelOfContactPanels
            // 
            this.PanelOfContactPanels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelOfContactPanels.AutoScroll = true;
            this.PanelOfContactPanels.Location = new System.Drawing.Point(0, 126);
            this.PanelOfContactPanels.Name = "PanelOfContactPanels";
            this.PanelOfContactPanels.Size = new System.Drawing.Size(182, 582);
            this.PanelOfContactPanels.TabIndex = 7;
            // 
            // ContactSearchBox
            // 
            this.ContactSearchBox.BorderColor = System.Drawing.Color.Transparent;
            this.ContactSearchBox.BorderSize = 1;
            this.ContactSearchBox.ColorBorderOnFocus = true;
            this.ContactSearchBox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ContactSearchBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.ContactSearchBox.Location = new System.Drawing.Point(5, 76);
            this.ContactSearchBox.MaxCharacters = 32767;
            this.ContactSearchBox.Multiline = false;
            this.ContactSearchBox.Name = "ContactSearchBox";
            this.ContactSearchBox.OnFocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(196)))), ((int)(((byte)(11)))));
            this.ContactSearchBox.ReadOnly = false;
            this.ContactSearchBox.Size = new System.Drawing.Size(133, 33);
            this.ContactSearchBox.TabIndex = 5;
            this.ContactSearchBox.TextboxRadius = 15;
            this.ContactSearchBox.TextChanged += new System.EventHandler(this.ContactSearchBox_TextChanged);
            // 
            // OwnProfilePanel
            // 
            this.OwnProfilePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.OwnProfilePanel.Location = new System.Drawing.Point(0, 0);
            this.OwnProfilePanel.Name = "OwnProfilePanel";
            this.OwnProfilePanel.Size = new System.Drawing.Size(183, 66);
            this.OwnProfilePanel.TabIndex = 3;
            // 
            // ChatPanel1
            // 
            this.ChatPanel1.ActionPanel = null;
            this.ChatPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(21)))));
            this.ChatPanel1.Contact = null;
            this.ChatPanel1.Controls.Add(this.ActionPanel1);
            this.ChatPanel1.Controls.Add(this.panel1);
            this.ChatPanel1.Controls.Add(this.panel2);
            this.ChatPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChatPanel1.Location = new System.Drawing.Point(183, 0);
            this.ChatPanel1.Name = "ChatPanel1";
            this.ChatPanel1.Size = new System.Drawing.Size(884, 707);
            this.ChatPanel1.TabIndex = 26;
            this.ChatPanel1.Textbox = null;
            // 
            // ActionPanel1
            // 
            this.ActionPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(31)))));
            this.ActionPanel1.Contact = null;
            this.ActionPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ActionPanel1.Location = new System.Drawing.Point(0, 0);
            this.ActionPanel1.Name = "ActionPanel1";
            this.ActionPanel1.Size = new System.Drawing.Size(884, 66);
            this.ActionPanel1.TabIndex = 9;
            //this.ActionPanel1.Visible = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chatTextBox1);
            this.panel1.Controls.Add(this.SendIcon1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 647);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 60);
            this.panel1.TabIndex = 2;
            // 
            // chatTextBox1
            // 
            this.chatTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatTextBox1.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chatTextBox1.Location = new System.Drawing.Point(4, 5);
            this.chatTextBox1.MaxLength = 20000;
            this.chatTextBox1.Multiline = true;
            this.chatTextBox1.Name = "chatTextBox1";
            this.chatTextBox1.RectHeight = 15;
            this.chatTextBox1.RectLeft = 2;
            this.chatTextBox1.RectTop = 3;
            this.chatTextBox1.RectWidth = 15;
            this.chatTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.chatTextBox1.Size = new System.Drawing.Size(821, 50);
            this.chatTextBox1.TabIndex = 3;
            this.chatTextBox1.Visible = false;
            // 
            // SendIcon1
            // 
            this.SendIcon1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SendIcon1.ClickIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(79)))), ((int)(((byte)(25)))));
            this.SendIcon1.ColorIconOnClick = true;
            this.SendIcon1.ColorIconOnHover = true;
            this.SendIcon1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SendIcon1.HoverIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(182)))), ((int)(((byte)(62)))));
            this.SendIcon1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.SendIcon1.Image = ((System.Drawing.Image)(resources.GetObject("SendIcon1.Image")));
            this.SendIcon1.Location = new System.Drawing.Point(831, 13);
            this.SendIcon1.Name = "SendIcon1";
            this.SendIcon1.Size = new System.Drawing.Size(41, 35);
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
            // RemoveContactIcon
            // 
            this.RemoveContactIcon.CircleColor = System.Drawing.Color.White;
            this.RemoveContactIcon.ClickCircleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(79)))), ((int)(((byte)(25)))));
            this.RemoveContactIcon.ClickLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(79)))), ((int)(((byte)(25)))));
            this.RemoveContactIcon.ColorCircleOnClick = false;
            this.RemoveContactIcon.ColorCircleOnHover = false;
            this.RemoveContactIcon.ColorLinesOnClick = true;
            this.RemoveContactIcon.ColorLinesOnHover = false;
            this.RemoveContactIcon.ContextMenuStrip = this.contextMenuStrip1;
            this.RemoveContactIcon.CurrentIcon = ClausaComm.Components.AddCloseIcon.Icon.Cross;
            this.RemoveContactIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RemoveContactIcon.HoverCircleColor = System.Drawing.Color.White;
            this.RemoveContactIcon.HoverLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(117)))), ((int)(((byte)(252)))));
            this.RemoveContactIcon.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.RemoveContactIcon.LineWidth = 2F;
            this.RemoveContactIcon.Location = new System.Drawing.Point(6, 12);
            this.RemoveContactIcon.Name = "RemoveContactIcon";
            this.RemoveContactIcon.ShowCircle = false;
            this.RemoveContactIcon.Size = new System.Drawing.Size(43, 43);
            this.RemoveContactIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.RemoveContactIcon.TabIndex = 5;
            this.RemoveContactIcon.TabStop = false;
            this.toolTip1.SetToolTip(this.RemoveContactIcon, "Remove Contact");
            this.RemoveContactIcon.UnderlineOnHover = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // CallContactIcon
            // 
            this.CallContactIcon.ClickIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(79)))), ((int)(((byte)(25)))));
            this.CallContactIcon.ColorIconOnClick = true;
            this.CallContactIcon.ColorIconOnHover = false;
            this.CallContactIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CallContactIcon.HoverIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(117)))), ((int)(((byte)(252)))));
            this.CallContactIcon.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.CallContactIcon.Image = ((System.Drawing.Image)(resources.GetObject("CallContactIcon.Image")));
            this.CallContactIcon.Location = new System.Drawing.Point(65, 12);
            this.CallContactIcon.Name = "CallContactIcon";
            this.CallContactIcon.Padding = new System.Windows.Forms.Padding(6);
            this.CallContactIcon.Size = new System.Drawing.Size(43, 43);
            this.CallContactIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CallContactIcon.TabIndex = 4;
            this.CallContactIcon.TabStop = false;
            this.toolTip1.SetToolTip(this.CallContactIcon, "Call");
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(68)))), ((int)(((byte)(90)))));
            this.ClientSize = new System.Drawing.Size(1067, 707);
            this.Controls.Add(this.ChatPanel1);
            this.Controls.Add(this.LeftPanel);
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.IsMdiContainer = true;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MainForm";
            this.Text = "ClausaComm";
            this.LeftPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AddContactIcon)).EndInit();
            this.ChatPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SendIcon1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoveContactIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CallContactIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel LeftPanel;
        private Components.PanelOfContactPanels PanelOfContactPanels;
        private Components.RoundTextBox ContactSearchBox;
        private System.Windows.Forms.Panel OwnProfilePanel;
        private Components.ChatPanel ChatPanel1;
        private Components.ContactProfilePicture ProfilePicture;
        private Components.AddCloseIcon AddContactIcon;
        private Components.AddCloseIcon RemoveContactIcon;
        private Components.PhoneIcon CallContactIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolTip toolTip1;
        private Components.ContactData.ContactName NameLbl;
        private Components.ContactData.ContactStatus Status;
        private System.Windows.Forms.Panel panel1;
        private Components.Icons.SendIcon SendIcon1;
        private System.Windows.Forms.Label IpLbl;
        private System.Windows.Forms.Panel panel2;
        private Components.ChatTextBox chatTextBox1;
        private Components.ActionPanel ActionPanel1;
    }
}

