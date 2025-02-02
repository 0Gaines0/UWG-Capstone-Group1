using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_system_winforms.Model;
using ticket_system_winforms.View.Dialogs;
using ticket_system_winforms.ViewModel;

namespace ticket_system_winforms.View.UserCRUD
{
    public partial class UserUpdateForm : Form
    {
        private UserUpdateViewModel viewmodel;

        public UserUpdateForm(User user)
        {
            InitializeComponent();
            this.viewmodel = new UserUpdateViewModel(user);
            this.ErrMsgLabel.DataBindings.Add(new Binding("Text", this.viewmodel, "ErrMsg"));

            this.CurrentUserIDTextBox.DataBindings.Add(new Binding("Text", this.viewmodel, "CurrentUserID"));
            this.CurrentUsernameTextBox.DataBindings.Add(new Binding("Text", this.viewmodel, "CurrentUsername"));
            this.CurrentPasswordTextBox.DataBindings.Add(new Binding("Text", this.viewmodel, "CurrentPassword"));

            this.NewUserIDTextBox.DataBindings.Add(new Binding("Text", this.viewmodel, "NewUserID"));
            this.NewUsernameTextBox.DataBindings.Add(new Binding("Text", this.viewmodel, "NewUsername"));
            this.NewPasswordTextBox.DataBindings.Add(new Binding("Text", this.viewmodel, "NewPassword"));
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (this.viewmodel.Update())
            {
                Form alert = new AlertDialog("Success", "Updated user successfully.");
                alert.ShowDialog();

                Form form = new UsersForm();
                form.Location = this.Location;
                form.StartPosition = FormStartPosition.Manual;
                form.FormClosing += delegate { this.Close(); };
                form.Show();
                this.Hide();
            }
        }

        private void BackNavBtn_Click(object sender, EventArgs e)
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
