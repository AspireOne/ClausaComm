
namespace ClausaComm.Forms
{
    partial class AddContactPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.IpLabel = new System.Windows.Forms.Label();
            this.IpBox = new ClausaComm.Components.RoundTextBox();
            this.NoteLabel = new System.Windows.Forms.Label();
            this.AddButton = new ClausaComm.Components.SimpleLineButton(this.components);
            this.TitleBar = new ClausaComm.Components.TitleBar(this.components);
            this.SuspendLayout();
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.HeaderLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.HeaderLabel.Location = new System.Drawing.Point(12, 37);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(493, 41);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "Add a Contact";
            this.HeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IpLabel
            // 
            this.IpLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IpLabel.AutoSize = true;
            this.IpLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IpLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.IpLabel.Location = new System.Drawing.Point(150, 126);
            this.IpLabel.Name = "IpLabel";
            this.IpLabel.Size = new System.Drawing.Size(38, 32);
            this.IpLabel.TabIndex = 3;
            this.IpLabel.Text = "IP:";
            // 
            // IpBox
            // 
            this.IpBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IpBox.BorderColor = System.Drawing.Color.Transparent;
            this.IpBox.BorderSize = 2;
            this.IpBox.ColorBorderOnFocus = true;
            this.IpBox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IpBox.ForeColor = System.Drawing.Color.Black;
            this.IpBox.Location = new System.Drawing.Point(194, 127);
            this.IpBox.MaxCharacters = 32767;
            this.IpBox.Multiline = false;
            this.IpBox.Name = "IpBox";
            this.IpBox.OnFocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(196)))), ((int)(((byte)(11)))));
            this.IpBox.ReadOnly = false;
            this.IpBox.Size = new System.Drawing.Size(149, 31);
            this.IpBox.TabIndex = 4;
            this.IpBox.TextboxRadius = 15;
            // 
            // NoteLabel
            // 
            this.NoteLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NoteLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoteLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.NoteLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.NoteLabel.Location = new System.Drawing.Point(75, 345);
            this.NoteLabel.MinimumSize = new System.Drawing.Size(371, 72);
            this.NoteLabel.Name = "NoteLabel";
            this.NoteLabel.Size = new System.Drawing.Size(371, 72);
            this.NoteLabel.TabIndex = 7;
            this.NoteLabel.Text = "Note: If the user is offline, their data (such as name or profile picture) will n" +
    "ot be available until they go online for the first time.";
            // 
            // AddButton
            // 
            this.AddButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AddButton.AutoSize = true;
            this.AddButton.ColorLineOnHover = true;
            this.AddButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddButton.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AddButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.AddButton.LineColor = System.Drawing.Color.White;
            this.AddButton.LineColorOnHover = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(196)))), ((int)(((byte)(11)))));
            this.AddButton.LineWidth = 3F;
            this.AddButton.Location = new System.Drawing.Point(215, 213);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(70, 40);
            this.AddButton.TabIndex = 8;
            this.AddButton.Text = "Add";
            this.AddButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TitleBar
            // 
            this.TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.TitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleBar.Form = null;
            this.TitleBar.Location = new System.Drawing.Point(0, 0);
            this.TitleBar.MaximumSize = new System.Drawing.Size(0, 25);
            this.TitleBar.MinimumSize = new System.Drawing.Size(0, 25);
            this.TitleBar.Name = "TitleBar";
            this.TitleBar.Size = new System.Drawing.Size(517, 25);
            this.TitleBar.TabIndex = 1;
            this.TitleBar.Title = "ClausaComm";
            // 
            // AddContactPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(517, 426);
            this.Controls.Add(this.TitleBar);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.NoteLabel);
            this.Controls.Add(this.IpBox);
            this.Controls.Add(this.IpLabel);
            this.Controls.Add(this.HeaderLabel);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(383, 426);
            this.Name = "AddContactPopup";
            this.Text = "Add Contact";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Label IpLabel;
        private System.Windows.Forms.Label NoteLabel;
        private Components.RoundTextBox IpBox;
        private Components.SimpleLineButton AddButton;
        private Components.TitleBar TitleBar;
    }
}