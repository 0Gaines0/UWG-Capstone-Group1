namespace ticket_system_winforms.View
{
    partial class UsersForm
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
            this.PrivacyNavBtn = new System.Windows.Forms.Button();
            this.UsersNavBtn = new System.Windows.Forms.Button();
            this.HomeNavBtn = new System.Windows.Forms.Button();
            this.DeleteUserBtn = new System.Windows.Forms.Button();
            this.UpdateUserBtn = new System.Windows.Forms.Button();
            this.RetrieveUserBtn = new System.Windows.Forms.Button();
            this.CreateUserBtn = new System.Windows.Forms.Button();
            this.UsersListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // PrivacyNavBtn
            // 
            this.PrivacyNavBtn.Location = new System.Drawing.Point(174, 12);
            this.PrivacyNavBtn.Name = "PrivacyNavBtn";
            this.PrivacyNavBtn.Size = new System.Drawing.Size(75, 23);
            this.PrivacyNavBtn.TabIndex = 5;
            this.PrivacyNavBtn.Text = "Privacy";
            this.PrivacyNavBtn.UseVisualStyleBackColor = true;
            this.PrivacyNavBtn.Click += new System.EventHandler(this.PrivacyNavBtn_Click);
            // 
            // UsersNavBtn
            // 
            this.UsersNavBtn.Enabled = false;
            this.UsersNavBtn.Location = new System.Drawing.Point(93, 12);
            this.UsersNavBtn.Name = "UsersNavBtn";
            this.UsersNavBtn.Size = new System.Drawing.Size(75, 23);
            this.UsersNavBtn.TabIndex = 4;
            this.UsersNavBtn.Text = "Users";
            this.UsersNavBtn.UseVisualStyleBackColor = true;
            // 
            // HomeNavBtn
            // 
            this.HomeNavBtn.Location = new System.Drawing.Point(12, 12);
            this.HomeNavBtn.Name = "HomeNavBtn";
            this.HomeNavBtn.Size = new System.Drawing.Size(75, 23);
            this.HomeNavBtn.TabIndex = 3;
            this.HomeNavBtn.Text = "Home";
            this.HomeNavBtn.UseVisualStyleBackColor = true;
            this.HomeNavBtn.Click += new System.EventHandler(this.HomeNavBtn_Click);
            // 
            // DeleteUserBtn
            // 
            this.DeleteUserBtn.Enabled = false;
            this.DeleteUserBtn.Location = new System.Drawing.Point(139, 128);
            this.DeleteUserBtn.Name = "DeleteUserBtn";
            this.DeleteUserBtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteUserBtn.TabIndex = 15;
            this.DeleteUserBtn.Text = "Delete";
            this.DeleteUserBtn.UseVisualStyleBackColor = true;
            // 
            // UpdateUserBtn
            // 
            this.UpdateUserBtn.Location = new System.Drawing.Point(139, 99);
            this.UpdateUserBtn.Name = "UpdateUserBtn";
            this.UpdateUserBtn.Size = new System.Drawing.Size(75, 23);
            this.UpdateUserBtn.TabIndex = 14;
            this.UpdateUserBtn.Text = "Edit";
            this.UpdateUserBtn.UseVisualStyleBackColor = true;
            this.UpdateUserBtn.Click += new System.EventHandler(this.UpdateUserBtn_Click);
            // 
            // RetrieveUserBtn
            // 
            this.RetrieveUserBtn.Enabled = false;
            this.RetrieveUserBtn.Location = new System.Drawing.Point(139, 70);
            this.RetrieveUserBtn.Name = "RetrieveUserBtn";
            this.RetrieveUserBtn.Size = new System.Drawing.Size(75, 23);
            this.RetrieveUserBtn.TabIndex = 13;
            this.RetrieveUserBtn.Text = "Details (WIP)";
            this.RetrieveUserBtn.UseVisualStyleBackColor = true;
            // 
            // CreateUserBtn
            // 
            this.CreateUserBtn.Location = new System.Drawing.Point(139, 41);
            this.CreateUserBtn.Name = "CreateUserBtn";
            this.CreateUserBtn.Size = new System.Drawing.Size(75, 23);
            this.CreateUserBtn.TabIndex = 12;
            this.CreateUserBtn.Text = "New";
            this.CreateUserBtn.UseVisualStyleBackColor = true;
            this.CreateUserBtn.Click += new System.EventHandler(this.CreateUserBtn_Click);
            // 
            // UsersListBox
            // 
            this.UsersListBox.DisplayMember = "Username";
            this.UsersListBox.FormattingEnabled = true;
            this.UsersListBox.Location = new System.Drawing.Point(12, 41);
            this.UsersListBox.Name = "UsersListBox";
            this.UsersListBox.Size = new System.Drawing.Size(120, 108);
            this.UsersListBox.TabIndex = 16;
            // 
            // UsersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UsersListBox);
            this.Controls.Add(this.DeleteUserBtn);
            this.Controls.Add(this.UpdateUserBtn);
            this.Controls.Add(this.RetrieveUserBtn);
            this.Controls.Add(this.CreateUserBtn);
            this.Controls.Add(this.PrivacyNavBtn);
            this.Controls.Add(this.UsersNavBtn);
            this.Controls.Add(this.HomeNavBtn);
            this.Name = "UsersForm";
            this.Text = "Users";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button PrivacyNavBtn;
        private System.Windows.Forms.Button UsersNavBtn;
        private System.Windows.Forms.Button HomeNavBtn;
        private System.Windows.Forms.Button DeleteUserBtn;
        private System.Windows.Forms.Button UpdateUserBtn;
        private System.Windows.Forms.Button RetrieveUserBtn;
        private System.Windows.Forms.Button CreateUserBtn;
        private System.Windows.Forms.ListBox UsersListBox;
    }
}