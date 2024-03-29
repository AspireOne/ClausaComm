﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Contacts;
using ClausaComm.Utils;

namespace ClausaComm.Forms
{
    public partial class EditInfoPopup : PopupBase
    {
        private readonly Contact User;
        private static readonly OpenFileDialog SelectImageDialog = new()
        {
            CheckFileExists = true,
            ValidateNames = true,
            Title = "Select Profile Picture",
            Multiselect = false,
            CheckPathExists = true,
            DereferenceLinks = true,
            Filter = "Image Files |*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.exif;*.tiff;*.svg;"
        };

        public EditInfoPopup(Contact user, MainForm containingForm) : base(containingForm, "Info")
        {
            User = user;
            InitializeComponent();
            InitializeComponentFurther();
            Init();
            
            foreach (Control control in Controls)
            {
                control.KeyPress += (_, e) =>
                {
                    if (e.KeyChar == (char)13)
                        OnSaveButtonClick();
                };
            }
        }

        private void InitializeComponentFurther()
        {
            NameBox.Textbox.Text = User.Name;
            NameBox.Textbox.MaxLength = Contact.NameLength.max;
            NameBox.Textbox.TextChanged += (_, _) => OnNameBoxTextChange();

            IpBox.Textbox.Text = User.Ip.ToString();
            IpBox.Textbox.ReadOnly = true;

            ProfilePictureBox.Image = User.ProfilePic;
            ProfilePictureBox.Cursor = Cursors.Hand;

            SaveButton.Paint += (_, _) => OnSaveButtonPaint();
            SaveButton.MouseDown += (_, _) => OnSaveButtonClick();
            SaveButton.LineColorOnHover = Constants.UiConstants.ElementOnHoverColor;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == (char)13)
                OnSaveButtonClick();
        }

        private void OnNameBoxTextChange()
        {
            NameBox.BorderColor = NameBox.Textbox.Text.Length < Contact.NameLength.min || NameBox.Textbox.Text.Length > Contact.NameLength.max ? Color.Red : Color.Transparent;
            SaveButton.Invalidate();
        }

        private void OnSaveButtonPaint()
        {
            SaveButton.Cursor = NameBox.BorderColor == Color.Red ? Cursors.No : Cursors.Hand;
        }

        private void OnSaveButtonClick()
        {
            if (SaveButton.Cursor == Cursors.Hand)
            {
                User.Name = NameBox.Textbox.Text;
                User.ProfilePic = ProfilePictureBox.Image;
                Close();
            }
        }

        private void ProfilePictureBox_Click(object sender, EventArgs e)
        {
            SelectImageDialog.ShowDialog();
            Image newImage = null;
            try
            {
                newImage = Image.FromFile(SelectImageDialog.FileName);
                /*int divisor = newImage.Width <= 300 ? 1 : newImage.Width / 300;
                int newWidth = newImage.Width / divisor;
                int newHeight = newImage.Height / divisor;
                newImage = ImageUtils.Resize(newImage, newWidth, newHeight);*/
            }
            catch (Exception ex) when (ex is System.IO.FileNotFoundException or ArgumentException or OutOfMemoryException)
            {
                Logger.Log(ex.ToString());
            }

            if (newImage is not null)
                ProfilePictureBox.Image = newImage;
        }
    }
}