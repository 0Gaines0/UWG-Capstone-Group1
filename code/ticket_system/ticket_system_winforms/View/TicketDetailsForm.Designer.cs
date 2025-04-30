using System.Windows.Forms;

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
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageComments;
        private System.Windows.Forms.TabPage tabPageChangeLog;
        private System.Windows.Forms.ListBox lstComments;
        private System.Windows.Forms.TextBox txtNewComment;
        private System.Windows.Forms.Button btnPostComment;
        private System.Windows.Forms.DataGridView dataGridChangeLog;


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
            lblSummary = new Label();
            lblDescription = new Label();
            lblPriority = new Label();
            lblState = new Label();
            cmbState = new ComboBox();
            chkAssigned = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            tabControl = new TabControl();
            tabPageComments = new TabPage();
            lstComments = new ListBox();
            txtNewComment = new TextBox();
            btnPostComment = new Button();
            tabPageChangeLog = new TabPage();
            dataGridChangeLog = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            tabControl.SuspendLayout();
            tabPageComments.SuspendLayout();
            tabPageChangeLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridChangeLog).BeginInit();
            SuspendLayout();
            // 
            // lblSummary
            // 
            lblSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSummary.AutoEllipsis = true;
            lblSummary.ForeColor = System.Drawing.Color.White;
            lblSummary.Location = new System.Drawing.Point(12, 9);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new System.Drawing.Size(351, 31);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "Summary: ";
            // 
            // lblDescription
            // 
            lblDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblDescription.AutoSize = true;
            lblDescription.ForeColor = System.Drawing.Color.White;
            lblDescription.Location = new System.Drawing.Point(12, 48);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new System.Drawing.Size(73, 15);
            lblDescription.TabIndex = 1;
            lblDescription.Text = "Description: ";
            // 
            // lblPriority
            // 
            lblPriority.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblPriority.AutoSize = true;
            lblPriority.ForeColor = System.Drawing.Color.White;
            lblPriority.Location = new System.Drawing.Point(12, 116);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new System.Drawing.Size(51, 15);
            lblPriority.TabIndex = 2;
            lblPriority.Text = "Priority: ";
            // 
            // lblState
            // 
            lblState.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblState.AutoSize = true;
            lblState.ForeColor = System.Drawing.Color.White;
            lblState.Location = new System.Drawing.Point(12, 140);
            lblState.Name = "lblState";
            lblState.Size = new System.Drawing.Size(39, 15);
            lblState.TabIndex = 3;
            lblState.Text = "State: ";
            // 
            // cmbState
            // 
            cmbState.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cmbState.FormattingEnabled = true;
            cmbState.Location = new System.Drawing.Point(12, 158);
            cmbState.Name = "cmbState";
            cmbState.Size = new System.Drawing.Size(355, 23);
            cmbState.TabIndex = 4;
            // 
            // chkAssigned
            // 
            chkAssigned.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            chkAssigned.AutoSize = true;
            chkAssigned.ForeColor = System.Drawing.Color.White;
            chkAssigned.Location = new System.Drawing.Point(12, 187);
            chkAssigned.Name = "chkAssigned";
            chkAssigned.Size = new System.Drawing.Size(108, 19);
            chkAssigned.TabIndex = 5;
            chkAssigned.Text = "Assigned to me";
            chkAssigned.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = System.Drawing.Color.FromArgb(159, 122, 234);
            btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(121, 212);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(120, 40);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save Changes";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.BackColor = System.Drawing.Color.FromArgb(197, 48, 48);
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Location = new System.Drawing.Point(247, 212);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(3);
            btnCancel.Size = new System.Drawing.Size(120, 40);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabPageComments);
            tabControl.Controls.Add(tabPageChangeLog);
            tabControl.Location = new System.Drawing.Point(12, 264);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(355, 270);
            tabControl.TabIndex = 8;
            // 
            // tabPageComments
            // 
            tabPageComments.BackColor = System.Drawing.Color.FromArgb(41, 84, 61);
            tabPageComments.Controls.Add(lstComments);
            tabPageComments.Controls.Add(txtNewComment);
            tabPageComments.Controls.Add(btnPostComment);
            tabPageComments.ForeColor = System.Drawing.Color.White;
            tabPageComments.Location = new System.Drawing.Point(4, 24);
            tabPageComments.Name = "tabPageComments";
            tabPageComments.Padding = new Padding(3);
            tabPageComments.Size = new System.Drawing.Size(347, 242);
            tabPageComments.TabIndex = 0;
            tabPageComments.Text = "Comments";
            // 
            // lstComments
            // 
            lstComments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lstComments.BackColor = System.Drawing.Color.White;
            lstComments.Font = new System.Drawing.Font("Segoe UI", 9F);
            lstComments.ForeColor = System.Drawing.Color.Black;
            lstComments.FormattingEnabled = true;
            lstComments.ItemHeight = 15;
            lstComments.Location = new System.Drawing.Point(6, 6);
            lstComments.Name = "lstComments";
            lstComments.Size = new System.Drawing.Size(335, 199);
            lstComments.TabIndex = 0;
            // 
            // txtNewComment
            // 
            txtNewComment.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNewComment.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtNewComment.Location = new System.Drawing.Point(6, 211);
            txtNewComment.Name = "txtNewComment";
            txtNewComment.Size = new System.Drawing.Size(269, 23);
            txtNewComment.TabIndex = 1;
            // 
            // btnPostComment
            // 
            btnPostComment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPostComment.BackColor = System.Drawing.Color.FromArgb(159, 122, 234);
            btnPostComment.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnPostComment.ForeColor = System.Drawing.Color.White;
            btnPostComment.Location = new System.Drawing.Point(281, 211);
            btnPostComment.Name = "btnPostComment";
            btnPostComment.Size = new System.Drawing.Size(60, 23);
            btnPostComment.TabIndex = 2;
            btnPostComment.Text = "Post";
            btnPostComment.UseVisualStyleBackColor = false;
            btnPostComment.Click += btnPostComment_Click;
            // 
            // tabPageChangeLog
            // 
            tabPageChangeLog.BackColor = System.Drawing.Color.FromArgb(41, 84, 61);
            tabPageChangeLog.Controls.Add(dataGridChangeLog);
            tabPageChangeLog.ForeColor = System.Drawing.Color.White;
            tabPageChangeLog.Location = new System.Drawing.Point(4, 24);
            tabPageChangeLog.Name = "tabPageChangeLog";
            tabPageChangeLog.Padding = new Padding(3);
            tabPageChangeLog.Size = new System.Drawing.Size(347, 132);
            tabPageChangeLog.TabIndex = 1;
            tabPageChangeLog.Text = "History";
            // 
            // dataGridChangeLog
            // 
            dataGridChangeLog.AllowUserToAddRows = false;
            dataGridChangeLog.AllowUserToDeleteRows = false;
            dataGridChangeLog.AllowUserToResizeRows = false;
            dataGridChangeLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridChangeLog.BackgroundColor = System.Drawing.Color.White;
            dataGridChangeLog.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5 });
            dataGridChangeLog.Font = new System.Drawing.Font("Segoe UI", 9F);
            tabPageChangeLog.ForeColor = System.Drawing.Color.Black;
            dataGridChangeLog.Location = new System.Drawing.Point(6, 6);
            dataGridChangeLog.Name = "dataGridChangeLog";
            dataGridChangeLog.Size = new System.Drawing.Size(335, 120);
            dataGridChangeLog.TabIndex = 9;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.DataPropertyName = "Date";
            dataGridViewTextBoxColumn1.HeaderText = "Date";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.DataPropertyName = "Type";
            dataGridViewTextBoxColumn2.HeaderText = "Type";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.DataPropertyName = "PreviousValue";
            dataGridViewTextBoxColumn3.HeaderText = "Previous Value";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.DataPropertyName = "NewValue";
            dataGridViewTextBoxColumn4.HeaderText = "New Value";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.DataPropertyName = "Editor";
            dataGridViewTextBoxColumn5.HeaderText = "Editor";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // TaskDetailsForm
            // 
            BackColor = System.Drawing.Color.FromArgb(41, 84, 61);
            ClientSize = new System.Drawing.Size(378, 546);
            Controls.Add(tabControl);
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
            Load += TaskDetailsForm_Load;
            tabControl.ResumeLayout(false);
            tabPageComments.ResumeLayout(false);
            tabPageComments.PerformLayout();
            tabPageChangeLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridChangeLog).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}