using System.Windows.Forms;

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
            comboBoxTaskFilter = new ComboBox();
            flowLayoutPanelTasks = new FlowLayoutPanel();
            logoutButton = new Button();
            SuspendLayout();
            // 
            // comboBoxTaskFilter
            // 
            comboBoxTaskFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBoxTaskFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTaskFilter.FormattingEnabled = true;
            comboBoxTaskFilter.Location = new System.Drawing.Point(12, 12);
            comboBoxTaskFilter.Name = "comboBoxTaskFilter";
            comboBoxTaskFilter.Size = new System.Drawing.Size(204, 23);
            comboBoxTaskFilter.TabIndex = 0;
            // 
            // flowLayoutPanelTasks
            // 
            flowLayoutPanelTasks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanelTasks.AutoScroll = true;
            flowLayoutPanelTasks.WrapContents = true;
            flowLayoutPanelTasks.BackColor = System.Drawing.Color.FromArgb(15, 30, 19);
            flowLayoutPanelTasks.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelTasks.Location = new System.Drawing.Point(12, 50);
            flowLayoutPanelTasks.Name = "flowLayoutPanelTasks";
            flowLayoutPanelTasks.Size = new System.Drawing.Size(310, 400);
            flowLayoutPanelTasks.TabIndex = 1;
            flowLayoutPanelTasks.WrapContents = false;
            // 
            // logoutButton
            // 
            logoutButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            logoutButton.BackColor = System.Drawing.Color.FromArgb(159, 122, 234);
            logoutButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            logoutButton.ForeColor = System.Drawing.Color.White;
            logoutButton.Location = new System.Drawing.Point(222, 12);
            logoutButton.Name = "logoutButton";
            logoutButton.Padding = new Padding(3);
            logoutButton.Size = new System.Drawing.Size(100, 32);
            logoutButton.TabIndex = 2;
            logoutButton.Text = "Logout";
            logoutButton.UseVisualStyleBackColor = false;
            logoutButton.Click += logoutButton_Click;
            // 
            // ViewTasksPage
            // 
            BackColor = System.Drawing.Color.FromArgb(96, 130, 114);
            ClientSize = new System.Drawing.Size(334, 461);
            Controls.Add(logoutButton);
            Controls.Add(flowLayoutPanelTasks);
            Controls.Add(comboBoxTaskFilter);
            Name = "ViewTasksPage";
            Text = "View Tasks";
            Load += ViewTasksPage_Load;
            ResumeLayout(false);
        }
    }
}