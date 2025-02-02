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

namespace ticket_system_winforms.View
{
    public partial class UserCreateForm : Form
    {
        private UserCreateViewModel viewmodel;

        public UserCreateForm()
        {
            InitializeComponent();
            this.viewmodel = new UserCreateViewModel();
            this.UserIDTextBox.DataBindings.Add(new Binding("Text", this.viewmodel, "UserID"));
            this.UsernameTextBox.DataBindings.Add(new Binding("Text", this.viewmodel, "Username"));
            this.PasswordTextBox.DataBindings.Add(new Binding("Text", this.viewmodel, "Password"));
            this.ErrMsgLabel.DataBindings.Add(new Binding("Text", this.viewmodel, "ErrMsg"));
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            Form form = new HomeForm();
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { this.Close(); };
            form.Show();
            this.Hide();
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

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            if (this.viewmodel.Create())
            {
                Form alert = new AlertDialog("Success", "Successfully created new user.");
                alert.ShowDialog();
                
                Form form = new UsersForm();
                form.Location = this.Location;
                form.StartPosition = FormStartPosition.Manual;
                form.FormClosing += delegate { this.Close(); };
                form.Show();
                this.Hide();
            }
        }
    }
}
