using System;
using System.Drawing;
using System.Windows.Forms;
using ClausaComm.Contacts;

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
            NameBox.Textbox.Text = User.Name;
            NameBox.Textbox.MaxLength = Contact.NameLength.max;
            NameBox.Textbox.TextChanged += (_, _) => OnNameBoxTextChange();

            IpBox.Textbox.Text = User.Ip;
            IpBox.Textbox.ReadOnly = true;

            ProfilePictureBox.Image = User.ProfilePic;
            ProfilePictureBox.Cursor = Cursors.Hand;

            SaveButton.Paint += (_, _) => OnSaveButtonPaint();
            SaveButton.MouseDown += (_, _) => OnSaveButtonClick();
            SaveButton.LineColorOnHover = Constants.UIConstants.ElementOnHoverColor;

            TitleBar.Form = this;
            TitleBar.Title = "Edit your info";
        }


        private void HeaderLabel_Click(object sender, EventArgs e)
        {

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
