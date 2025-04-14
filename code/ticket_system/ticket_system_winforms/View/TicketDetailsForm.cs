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
        private TicketSystemDbContext _context;
        private readonly int currentUserId;

        public TaskDetailsForm(ProjectTask task, TicketSystemDbContext context)
        {
            InitializeComponent();
            this.task = task;
            currentUserId = ActiveEmployee.Employee.EId;
            _context = context;

            cmbState.DataSource = _context.BoardStates.ToList();
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateId";
            cmbState.SelectedValue = task.StateId;

            lblSummary.Text = $"Summary: {task.Summary}";
            lblDescription.Text = $"Description: {task.Description}";
            lblPriority.Text = $"Priority: {task.Priority}";
            lblState.Text = $"State: {task.StateId}";

            chkAssigned.Checked = task.AssigneeId == currentUserId;
            btnAssignUnassign.Text = chkAssigned.Checked ? "Unassign" : "Assign";
        }

        private void btnAssignUnassign_Click(object sender, EventArgs e)
        {
            if (chkAssigned.Checked)
            {
                task.AssigneeId = null;
                btnAssignUnassign.Text = "Assign";
                chkAssigned.Checked = false;
            }
            else
            {
                task.AssigneeId = currentUserId;
                btnAssignUnassign.Text = "Unassign";
                chkAssigned.Checked = true;
            }
            _context.SaveChanges();
        }

        private void btnChangeState_Click(object sender, EventArgs e)
        {
            var selectedStateId = (int)cmbState.SelectedValue;
            task.StateId = selectedStateId;

            _context.SaveChanges();
        }
    }
}
