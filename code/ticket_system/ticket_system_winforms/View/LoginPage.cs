using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;

namespace ticket_system_winforms.View
{
    public partial class LoginPage : Form
    {
        private readonly TicketSystemDbContext context;
        private static string VALIDATE_PARAMETER_MESSAGE = "input must not be null or empty";
        private static string INVALID_ENTERED_CREDENTIALS = "credentials entered are invalid";

        public LoginPage(TicketSystemDbContext context)
        {
            InitializeComponent();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), VALIDATE_PARAMETER_MESSAGE);
            }
            this.context = context;
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            String username = userNameTextField.Text;
            String password = passwordTextField.Text;
            Boolean invalidLogin = false;

            if (string.IsNullOrEmpty(username))
            {
                this.errorProvider1.SetError(label1, "Username cannot be empty.");
                invalidLogin = true;
            }
            else
            {
                this.errorProvider1.SetError(label1, string.Empty);
            }

            if (string.IsNullOrEmpty(password))
            {
                this.errorProvider2.SetError(label2, "Password cannot be empty.");
                invalidLogin = true;
            }
            else
            {
                this.errorProvider2.SetError(label2, string.Empty);
            }
            if (invalidLogin)
            {
                this.errorProvider3.SetError(login_button, string.Empty);
                return;
            }

            Employee user = this.context.Employees.FirstOrDefault(employee => employee.Username != null && employee.Username.Equals(username));
            var userPassword = user?.HashedPassword;
            if (user == null || userPassword == null || !this.verifyCorrectPassword(password, userPassword))
            {
                this.errorProvider3.SetError(login_button, "Invalid Credentials.");
                invalidLogin = true;
            }

            if (!invalidLogin)
            {
                Form form = new HomeForm();
                form.Show();
                this.Hide();
            } else
            {
                return;
            }
        }

        private bool verifyCorrectPassword(string passwordEntered, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(passwordEntered, hashedPassword);
        }
    }
}
