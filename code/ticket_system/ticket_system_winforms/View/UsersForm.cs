using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_system_winforms.View.Dialogs;
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

            this.UsersListBox.DataBindings.Add(new Binding("SelectedIndex", this.viewmodel, "SelectedUserIndex"));
            this.RetrieveUserBtn.DataBindings.Add(new Binding("Enabled", this.viewmodel, "HasSelectedUser"));
            this.UpdateUserBtn.DataBindings.Add(new Binding("Enabled", this.viewmodel, "HasSelectedUser"));
            this.DeleteUserBtn.DataBindings.Add(new Binding("Enabled", this.viewmodel, "HasSelectedUser"));
            //TODO: make the above buttons usable

            //TODO find a better way to do this
            foreach (var currItem in this.viewmodel.Users) {
                this.UsersListBox.Items.Add(currItem);
            }
        }

        private void UsersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
             * Normal binding only takes effect when the listbox loses focus.
             * This "bumps" the property so you don't have to click another control to be able to use the CRUD buttons.
             */
            this.viewmodel.SelectedUserIndex = this.UsersListBox.SelectedIndex;
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

        private void PrivacyNavBtn_Click(object sender, EventArgs e)
        {
            Form form = new PrivacyForm();
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { this.Close(); };
            form.Show();
            this.Hide();
        }

        private void CreateUserBtn_Click(object sender, EventArgs e)
        {
            Form form = new UserCreateForm();
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { this.Close(); };
            form.Show();
            this.Hide();
        }

        private void RetrieveUserBtn_Click(object sender, EventArgs e)
        {
            Form form = new UserRetrieveForm(this.viewmodel.Users[this.viewmodel.SelectedUserIndex]);
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { this.Close(); };
            form.Show();
            this.Hide();
        }

        private void UpdateUserBtn_Click(object sender, EventArgs e)
        {
            Form form = new UserUpdateForm(this.viewmodel.Users[this.viewmodel.SelectedUserIndex]);
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { this.Close(); };
            form.Show();
            this.Hide();
        }

        private void DeleteUserBtn_Click(object sender, EventArgs e)
        {
            ConfirmDialog confirm = new ConfirmDialog("Confirm", "Are you sure you want to delete this user?");
            confirm.OnConfirm += new EventHandler(this.onDeleteConfirm);
            confirm.ShowDialog();

            //TODO remove this when proper binding between the listbox and the viewmodel property is implemented
            this.UsersListBox.Items.Clear();
            foreach (var currItem in this.viewmodel.Users)
            {
                this.UsersListBox.Items.Add(currItem);
            }
        }

        private void onDeleteConfirm(object sender, EventArgs e)
        {
            this.viewmodel.DeleteUser();
        }
    }
}