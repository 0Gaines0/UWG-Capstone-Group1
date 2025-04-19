namespace ticket_system_winforms.View.UserControls
{
    partial class TaskItemControl
    {
        private System.ComponentModel.IContainer components = null;

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
            infoLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // infoLabel
            // 
            infoLabel.AutoSize = true;
            infoLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            infoLabel.ForeColor = System.Drawing.Color.White;
            infoLabel.Location = new System.Drawing.Point(10, 10);
            infoLabel.Name = "infoLabel";
            infoLabel.Size = new System.Drawing.Size(205, 19);
            infoLabel.TabIndex = 0;
            infoLabel.Text = "Ticket#1 - Ticket Name (Priority)";
            // 
            // TaskItemControl
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(41, 84, 61);
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(infoLabel);
            Margin = new System.Windows.Forms.Padding(5);
            Name = "TaskItemControl";
            Size = new System.Drawing.Size(300, 40);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label infoLabel;
    }
}
