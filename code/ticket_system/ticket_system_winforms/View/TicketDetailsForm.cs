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
        private TicketSystemDbContext _context;
        private readonly int currentUserId;

        public TaskDetailsForm(ProjectTask task, TicketSystemDbContext context)
        {
            InitializeComponent();
            this.task = task;
            currentUserId = ActiveEmployee.Employee.EId;
            _context = context;

            /*         string query = @"
                     select g.*
                     from groups g
                     where g.g_id in (
                         select gm.groupsexistingingid
                         from group_member gm
                         where gm.employeeseid = @userid
                         ) or g.manager_id = @userid;";

                     var groups = this._context.groups.fromsqlraw(query, new sqlparameter("@userid", currentuserid)).tolist();
                     var boardstates = this._context.stateassignedgroups.where(sag => groups.contains(sag.group)).select(sag => sag.boardstate).tolist();*/
            //context.ProjectBoards.Where(pb => pb.BoardId == task.BoardState.BoardId).SelectMany(pb => pb.States).ToList()

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

/*        private async Task<List<Group>> getGroupsForUser(int userId)
        {
            string query = @"
            SELECT g.*
            FROM Groups g
            WHERE g.g_id IN (
                SELECT gm.GroupsExistingInGId
                FROM group_member gm
                WHERE gm.EmployeesEId = @UserId
                ) OR g.manager_id = @UserId;";

            return await this._context.Groups.FromSqlRaw(query, new SqlParameter("@UserId", userId)).ToListAsync();
        }*/
    }
}
