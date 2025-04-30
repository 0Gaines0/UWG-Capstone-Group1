using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_winforms.Model;

namespace ticket_system_winforms.View.UserControls
{
    public partial class TaskItemControl : UserControl
    {
        public ProjectTask task { get; private set; }

        public event EventHandler<ProjectTask> ViewClicked;

        public event EventHandler ReloadRequested;

        public TaskItemControl(ProjectTask task, TicketSystemDbContext context)
        {
            InitializeComponent();
            this.task = task;

            infoLabel.Text = $"Ticket#{this.task.TaskId} - {this.task.Summary} ({this.getPriority(this.task.Priority)})";

            this.Click += (s, e) => onClick(task, context);
            infoLabel.Click += (s, e) => onClick(task, context);
        }

        private String getPriority(int priority)
        {
            switch (priority)
            {
                case 1:
                    return "Low";
                case 2:
                    return "Medium";
                case 3:
                    return "High";
                default:
                    return "Medium";
            }
        }

        private void onClick(ProjectTask task, TicketSystemDbContext context)
        {
            var form = new TaskDetailsForm(task, context);
            form.FormClosed += (sender, args) =>
            {
                ReloadRequested?.Invoke(this, EventArgs.Empty);
            };
            form.ShowDialog();
        }
    }
}
