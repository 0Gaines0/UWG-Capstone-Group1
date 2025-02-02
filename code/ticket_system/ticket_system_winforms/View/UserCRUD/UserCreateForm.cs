using System;
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
            var frm = new HomeForm();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Close(); };
            frm.Show();
            this.Hide();
        }

        private void BackNavBtn_Click(object sender, EventArgs e)
        {
            var frm = new UsersForm();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Close(); };
            frm.Show();
            this.Hide();
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            this.viewmodel.Create();
        }
    }
}
