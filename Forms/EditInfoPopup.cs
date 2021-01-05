using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausaComm.Forms
{
    public partial class EditInfoPopup : FormBase
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
            Filter = "Image Files |*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.exif;*.tiff;*.svg;" // TODO: test svg & tiff - may not work
        };

        public EditInfoPopup(Contact user)
        {
            User = user;

            InitializeComponent();
            InitializeComponentFurther();
        }

        private void InitializeComponentFurther()
        {
            NameBox.textbox.Text = User.Name;
            NameBox.textbox.MaxLength = Contact.MaxNameLength;
            NameBox.textbox.TextChanged += (object _, EventArgs _) => OnNameBoxTextChange();

            IpBox.textbox.Text = User.Ip;
            IpBox.textbox.ReadOnly = true;

            ProfilePictureBox.Image = User.ProfilePic;
            ProfilePictureBox.Cursor = Cursors.Hand;

            SaveButton.Paint += (object _, PaintEventArgs _) => OnSaveButtonPaint();
            SaveButton.MouseDown += (object _, MouseEventArgs _) => OnSaveButtonClick();
            SaveButton.LineColorOnHover = Constants.UIConstants.ElementOnHover.Color;

            TitleBar.Form = this;
            TitleBar.Title = "Edit your info";
        }


        private void HeaderLabel_Click(object sender, EventArgs e)
        {

        }

        private void OnNameBoxTextChange()
        {
            if (NameBox.textbox.Text.Length is < Contact.MinNameLength or > Contact.MaxNameLength)
                NameBox.BorderColor = Color.Red;
            else
                NameBox.BorderColor = Color.Transparent;
            SaveButton.Invalidate();
        }

        private void OnSaveButtonPaint()
        {
            if (NameBox.BorderColor == Color.Red)
                SaveButton.Cursor = Cursors.No;
            else
                SaveButton.Cursor = Cursors.Hand;
        }

        private void OnSaveButtonClick()
        {
            if (SaveButton.Cursor == Cursors.Hand)
            {
                User.Name = NameBox.textbox.Text;
                User.ProfilePic = ProfilePictureBox.Image;
                this.Close();
            }
        }

        private void ProfilePictureBox_Click(object sender, EventArgs e)
        {
            SelectImageDialog.ShowDialog();
            Image newImage = null;
            try
            {
                newImage = Image.FromFile(SelectImageDialog.FileName);
            }
            catch (Exception ex) when (ex is System.IO.FileNotFoundException or ArgumentException or OutOfMemoryException)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            if (newImage is not null)
                ProfilePictureBox.Image = newImage;
        }
    }
}
