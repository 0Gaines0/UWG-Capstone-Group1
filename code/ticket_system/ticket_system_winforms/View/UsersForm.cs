using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_system_winforms.View.UserCRUD;
using ticket_system_winforms.ViewModel;

namespace ticket_system_winforms.View
{
    public partial class UsersForm : Form
    {
        private UsersViewModel viewmodel;

        public UsersForm()
        {
            InitializeComponent();
            this.viewmodel = new UsersViewModel();
            this.viewmodel.GetUsers();
            foreach (var currItem in this.viewmodel.Users) {
                this.UsersListBox.Items.Add(currItem);
            }
            //TODO figure out how to bind the currently selected user
        }

        private void HomeNavBtn_Click(object sender, EventArgs e)
        {
            var frm = new HomeForm();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Close(); };
            frm.Show();
            this.Hide();
        }

        private void PrivacyNavBtn_Click(object sender, EventArgs e)
        {
            var frm = new PrivacyForm();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Close(); };
            frm.Show();
            this.Hide();
        }

        private void CreateUserBtn_Click(object sender, EventArgs e)
        {
            var frm = new UserCreateForm();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Close(); };
            frm.Show();
            this.Hide();
        }

        private void UpdateUserBtn_Click(object sender, EventArgs e)
        {
            var frm = new UserUpdateForm(this.viewmodel.SelectedUser);
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Close(); };
            frm.Show();
            this.Hide();
        }

    }
}