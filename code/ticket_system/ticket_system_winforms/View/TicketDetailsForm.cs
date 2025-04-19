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

            chkAssigned.Checked = task.AssigneeId == this.currentUserId;

            originalStateId = task.StateId;
            originalAssigneeId = task.AssigneeId;
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
    }
}
