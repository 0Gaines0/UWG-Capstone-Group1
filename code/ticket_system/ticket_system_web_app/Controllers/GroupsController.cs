using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;

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

        public IActionResult CreateGroupModal()
        {
            return PartialView("_CreateGroupModal");
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
            var userGroups = await managedGroups.Union(memberGroups).Distinct().Select(group => group.GName).ToListAsync();

            return Json(userGroups);
        }

        [HttpGet]
        public async Task<JsonResult> GetGroupById(int id)
        {
            var group = await this.context.Groups.Where(g => g.GId == id).Select(g => new
            {
                g.GId,
                g.GName,
                Manager = new { Name = this.context.Employees.Where(employee => employee.EId == g.ManagerId).Select(employee => employee.FName + " " + employee.LName).FirstOrDefault(), ID = this.context.Employees.Where(employee => employee.EId == g.ManagerId).Select(employee => employee.EId).FirstOrDefault() },
                Members = g.Employees
                    .Select(e => new {
                        eId = e.EId,
                        name = e.FName + " " + e.LName
                    })
                    .ToArray(),
                g.GDescription
            }).ToListAsync();
            return Json(group);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest jsonRequest)
        {
            if (jsonRequest == null || string.IsNullOrWhiteSpace(jsonRequest.GroupName) || jsonRequest.ManagerId == 0)
            {
                return BadRequest(new { message = "Invalid request data" });
            }

            if (this.context.Groups.Where(group => group.GName == jsonRequest.GroupName).Any())
            {
                return BadRequest(new { message = $"Group name, {jsonRequest.GroupName}, already exists. Try Again." });
            }

            var newGroup = new Group(jsonRequest.ManagerId, jsonRequest.GroupName, jsonRequest.GroupDescription);

            if (jsonRequest.MemberIds != null && jsonRequest.MemberIds.Any())
            {
                var groupEmployees = await this.context.Employees.Where(employee => jsonRequest.MemberIds.Contains(employee.EId)).ToListAsync();
                newGroup.Employees = groupEmployees;
            }

            this.context.Groups.Add(newGroup);
            await this.context.SaveChangesAsync();

            return Ok(new { message = "Group created successfully", groupId = newGroup.GId });


        }

        [HttpPost]
        public async Task<IActionResult> RemoveGroup([FromBody] RemoveGroupRequest request)
        {
            var groupName = request.GroupName;
            if (string.IsNullOrWhiteSpace(groupName))
            {
                return BadRequest(new { success = false, message = "Group name is required." });
            }
            if (!this.context.Groups.Where(group => group.GName ==  groupName).Any())
            {
                return BadRequest(new { success = false, message = "Group name does not exist." });

            }

            bool isRemoved = await this.removeGroupFromDb(groupName);

            if (isRemoved)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to remove group." });
            }
        }

        private async Task<bool> removeGroupFromDb(string groupName)
        {
            try
            {
                var group = await this.context.Groups.FirstOrDefaultAsync(currGroup => currGroup.GName == groupName);
                this.context.Groups.Remove(group);
                await this.context.SaveChangesAsync();
                return true;
                
            }
            catch (Exception)
            {
                return false;
            }
        }    

        [HttpPost]
        public async Task<IActionResult> SaveGroupEdits([FromBody] CreateGroupRequest jsonRequest)
        {
            var group = await this.context.Groups.Include(g => g.Employees).FirstOrDefaultAsync(g => g.GId == jsonRequest.GroupId);
            var duplicateGroup = await this.context.Groups.FirstOrDefaultAsync(g => g.GName == jsonRequest.GroupName && g.GId != jsonRequest.GroupId);
            if (duplicateGroup != null)
            {
                return Json(new { success = false, message = "A group with the same name already exists." });
            }
            group.GName = jsonRequest.GroupName;
            group.GDescription = jsonRequest.GroupDescription;
            group.ManagerId = jsonRequest.ManagerId;
            group.Employees.Clear();

            foreach (var memberId in jsonRequest.MemberIds)
            {
                var employee = await this.context.Employees.FindAsync(memberId);
                if (employee != null)
                {
                    group.Employees.Add(employee);
                }
            }
            await this.context.SaveChangesAsync();
            return Json(new { success = true, message = "Group updated successfully." });
        }

        // TODO: Move this to EmployeeController
        [HttpGet]
        public async Task<JsonResult> GetAllManagers()
        {
            

            var allManagers = this.context.Employees.Where(e => (e.IsManager ?? false) || (e.IsAdmin ?? false));
            var getNeededData = await allManagers.Select(e => new { Id = e.EId, Name = $"{e.FName} {e.LName}" }).ToListAsync();

            return Json(getNeededData);
        }

        // TODO: Move this to EmployeeController
        [HttpGet]
        public async Task<JsonResult> GetAllEmployees()
        {
            var employees = await this.context.Employees
                .Select(employee => new { Id = employee.EId, Name = $"{employee.FName} {employee.LName}" }) // Standardized Id
                .AsNoTracking()
                .ToListAsync();

            return Json(employees);
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
