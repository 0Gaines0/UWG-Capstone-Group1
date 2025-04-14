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

        public LoginPage(TicketSystemDbContext context)
        {
            InitializeComponent();
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context), "Input must not be null or empty");
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
                this.validationErrorProvider.SetError(usernameLabel, "Username cannot be empty.");
                invalidLogin = true;
            }
            else
            {
                this.validationErrorProvider.SetError(usernameLabel, string.Empty);
            }

            if (string.IsNullOrEmpty(password))
            {
                this.validationErrorProvider.SetError(passwordLabel, "Password cannot be empty.");
                invalidLogin = true;
            }
            else
            {
                this.validationErrorProvider.SetError(passwordLabel, string.Empty);
            }

            if (invalidLogin)
            {
                this.validationErrorProvider.SetError(login_button, string.Empty);
                return;
            }

            Employee user = this.context.Employees.FirstOrDefault(employee => employee.Username != null && employee.Username.Equals(username));
            var userPassword = user?.HashedPassword;
            if (user == null || userPassword == null || !this.verifyCorrectPassword(password, userPassword))
            {
                this.validationErrorProvider.SetError(login_button, "Incorrect username or password.");
                invalidLogin = true;
            }

            if (!invalidLogin)
            {
                ActiveEmployee.LogInEmployee(user);
                Form form = new ViewTasksPage(this.context);
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
