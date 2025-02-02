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

namespace ticket_system_winforms.View.UserCRUD
{
    public partial class UserRetrieveForm : Form
    {
        public UserRetrieveForm(User user)
        {
            InitializeComponent();

            this.UserIDTextBox.Text = user.UserID;
            this.UsernameTextBox.Text = user.Username;
            this.PasswordTextBox.Text = user.Password;
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
