﻿
using System.Drawing;
using System.Windows.Forms;

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
            this.IpLabel = new System.Windows.Forms.Label();
            this.IpBox = new ClausaComm.Components.RoundTextBox();
            this.NoteLabel = new System.Windows.Forms.Label();
            this.AddButton = new ClausaComm.Components.SimpleLineButton(this.components);
            this.SuspendLayout();
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
            this.IpBox.ColorBorderOnHover = true;
            this.IpBox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IpBox.ForeColor = System.Drawing.Color.Black;
            this.IpBox.Location = new System.Drawing.Point(194, 127);
            this.IpBox.MaxCharacters = 32767;
            this.IpBox.Multiline = false;
            this.IpBox.Name = "IpBox";
            this.IpBox.OnHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(196)))), ((int)(((byte)(11)))));
            this.IpBox.ReadOnly = false;
            this.IpBox.Size = new System.Drawing.Size(149, 31);
            this.IpBox.TabIndex = 4;
            this.IpBox.TextboxRadius = 15;
            // 
            // NoteLabel
            // 
            this.NoteLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.NoteLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoteLabel.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NoteLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.NoteLabel.Location = new System.Drawing.Point(0, 308);
            this.NoteLabel.MinimumSize = new System.Drawing.Size(371, 72);
            this.NoteLabel.Name = "NoteLabel";
            this.NoteLabel.Size = new System.Drawing.Size(517, 77);
            this.NoteLabel.TabIndex = 0;
            this.NoteLabel.Text = "Write contact\'s IP.";
            this.NoteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddButton
            // 
            this.AddButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AddButton.ColorLineOnHover = true;
            this.AddButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddButton.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AddButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.AddButton.LineColor = System.Drawing.Color.White;
            this.AddButton.LineColorOnHover = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(196)))), ((int)(((byte)(11)))));
            this.AddButton.LineWidth = 3F;
            this.AddButton.Location = new System.Drawing.Point(223, 227);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(70, 40);
            this.AddButton.TabIndex = 8;
            this.AddButton.Text = "Add";
            this.AddButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AddContactPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(517, 385);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.NoteLabel);
            this.Controls.Add(this.IpBox);
            this.Controls.Add(this.IpLabel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "AddContactPopup";
            this.Text = "Add Contact";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        
        private System.Windows.Forms.Label IpLabel;
        private System.Windows.Forms.Label NoteLabel;
        private Components.RoundTextBox IpBox;
        private Components.SimpleLineButton AddButton;
    }
}