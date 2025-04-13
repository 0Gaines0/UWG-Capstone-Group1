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
            ((System.ComponentModel.ISupportInitialize)validationErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // login_button
            // 
            login_button.Location = new System.Drawing.Point(357, 207);
            login_button.Name = "login_button";
            login_button.Size = new System.Drawing.Size(75, 23);
            login_button.TabIndex = 0;
            login_button.Text = "Log In";
            login_button.UseVisualStyleBackColor = true;
            login_button.Click += login_button_Click;
            // 
            // userNameTextField
            // 
            userNameTextField.Location = new System.Drawing.Point(346, 94);
            userNameTextField.Name = "userNameTextField";
            userNameTextField.Size = new System.Drawing.Size(100, 23);
            userNameTextField.TabIndex = 1;
            // 
            // passwordTextField
            // 
            passwordTextField.Location = new System.Drawing.Point(346, 160);
            passwordTextField.Name = "passwordTextField";
            passwordTextField.Size = new System.Drawing.Size(100, 23);
            passwordTextField.TabIndex = 2;
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new System.Drawing.Point(346, 67);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new System.Drawing.Size(60, 15);
            usernameLabel.TabIndex = 3;
            usernameLabel.Text = "Username";
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new System.Drawing.Point(346, 133);
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
            // LoginPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
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
    }
}