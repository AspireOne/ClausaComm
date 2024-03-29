﻿
namespace ClausaComm.Forms
{
    partial class EditInfoPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditInfoPopup));
            this.NameLbl = new System.Windows.Forms.Label();
            this.NameBox = new ClausaComm.Components.RoundTextBox();
            this.ProfilePictureLbl = new System.Windows.Forms.Label();
            this.IpLbl = new System.Windows.Forms.Label();
            this.IpBox = new ClausaComm.Components.RoundTextBox();
            this.ProfilePictureBox = new ClausaComm.Components.ContactData.ContactProfilePicture(this.components);
            this.SaveButton = new ClausaComm.Components.SimpleLineButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // NameLbl
            // 
            this.NameLbl.AutoSize = true;
            this.NameLbl.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NameLbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.NameLbl.Location = new System.Drawing.Point(24, 209);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(83, 32);
            this.NameLbl.TabIndex = 4;
            this.NameLbl.Text = "Name:";
            // 
            // NameBox
            // 
            this.NameBox.BorderColor = System.Drawing.Color.Transparent;
            this.NameBox.BorderSize = 2;
            this.NameBox.ColorBorderOnHover = true;
            this.NameBox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NameBox.ForeColor = System.Drawing.Color.Black;
            this.NameBox.Location = new System.Drawing.Point(113, 208);
            this.NameBox.MaxCharacters = 32767;
            this.NameBox.Multiline = false;
            this.NameBox.Name = "NameBox";
            this.NameBox.OnHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(196)))), ((int)(((byte)(11)))));
            this.NameBox.ReadOnly = false;
            this.NameBox.Size = new System.Drawing.Size(241, 33);
            this.NameBox.TabIndex = 5;
            this.NameBox.Text = "roundTextBox1";
            this.NameBox.TextboxRadius = 15;
            // 
            // ProfilePictureLbl
            // 
            this.ProfilePictureLbl.AutoSize = true;
            this.ProfilePictureLbl.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ProfilePictureLbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.ProfilePictureLbl.Location = new System.Drawing.Point(24, 278);
            this.ProfilePictureLbl.Name = "ProfilePictureLbl";
            this.ProfilePictureLbl.Size = new System.Drawing.Size(167, 32);
            this.ProfilePictureLbl.TabIndex = 6;
            this.ProfilePictureLbl.Text = "Profile Picture:";
            // 
            // IpLbl
            // 
            this.IpLbl.AutoSize = true;
            this.IpLbl.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IpLbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.IpLbl.Location = new System.Drawing.Point(24, 133);
            this.IpLbl.Name = "IpLbl";
            this.IpLbl.Size = new System.Drawing.Size(38, 32);
            this.IpLbl.TabIndex = 7;
            this.IpLbl.Text = "IP:";
            // 
            // IpBox
            // 
            this.IpBox.BorderColor = System.Drawing.Color.Transparent;
            this.IpBox.BorderSize = 2;
            this.IpBox.ColorBorderOnHover = true;
            this.IpBox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IpBox.ForeColor = System.Drawing.Color.Black;
            this.IpBox.Location = new System.Drawing.Point(68, 133);
            this.IpBox.MaxCharacters = 32767;
            this.IpBox.Multiline = false;
            this.IpBox.Name = "IpBox";
            this.IpBox.OnHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(196)))), ((int)(((byte)(11)))));
            this.IpBox.ReadOnly = false;
            this.IpBox.Size = new System.Drawing.Size(161, 33);
            this.IpBox.TabIndex = 8;
            this.IpBox.Text = "roundTextBox2";
            this.IpBox.TextboxRadius = 15;
            // 
            // ProfilePictureBox
            // 
            this.ProfilePictureBox.Contact = null;
            this.ProfilePictureBox.Image = null;
            this.ProfilePictureBox.Location = new System.Drawing.Point(197, 265);
            this.ProfilePictureBox.Name = "ProfilePictureBox";
            this.ProfilePictureBox.Padding = new System.Windows.Forms.Padding(2);
            this.ProfilePictureBox.Size = new System.Drawing.Size(70, 70);
            this.ProfilePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ProfilePictureBox.TabIndex = 9;
            this.ProfilePictureBox.TabStop = false;
            this.ProfilePictureBox.Click += new System.EventHandler(this.ProfilePictureBox_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.AutoSize = true;
            this.SaveButton.ColorLineOnHover = true;
            this.SaveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveButton.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.SaveButton.LineColor = System.Drawing.Color.White;
            this.SaveButton.LineColorOnHover = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(196)))), ((int)(((byte)(11)))));
            this.SaveButton.LineWidth = 3F;
            this.SaveButton.Location = new System.Drawing.Point(183, 438);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(103, 37);
            this.SaveButton.TabIndex = 12;
            this.SaveButton.Text = "Save it!";
            this.SaveButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // EditInfoPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 508);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ProfilePictureBox);
            this.Controls.Add(this.IpBox);
            this.Controls.Add(this.IpLbl);
            this.Controls.Add(this.ProfilePictureLbl);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.NameLbl);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "EditInfoPopup";
            this.Text = "Change your info";
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private System.Windows.Forms.Label NameLbl;
        private Components.RoundTextBox NameBox;
        private System.Windows.Forms.Label ProfilePictureLbl;
        private System.Windows.Forms.Label IpLbl;
        private Components.RoundTextBox IpBox;
        private Components.ContactData.ContactProfilePicture ProfilePictureBox;
        private Components.SimpleLineButton SaveButton;
    }
}