namespace ticket_system_winforms.View.UserCRUD
{
    partial class UserUpdateForm
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
            this.BackNavBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentPasswordTextBox = new System.Windows.Forms.TextBox();
            this.CurrentUserIDTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CurrentUsernameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.NewPasswordTextBox = new System.Windows.Forms.TextBox();
            this.NewUserIDTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.NewUsernameTextBox = new System.Windows.Forms.TextBox();
            this.UpdateBtn = new System.Windows.Forms.Button();
            this.ErrMsgLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BackNavBtn
            // 
            this.BackNavBtn.Location = new System.Drawing.Point(12, 12);
            this.BackNavBtn.Name = "BackNavBtn";
            this.BackNavBtn.Size = new System.Drawing.Size(75, 23);
            this.BackNavBtn.TabIndex = 1;
            this.BackNavBtn.Text = "Back";
            this.BackNavBtn.UseVisualStyleBackColor = true;
            this.BackNavBtn.Click += new System.EventHandler(this.BackNavBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CurrentPasswordTextBox);
            this.groupBox1.Controls.Add(this.CurrentUserIDTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CurrentUsernameTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current info";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "UserID";
            // 
            // CurrentPasswordTextBox
            // 
            this.CurrentPasswordTextBox.Location = new System.Drawing.Point(81, 71);
            this.CurrentPasswordTextBox.Name = "CurrentPasswordTextBox";
            this.CurrentPasswordTextBox.ReadOnly = true;
            this.CurrentPasswordTextBox.Size = new System.Drawing.Size(100, 20);
            this.CurrentPasswordTextBox.TabIndex = 11;
            // 
            // CurrentUserIDTextBox
            // 
            this.CurrentUserIDTextBox.Location = new System.Drawing.Point(81, 19);
            this.CurrentUserIDTextBox.Name = "CurrentUserIDTextBox";
            this.CurrentUserIDTextBox.ReadOnly = true;
            this.CurrentUserIDTextBox.Size = new System.Drawing.Size(100, 20);
            this.CurrentUserIDTextBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Username";
            // 
            // CurrentUsernameTextBox
            // 
            this.CurrentUsernameTextBox.Location = new System.Drawing.Point(81, 45);
            this.CurrentUsernameTextBox.Name = "CurrentUsernameTextBox";
            this.CurrentUsernameTextBox.ReadOnly = true;
            this.CurrentUsernameTextBox.Size = new System.Drawing.Size(100, 20);
            this.CurrentUsernameTextBox.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.NewPasswordTextBox);
            this.groupBox2.Controls.Add(this.NewUserIDTextBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.NewUsernameTextBox);
            this.groupBox2.Location = new System.Drawing.Point(210, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 100);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "New info";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "UserID";
            // 
            // NewPasswordTextBox
            // 
            this.NewPasswordTextBox.Location = new System.Drawing.Point(81, 71);
            this.NewPasswordTextBox.Name = "NewPasswordTextBox";
            this.NewPasswordTextBox.PasswordChar = '*';
            this.NewPasswordTextBox.Size = new System.Drawing.Size(100, 20);
            this.NewPasswordTextBox.TabIndex = 11;
            // 
            // NewUserIDTextBox
            // 
            this.NewUserIDTextBox.Location = new System.Drawing.Point(81, 19);
            this.NewUserIDTextBox.Name = "NewUserIDTextBox";
            this.NewUserIDTextBox.Size = new System.Drawing.Size(100, 20);
            this.NewUserIDTextBox.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Username";
            // 
            // NewUsernameTextBox
            // 
            this.NewUsernameTextBox.Location = new System.Drawing.Point(81, 45);
            this.NewUsernameTextBox.Name = "NewUsernameTextBox";
            this.NewUsernameTextBox.Size = new System.Drawing.Size(100, 20);
            this.NewUsernameTextBox.TabIndex = 9;
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.Location = new System.Drawing.Point(291, 147);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(75, 23);
            this.UpdateBtn.TabIndex = 14;
            this.UpdateBtn.Text = "Update";
            this.UpdateBtn.UseVisualStyleBackColor = true;
            this.UpdateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // ErrMsgLabel
            // 
            this.ErrMsgLabel.AutoSize = true;
            this.ErrMsgLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrMsgLabel.Location = new System.Drawing.Point(12, 147);
            this.ErrMsgLabel.Name = "ErrMsgLabel";
            this.ErrMsgLabel.Size = new System.Drawing.Size(40, 13);
            this.ErrMsgLabel.TabIndex = 15;
            this.ErrMsgLabel.Text = "ErrMsg";
            // 
            // UserUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 177);
            this.Controls.Add(this.ErrMsgLabel);
            this.Controls.Add(this.UpdateBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BackNavBtn);
            this.Name = "UserUpdateForm";
            this.Text = "UserUpdate";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BackNavBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CurrentPasswordTextBox;
        private System.Windows.Forms.TextBox CurrentUserIDTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CurrentUsernameTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox NewPasswordTextBox;
        private System.Windows.Forms.TextBox NewUserIDTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox NewUsernameTextBox;
        private System.Windows.Forms.Button UpdateBtn;
        private System.Windows.Forms.Label ErrMsgLabel;
    }
}