﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ticket_system_winforms.View
{
    public partial class PrivacyForm : Form
    {
        public PrivacyForm()
        {
            InitializeComponent();
        }

        private void HomeNavBtn_Click(object sender, EventArgs e)
        {
            Form form = new HomeForm();
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { this.Close(); };
            form.Show();
            this.Hide();
        }

        private void UsersNavBtn_Click(object sender, EventArgs e)
        {
            Form form = new UsersForm();
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { this.Close(); };
            form.Show();
            this.Hide();
        }
    }
}
