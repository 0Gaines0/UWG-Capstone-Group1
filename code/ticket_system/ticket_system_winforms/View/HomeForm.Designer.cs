namespace ticket_system_winforms
{
    partial class HomeForm
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
            this.UsersNavBtn.Location = new System.Drawing.Point(93, 12);
            this.UsersNavBtn.Name = "UsersNavBtn";
            this.UsersNavBtn.Size = new System.Drawing.Size(75, 23);
            this.UsersNavBtn.TabIndex = 4;
            this.UsersNavBtn.Text = "Users";
            this.UsersNavBtn.UseVisualStyleBackColor = true;
            this.UsersNavBtn.Click += new System.EventHandler(this.UsersNavBtn_Click);
            // 
            // HomeNavBtn
            // 
            this.HomeNavBtn.Enabled = false;
            this.HomeNavBtn.Location = new System.Drawing.Point(12, 12);
            this.HomeNavBtn.Name = "HomeNavBtn";
            this.HomeNavBtn.Size = new System.Drawing.Size(75, 23);
            this.HomeNavBtn.TabIndex = 3;
            this.HomeNavBtn.Text = "Home";
            this.HomeNavBtn.UseVisualStyleBackColor = true;
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PrivacyNavBtn);
            this.Controls.Add(this.UsersNavBtn);
            this.Controls.Add(this.HomeNavBtn);
            this.Name = "HomeForm";
            this.Text = "Home";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button PrivacyNavBtn;
        private System.Windows.Forms.Button UsersNavBtn;
        private System.Windows.Forms.Button HomeNavBtn;
    }
}

