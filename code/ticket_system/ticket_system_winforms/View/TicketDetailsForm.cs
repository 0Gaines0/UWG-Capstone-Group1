using Microsoft.Data.SqlClient;
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
using System.Windows.Forms.VisualStyles;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;


namespace ticket_system_winforms.View
{
    public partial class TaskDetailsForm : Form
    {
        private ProjectTask task;
        private TicketSystemDbContext context;
        private readonly int currentUserId;
        private int originalStateId;
        private int? originalAssigneeId;

        public TaskDetailsForm(ProjectTask task, TicketSystemDbContext context)
        {
            InitializeComponent();
            this.task = task;
            currentUserId = ActiveEmployee.Employee.EId;
            this.context = context;

            var boardStatesForTask = this.context.ProjectBoards.Where(pb => pb.BoardId == this.task.BoardState.BoardId).SelectMany(pb => pb.States).ToList();
            cmbState.DataSource = boardStatesForTask;
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateId";
            cmbState.SelectedValue = task.StateId;

            lblSummary.Text = $"Summary: {Environment.NewLine}{task.Summary}";
            lblDescription.Text = $"Description: {Environment.NewLine}{task.Description}";
            lblPriority.Text = $"Priority: {this.getPriority(task.Priority)}";

            lblDescription.AutoSize = true;

            // Dynamically reposition controls based on label content
            lblPriority.Top = lblDescription.Bottom + 10;
            lblState.Top = lblPriority.Bottom + 5;
            cmbState.Top = lblState.Bottom + 3;
            chkAssigned.Top = cmbState.Bottom + 5;
            btnSave.Top = chkAssigned.Bottom + 10;
            btnCancel.Top = chkAssigned.Bottom + 10;
            tabControl.Top = btnSave.Bottom + 10;

            // Optional: update the form height if needed
            this.ClientSize = new Size(this.ClientSize.Width, tabControl.Bottom + 20);

            chkAssigned.Checked = task.AssigneeId == this.currentUserId;

            originalStateId = task.StateId;
            originalAssigneeId = task.AssigneeId;
            LoadComments();
            LoadChangeLog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var selfAssigned = chkAssigned.Checked;
            var selectedStateId = (int)cmbState.SelectedValue;

            if ((selfAssigned == (originalAssigneeId.HasValue)) && (this.originalStateId == selectedStateId))
            {
                MessageBox.Show("No changes made.");
                return;
            }

            if (selfAssigned)
            {
                var employeeGroups = ActiveEmployee.Employee.GroupsExistingIn.Union(ActiveEmployee.Employee.ManagedGroups);
                var employeeStates = this.context.StateAssignedGroups.Where(sag => employeeGroups.Contains(sag.Group)).Select(pb => pb.StateId).ToList();
                if (!employeeStates.Contains((int)cmbState.SelectedValue))
                {
                    MessageBox.Show($"You can't be assigned to a ticket in the {cmbState.Text} state.");
                    return;
                }
            }


            task.StateId = selectedStateId;
            task.AssigneeId = selfAssigned ? currentUserId : (int?)null;

            context.SaveChanges();
            MessageBox.Show("Changes saved.");
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
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

        private void LoadComments()
        {
            var comments = context.TaskComments
                .Where(c => c.TaskId == task.TaskId)
                .Include(c => c.Commenter)
                .OrderByDescending(c => c.CommentedAt)
                .ToList();

            lstComments.Items.Clear();
            foreach (var comment in comments)
            {
                string display = $"{comment.Commenter.FName + " " + comment.Commenter.LName} ({comment.CommentedAt:g}): {comment.CommentText}";
                lstComments.Items.Add(display);
            }
        }

        private void LoadChangeLog()
        {
            var changeLogs = context.TaskChangeLogs
                .Where(l => l.TaskId == task.TaskId)
                .Include(l => l.TaskChange)
                .ThenInclude(c => c.Assignee)
                .OrderByDescending(l => l.TaskChange.ChangedDate)
                .ToList();

            foreach (var log in changeLogs)
            {
                var change = log.TaskChange!;
                dataGridChangeLog.Rows.Add(
                    change.ChangedDate.ToString("g"),
                    change.Type,
                    change.PreviousValue ?? "-",
                    change.NewValue,
                    change.Assignee.FName + " " + change.Assignee.LName
                );
            }
            dataGridChangeLog.Refresh();
        }

        private void btnPostComment_Click(object sender, EventArgs e)
        {
            var text = txtNewComment.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Comment cannot be empty.");
                return;
            }

            var comment = new TaskComment
            {
                CommentText = text,
                CommentedAt = DateTime.Now,
                TaskId = task.TaskId,
                CommenterId = currentUserId
            };

            context.TaskComments.Add(comment);
            context.SaveChanges();
            txtNewComment.Clear();
            LoadComments();
        }

        private void TaskDetailsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
