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

        private void ViewTasksPage_Load(object sender, EventArgs e)
        {
            comboBoxTaskFilter.Items.AddRange(new[] { "Available Tasks", "Active Tasks" });
            comboBoxTaskFilter.SelectedIndex = 0;
            comboBoxTaskFilter.SelectedIndexChanged += ComboBoxTaskFilter_SelectedIndexChanged;

            var employeeGroups = ActiveEmployee.Employee.GroupsExistingIn.Union(ActiveEmployee.Employee.ManagedGroups);
            var groupedTasks =  this.context.StateAssignedGroups.Where(sag => employeeGroups.Contains(sag.Group)).SelectMany(sag => sag.BoardState.Tasks).Include(pt => pt.BoardState).ToList();
            var ungroupedTasks = employeeGroups.SelectMany(g => g.Collaborations).Where(pg => pg.Accepted).SelectMany(pg => pg.Project?.ProjectBoard?.States ?? new List<BoardState>())
                .Where(state => state.AssignedGroups == null || !state.AssignedGroups.Any()).SelectMany(state => state.Tasks).ToList();

            this.allTasks = groupedTasks.Union(ungroupedTasks).ToList();

            DisplayTasks("Available Tasks");
        }

        public void ReloadTasks()
        {
            string selected = comboBoxTaskFilter.SelectedItem.ToString();
            DisplayTasks(selected);
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

            var employeeGroups = ActiveEmployee.Employee.GroupsExistingIn.Union(ActiveEmployee.Employee.ManagedGroups);
            var groupedTasks = this.context.StateAssignedGroups.Where(sag => employeeGroups.Contains(sag.Group)).SelectMany(sag => sag.BoardState.Tasks).Include(pt => pt.BoardState).ToList();
            var ungroupedTasks = employeeGroups.SelectMany(g => g.Collaborations).Where(pg => pg.Accepted).SelectMany(pg => pg.Project?.ProjectBoard?.States ?? new List<BoardState>())
                .Where(state => state.AssignedGroups == null || !state.AssignedGroups.Any()).SelectMany(state => state.Tasks).ToList();
            this.allTasks = groupedTasks.Union(ungroupedTasks).ToList();

            var filteredTasks = filter == "Available Tasks"
                ? allTasks.Where(t => t.AssigneeId == null).ToList()
                : allTasks.Where(t => t.AssigneeId == currentUserId).ToList();

            foreach (var task in filteredTasks)
            {
                var control = new TaskItemControl(task, this.context);
                control.ViewClicked += TaskControl_ViewClicked;
                control.Margin = new Padding(5);
                flowLayoutPanelTasks.Controls.Add(control);
                control.ReloadRequested += (s, e) => ReloadTasks();
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
    }
}
