namespace ticket_system_winforms.View.UserControls
{
    partial class TaskItemControl
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button viewButton;

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
            this.infoLabel = new System.Windows.Forms.Label();
            this.viewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.infoLabel.Location = new System.Drawing.Point(10, 10);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(150, 19);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Ticket#1 - Ticket Name (Priority)";
            // 
            // viewButton
            // 
            this.viewButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.viewButton.Location = new System.Drawing.Point(300, 5);
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(75, 30);
            this.viewButton.TabIndex = 1;
            this.viewButton.Text = "View";
            this.viewButton.UseVisualStyleBackColor = true;
            // 
            // TicketItemControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.viewButton);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TicketItemControl";
            this.Size = new System.Drawing.Size(390, 40);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
