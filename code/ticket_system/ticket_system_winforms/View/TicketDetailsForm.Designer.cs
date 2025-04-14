namespace ticket_system_winforms.View
{
    partial class TaskDetailsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.ComboBox cmbState;  // Dropdown for Task State
        private System.Windows.Forms.CheckBox chkAssigned;
        private System.Windows.Forms.Button btnChangeState;
        private System.Windows.Forms.Button btnAssignUnassign;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblSummary = new System.Windows.Forms.Label();
            lblDescription = new System.Windows.Forms.Label();
            lblPriority = new System.Windows.Forms.Label();
            lblState = new System.Windows.Forms.Label();
            cmbState = new System.Windows.Forms.ComboBox();
            chkAssigned = new System.Windows.Forms.CheckBox();
            btnChangeState = new System.Windows.Forms.Button();
            btnAssignUnassign = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Location = new System.Drawing.Point(12, 9);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new System.Drawing.Size(64, 15);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "Summary: ";
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new System.Drawing.Point(12, 40);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new System.Drawing.Size(73, 15);
            lblDescription.TabIndex = 1;
            lblDescription.Text = "Description: ";
            // 
            // lblPriority
            // 
            lblPriority.AutoSize = true;
            lblPriority.Location = new System.Drawing.Point(12, 70);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new System.Drawing.Size(51, 15);
            lblPriority.TabIndex = 2;
            lblPriority.Text = "Priority: ";
            // 
            // lblState
            // 
            lblState.AutoSize = true;
            lblState.Location = new System.Drawing.Point(12, 100);
            lblState.Name = "lblState";
            lblState.Size = new System.Drawing.Size(39, 15);
            lblState.TabIndex = 3;
            lblState.Text = "State: ";
            // 
            // cmbState
            // 
            cmbState.FormattingEnabled = true;
            cmbState.Location = new System.Drawing.Point(12, 120);
            cmbState.Name = "cmbState";
            cmbState.Size = new System.Drawing.Size(200, 23);
            cmbState.TabIndex = 4;
            // 
            // chkAssigned
            // 
            chkAssigned.AutoCheck = false;
            chkAssigned.AutoSize = true;
            chkAssigned.Location = new System.Drawing.Point(12, 150);
            chkAssigned.Name = "chkAssigned";
            chkAssigned.Size = new System.Drawing.Size(108, 19);
            chkAssigned.TabIndex = 5;
            chkAssigned.Text = "Assigned to me";
            chkAssigned.UseVisualStyleBackColor = true;
            // 
            // btnChangeState
            // 
            btnChangeState.Location = new System.Drawing.Point(12, 180);
            btnChangeState.Name = "btnChangeState";
            btnChangeState.Size = new System.Drawing.Size(100, 30);
            btnChangeState.TabIndex = 6;
            btnChangeState.Text = "Change State";
            btnChangeState.UseVisualStyleBackColor = true;
            btnChangeState.Click += btnChangeState_Click;
            // 
            // btnAssignUnassign
            // 
            btnAssignUnassign.Location = new System.Drawing.Point(130, 180);
            btnAssignUnassign.Name = "btnAssignUnassign";
            btnAssignUnassign.Size = new System.Drawing.Size(120, 30);
            btnAssignUnassign.TabIndex = 7;
            btnAssignUnassign.Text = "Assign / Unassign";
            btnAssignUnassign.UseVisualStyleBackColor = true;
            btnAssignUnassign.Click += btnAssignUnassign_Click;
            // 
            // TaskDetailsForm
            // 
            ClientSize = new System.Drawing.Size(284, 221);
            Controls.Add(btnAssignUnassign);
            Controls.Add(btnChangeState);
            Controls.Add(chkAssigned);
            Controls.Add(cmbState);
            Controls.Add(lblState);
            Controls.Add(lblPriority);
            Controls.Add(lblDescription);
            Controls.Add(lblSummary);
            Name = "TaskDetailsForm";
            Text = "Task Details";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}