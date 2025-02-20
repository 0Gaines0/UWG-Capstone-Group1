using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;

namespace ticket_system_web_app.Controllers
{
    public class GroupsController : Controller
    {

        private readonly TicketSystemDbContext context;

        public GroupsController(TicketSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await this.constructGroups();
            return View(groups);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllGroups()
        {
            var groups = await this.constructGroups();
            return Json(groups);
        }

        [HttpGet]
        public async Task<JsonResult> GetActiveUserGroups()
        {
            var activeEmployeeId = ActiveEmployee.Employee?.EId;
            if (activeEmployeeId == null)
            {
                return Json(null);
            }

            var managedGroups = this.context.Groups.Where(group => group.ManagerId == activeEmployeeId);
            var memberGroups = this.context.Groups.Where(group => group.Employees.Any(employ => employ.EId == activeEmployeeId));
            var userGroups = await managedGroups.Union(memberGroups).Distinct().ToListAsync();

            return Json(userGroups);
        }

        private async Task<List<object>> constructGroups()
        {
            var groups = await this.context.Groups.Include(group => group.Employees).Select(group => new
            {
                group.GId,
                group.GName,
                ManagerName = this.context.Employees.Where(employee => employee.EId == group.ManagerId).Select(employee => employee.FName + " " + employee.LName).FirstOrDefault(),
                MembersCount = group.Employees.Count() + 1,
                group.GDescription
            }).ToListAsync();
            return groups.Cast<object>().ToList();
        }


    }
}
