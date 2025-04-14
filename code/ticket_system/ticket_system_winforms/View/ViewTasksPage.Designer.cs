using System.Windows.Forms;

namespace ticket_system_winforms.View
{
    partial class ViewTasksPage
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox comboBoxTaskFilter;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTasks;
        private System.Windows.Forms.Button createTaskButton;
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
            createTaskButton = new Button();
            SuspendLayout();
            // 
            // comboBoxTaskFilter
            // 
            comboBoxTaskFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTaskFilter.FormattingEnabled = true;
            comboBoxTaskFilter.Location = new System.Drawing.Point(12, 12);
            comboBoxTaskFilter.Name = "comboBoxTaskFilter";
            comboBoxTaskFilter.Size = new System.Drawing.Size(200, 23);
            comboBoxTaskFilter.TabIndex = 0;
            // 
            // flowLayoutPanelTasks
            // 
            flowLayoutPanelTasks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanelTasks.AutoScroll = true;
            flowLayoutPanelTasks.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelTasks.Location = new System.Drawing.Point(12, 50);
            flowLayoutPanelTasks.Name = "flowLayoutPanelTasks";
            flowLayoutPanelTasks.Size = new System.Drawing.Size(776, 388);
            flowLayoutPanelTasks.TabIndex = 1;
            flowLayoutPanelTasks.WrapContents = false;
            // 
            // logoutButton
            // 
            logoutButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            logoutButton.Location = new System.Drawing.Point(680, 12);
            logoutButton.Name = "logoutButton";
            logoutButton.Size = new System.Drawing.Size(100, 23);
            logoutButton.TabIndex = 2;
            logoutButton.Text = "Logout";
            logoutButton.UseVisualStyleBackColor = true;
            logoutButton.Click += logoutButton_Click;
            // 
            // createTaskButton
            // 
            createTaskButton.Location = new System.Drawing.Point(230, 7);
            createTaskButton.Name = "createTaskButton";
            createTaskButton.Size = new System.Drawing.Size(122, 32);
            createTaskButton.TabIndex = 0;
            createTaskButton.Text = "Create Task";
            createTaskButton.UseVisualStyleBackColor = true;
            createTaskButton.Click += createTaskButton_Click;
            // 
            // ViewTasksPage
            // 
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(createTaskButton);
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