namespace ticket_system_winforms.View
{
    partial class LoginPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            login_button = new System.Windows.Forms.Button();
            userNameTextField = new System.Windows.Forms.TextBox();
            passwordTextField = new System.Windows.Forms.TextBox();
            usernameLabel = new System.Windows.Forms.Label();
            passwordLabel = new System.Windows.Forms.Label();
            validationErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)validationErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // login_button
            // 
            login_button.BackColor = System.Drawing.Color.FromArgb(159, 122, 234);
            login_button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            login_button.ForeColor = System.Drawing.Color.White;
            login_button.Location = new System.Drawing.Point(122, 280);
            login_button.Name = "login_button";
            login_button.Padding = new System.Windows.Forms.Padding(3);
            login_button.Size = new System.Drawing.Size(100, 40);
            login_button.TabIndex = 0;
            login_button.Text = "Log In";
            login_button.UseVisualStyleBackColor = false;
            login_button.Click += login_button_Click;
            // 
            // userNameTextField
            // 
            userNameTextField.Location = new System.Drawing.Point(95, 172);
            userNameTextField.Name = "userNameTextField";
            userNameTextField.Size = new System.Drawing.Size(151, 23);
            userNameTextField.TabIndex = 1;
            // 
            // passwordTextField
            // 
            passwordTextField.Location = new System.Drawing.Point(95, 229);
            passwordTextField.Name = "passwordTextField";
            passwordTextField.PasswordChar = '*';
            passwordTextField.Size = new System.Drawing.Size(151, 23);
            passwordTextField.TabIndex = 2;
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.ForeColor = System.Drawing.Color.White;
            usernameLabel.Location = new System.Drawing.Point(95, 154);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new System.Drawing.Size(60, 15);
            usernameLabel.TabIndex = 3;
            usernameLabel.Text = "Username";
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.ForeColor = System.Drawing.Color.White;
            passwordLabel.Location = new System.Drawing.Point(95, 211);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(57, 15);
            passwordLabel.TabIndex = 4;
            passwordLabel.Text = "Password";
            // 
            // validationErrorProvider
            // 
            validationErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            validationErrorProvider.ContainerControl = this;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Black;
            label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(28, 41);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(106, 47);
            label1.TabIndex = 5;
            label1.Text = "Login";
            // 
            // LoginPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(41, 84, 61);
            ClientSize = new System.Drawing.Size(334, 461);
            Controls.Add(label1);
            Controls.Add(passwordLabel);
            Controls.Add(usernameLabel);
            Controls.Add(passwordTextField);
            Controls.Add(userNameTextField);
            Controls.Add(login_button);
            Name = "LoginPage";
            Text = "LoginPage";
            ((System.ComponentModel.ISupportInitialize)validationErrorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button login_button;
        private System.Windows.Forms.TextBox userNameTextField;
        private System.Windows.Forms.TextBox passwordTextField;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.ErrorProvider validationErrorProvider;
        private System.Windows.Forms.Label label1;
    }
}