﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausaComm.Components
{
    public partial class ContactProfilePicture : PictureBox, IContactUsable
    {
        private Contact _contact;
        public Contact Contact
        {
            get => _contact;
            set
            {
                // If the previous contact is not null, remove the profile change handler for it, so that when
                // it's profile picture changes, it doesn't show it (it manages a different contact now).
                if (_contact is not null)
                    _contact.ProfilePicChange -= ProfilePicChangeHandler;

                if ((_contact = value) is not null)
                {
                    value.ProfilePicChange += ProfilePicChangeHandler;
                    ProfilePicChangeHandler(null, value.ProfilePic);
                }
            }
        }

        public new Image Image
        {
            get => base.Image;
            set => base.Image = ImageUtils.ClipToCircle(value);
        }

        public ContactProfilePicture()
        {
            InitializeComponent();
            Name = "ProfilePicture";
            this.Size = new Size(Height, Height);
            Padding = new Padding(2, 2, 2, 2);
            SizeMode = PictureBoxSizeMode.StretchImage;
            BorderStyle = BorderStyle.None;
        }

        private void ProfilePicChangeHandler(object _, Image img)
        {
            Image = img;
            Invalidate();
        }

        public ContactProfilePicture(Contact contact) : this() => Contact = contact;
        public ContactProfilePicture(IContainer container) : this() => container.Add(this);
    }
}
