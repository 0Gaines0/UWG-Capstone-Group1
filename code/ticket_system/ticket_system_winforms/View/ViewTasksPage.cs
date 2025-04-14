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
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_winforms.Model;
using ticket_system_winforms.View.UserControls;

namespace ticket_system_winforms.View
{
    public partial class ViewTasksPage : Form
    {
        private List<ProjectTask> allTasks;
        private readonly TicketSystemDbContext context;

        public ViewTasksPage(TicketSystemDbContext context)
        {
            InitializeComponent();
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context), "Input must not be null or empty");
            }
            this.context = context;
        }

        private async void ViewTasksPage_Load(object sender, EventArgs e)
        {
            comboBoxTaskFilter.Items.AddRange(new[] { "Available Tasks", "Active Tasks" });
            comboBoxTaskFilter.SelectedIndex = 0;
            comboBoxTaskFilter.SelectedIndexChanged += ComboBoxTaskFilter_SelectedIndexChanged;

            var employeeId = ActiveEmployee.Employee.EId;
            allTasks = await this.getTasksForUserAsync(employeeId);

            DisplayTasks("Available Tasks");
        }

        private void ComboBoxTaskFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBoxTaskFilter.SelectedItem.ToString();
            DisplayTasks(selected);
        }

        private void DisplayTasks(string filter)
        {
            flowLayoutPanelTasks.Controls.Clear();

            var currentUserId = ActiveEmployee.Employee.EId;
            var filteredTasks = filter == "Available Tasks"
                ? allTasks.Where(t => t.AssigneeId == null).ToList()
                : allTasks.Where(t => t.AssigneeId == currentUserId).ToList();

            foreach (var task in filteredTasks)
            {
                var control = new TaskItemControl(task);
                control.ViewClicked += TaskControl_ViewClicked;
                control.Margin = new Padding(5);
                flowLayoutPanelTasks.Controls.Add(control);
            }
        }

        private void TaskControl_ViewClicked(object sender, ProjectTask task)
        {
            MessageBox.Show(
                $"Task #{task.TaskId}\n\n{task.Summary}\nPriority: {task.Priority}\nCreated: {task.CreatedDate:d}",
                "Task Details");
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            ActiveEmployee.LogoutCurrentEmployee();
            Form form = new LoginPage(this.context);
            form.Show();
            this.Hide();
        }

        private async Task<List<ProjectTask>> getTasksForUserAsync(int userId)
        {
            string query = @"
            SELECT t.*
            FROM task t
            WHERE t.state_id IN (
                SELECT sg.StateId
                FROM StateAssignedGroups sg
                WHERE sg.GroupId IN (
                    SELECT gm.GroupsExistingInGId
                    FROM group_member gm
                    WHERE gm.EmployeesEId = @UserId
                )
            );";

            return await context.Tasks.FromSqlRaw(query, new SqlParameter("@UserId", userId)).ToListAsync();
        }
    }
}
