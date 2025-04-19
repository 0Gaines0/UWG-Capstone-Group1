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
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

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
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.BackColor = System.Drawing.Color.Black;
            lblSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblSummary.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblSummary.ForeColor = System.Drawing.Color.White;
            lblSummary.Location = new System.Drawing.Point(12, 9);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new System.Drawing.Size(183, 47);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "Summary: ";
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.ForeColor = System.Drawing.Color.White;
            lblDescription.Location = new System.Drawing.Point(12, 112);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new System.Drawing.Size(73, 15);
            lblDescription.TabIndex = 1;
            lblDescription.Text = "Description: ";
            // 
            // lblPriority
            // 
            lblPriority.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblPriority.AutoSize = true;
            lblPriority.ForeColor = System.Drawing.Color.White;
            lblPriority.Location = new System.Drawing.Point(12, 180);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new System.Drawing.Size(51, 15);
            lblPriority.TabIndex = 2;
            lblPriority.Text = "Priority: ";
            // 
            // lblState
            // 
            lblState.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblState.AutoSize = true;
            lblState.ForeColor = System.Drawing.Color.White;
            lblState.Location = new System.Drawing.Point(12, 204);
            lblState.Name = "lblState";
            lblState.Size = new System.Drawing.Size(39, 15);
            lblState.TabIndex = 3;
            lblState.Text = "State: ";
            // 
            // cmbState
            // 
            cmbState.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbState.FormattingEnabled = true;
            cmbState.Location = new System.Drawing.Point(12, 222);
            cmbState.Name = "cmbState";
            cmbState.Size = new System.Drawing.Size(246, 23);
            cmbState.TabIndex = 4;
            // 
            // chkAssigned
            // 
            chkAssigned.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            chkAssigned.AutoSize = true;
            chkAssigned.ForeColor = System.Drawing.Color.White;
            chkAssigned.Location = new System.Drawing.Point(12, 251);
            chkAssigned.Name = "chkAssigned";
            chkAssigned.Size = new System.Drawing.Size(108, 19);
            chkAssigned.TabIndex = 5;
            chkAssigned.Text = "Assigned to me";
            chkAssigned.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSave.BackColor = System.Drawing.Color.FromArgb(159, 122, 234);
            btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(12, 276);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(120, 40);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save Changes";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.BackColor = System.Drawing.Color.FromArgb(159, 122, 234);
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Location = new System.Drawing.Point(138, 276);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new System.Windows.Forms.Padding(3);
            btnCancel.Size = new System.Drawing.Size(120, 40);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // TaskDetailsForm
            // 
            BackColor = System.Drawing.Color.FromArgb(41, 84, 61);
            ClientSize = new System.Drawing.Size(269, 328);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
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