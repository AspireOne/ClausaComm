using System.ComponentModel;

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
            this.ChatMessageHeader = new System.Windows.Forms.Panel();
            this.ChatMessageHeaderText = new System.Windows.Forms.Label();
            this.ChatMessageHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChatMessageHeader
            // 
            this.ChatMessageHeader.BackColor = System.Drawing.Color.Indigo;
            this.ChatMessageHeader.Controls.Add(this.ChatMessageHeaderText);
            this.ChatMessageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChatMessageHeader.Location = new System.Drawing.Point(0, 0);
            this.ChatMessageHeader.Name = "ChatMessageHeader";
            this.ChatMessageHeader.Size = new System.Drawing.Size(601, 36);
            this.ChatMessageHeader.TabIndex = 0;
            // 
            // ChatMessageHeaderText
            // 
            this.ChatMessageHeaderText.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChatMessageHeaderText.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ChatMessageHeaderText.Location = new System.Drawing.Point(0, 0);
            this.ChatMessageHeaderText.Name = "ChatMessageHeaderText";
            this.ChatMessageHeaderText.Size = new System.Drawing.Size(316, 36);
            this.ChatMessageHeaderText.TabIndex = 0;
            this.ChatMessageHeaderText.Text = "Matěj Pešl";
            this.ChatMessageHeaderText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChatMessagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.Controls.Add(this.ChatMessageHeader);
            this.Name = "ChatMessagePanel";
            this.Size = new System.Drawing.Size(601, 140);
            this.ChatMessageHeader.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label ChatMessageHeaderText;

        private System.Windows.Forms.Panel ChatMessageHeader;

        #endregion
    }
}