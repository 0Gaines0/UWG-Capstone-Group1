﻿using Microsoft.EntityFrameworkCore;
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

            infoLabel.Text = $"Ticket#{this.task.TaskId} - {this.task.Summary} ({this.task.Priority})";
            viewButton.Text = "View";

            viewButton.Click += (s, e) =>
            {
                var form = new TaskDetailsForm(task, context);
                form.FormClosed += (sender, args) =>
                {
                    ReloadRequested?.Invoke(this, EventArgs.Empty);
                };
                form.ShowDialog();
            };
        }
    }
}
