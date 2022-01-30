using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ClausaComm.Components
{
    partial class ChatMessagePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ChatMessageHeader = new System.Windows.Forms.Panel();
            this.ChatMessageTime = new System.Windows.Forms.Label();
            this.ChatMessageName = new System.Windows.Forms.Label();
            this.ChatMessagePicture = new System.Windows.Forms.PictureBox();
            this.ChatMessageText = new Components.SelectableTextBox(this.components);
            this.ChatMessageHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChatMessagePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // ChatMessageHeader
            // 
            this.ChatMessageHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatMessageHeader.Controls.Add(this.ChatMessageTime);
            this.ChatMessageHeader.Controls.Add(this.ChatMessageName);
            this.ChatMessageHeader.Location = new System.Drawing.Point(51, 0);
            this.ChatMessageHeader.Name = "ChatMessageHeader";
            this.ChatMessageHeader.Size = new System.Drawing.Size(550, 24);
            this.ChatMessageHeader.TabIndex = 0;
            // 
            // ChatMessageTime
            // 
            this.ChatMessageTime.AutoSize = true;
            this.ChatMessageTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChatMessageTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatMessageTime.ForeColor = Color.FromArgb(110, 110, 120);
            this.ChatMessageTime.Location = new System.Drawing.Point(84, 0);
            this.ChatMessageTime.Name = "ChatMessageTime";
            this.ChatMessageTime.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.ChatMessageTime.Size = new System.Drawing.Size(94, 19);
            this.ChatMessageTime.TabIndex = 1;
            this.ChatMessageTime.Text = "------------";
            this.ChatMessageTime.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ChatMessageName
            // 
            this.ChatMessageName.AutoSize = true;
            this.ChatMessageName.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChatMessageName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatMessageName.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ChatMessageName.Location = new System.Drawing.Point(0, 0);
            this.ChatMessageName.Name = "ChatMessageName";
            this.ChatMessageName.Size = new System.Drawing.Size(84, 21);
            this.ChatMessageName.TabIndex = 0;
            this.ChatMessageName.Text = "-------";
            this.ChatMessageName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChatMessagePicture
            // 
            this.ChatMessagePicture.Location = new System.Drawing.Point(0, 0);
            this.ChatMessagePicture.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ChatMessagePicture.Name = "ChatMessagePicture";
            this.ChatMessagePicture.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.ChatMessagePicture.Size = new System.Drawing.Size(53, 41 - 8);
            this.ChatMessagePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ChatMessagePicture.TabIndex = 1;
            this.ChatMessagePicture.TabStop = false;
            // 
            // ChatMessageText
            // 
            this.ChatMessageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatMessageText.BackColor = System.Drawing.Color.Black;
            this.ChatMessageText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ChatMessageText.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ChatMessageText.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatMessageText.ForeColor =Constants.UiConstants.ChatTextColor;
            this.ChatMessageText.Location = new System.Drawing.Point(51, 20);
            this.ChatMessageText.Multiline = true;
            this.ChatMessageText.Name = "ChatMessageText";
            this.ChatMessageText.ReadOnly = true;
            this.ChatMessageText.Size = new System.Drawing.Size(547, 21);
            this.ChatMessageText.TabIndex = 0;
            this.ChatMessageText.Text = "";
            // 
            // ChatMessagePanel
            // 
            this.Padding = new Padding(0, 20, 0, 20);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.ChatMessageText);
            this.Controls.Add(this.ChatMessagePicture);
            this.Controls.Add(this.ChatMessageHeader);
            this.Name = "ChatMessagePanel";
            this.Size = new System.Drawing.Size(650, 53);
            this.ChatMessageHeader.ResumeLayout(false);
            this.ChatMessageHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChatMessagePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Components.SelectableTextBox ChatMessageText;

        private System.Windows.Forms.Label ChatMessageTime;

        private System.Windows.Forms.PictureBox ChatMessagePicture;

        private System.Windows.Forms.Label ChatMessageName;

        private System.Windows.Forms.Panel ChatMessageHeader;

        #endregion
    }
}