namespace ticket_system_winforms.View
{
    partial class ViewTasksPage
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox comboBoxTaskFilter;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTasks;
        private System.Windows.Forms.Button logoutButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxTaskFilter = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanelTasks = new System.Windows.Forms.FlowLayoutPanel();
            this.logoutButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxTaskFilter
            // 
            this.comboBoxTaskFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTaskFilter.FormattingEnabled = true;
            this.comboBoxTaskFilter.Location = new System.Drawing.Point(12, 12);
            this.comboBoxTaskFilter.Name = "comboBoxTaskFilter";
            this.comboBoxTaskFilter.Size = new System.Drawing.Size(200, 23);
            this.comboBoxTaskFilter.TabIndex = 0;
            // 
            // logoutButton
            // 
            this.logoutButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.logoutButton.Location = new System.Drawing.Point(680, 12);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(100, 23);
            this.logoutButton.TabIndex = 2;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // flowLayoutPanelTasks
            // 
            this.flowLayoutPanelTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | System.Windows.Forms.AnchorStyles.Left)
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelTasks.AutoScroll = true;
            this.flowLayoutPanelTasks.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelTasks.Location = new System.Drawing.Point(12, 50);
            this.flowLayoutPanelTasks.Name = "flowLayoutPanelTasks";
            this.flowLayoutPanelTasks.Size = new System.Drawing.Size(776, 388);
            this.flowLayoutPanelTasks.TabIndex = 1;
            this.flowLayoutPanelTasks.WrapContents = false;
            // 
            // ViewTasksPage
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.flowLayoutPanelTasks);
            this.Controls.Add(this.comboBoxTaskFilter);
            this.Name = "ViewTasksPage";
            this.Text = "View Tasks";
            this.Load += new System.EventHandler(this.ViewTasksPage_Load);
            this.ResumeLayout(false);
        }
    }
}