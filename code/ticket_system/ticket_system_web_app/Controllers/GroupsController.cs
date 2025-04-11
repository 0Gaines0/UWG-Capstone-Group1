using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_web_app.Controllers
{
    /// <summary>
    /// GroupController class
    /// All client method calls require auth token validation.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class GroupsController : Controller
    {

        #region Fields

        private readonly TicketSystemDbContext context;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupsController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GroupsController(TicketSystemDbContext context)
        {
            this.context = context;
        }

        #endregion

        #region View Loaders

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var groups = await this.constructGroups();
            return View(groups);
        }

        /// <summary>
        /// Creates the group modal.
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateGroupModal()
        {
            return PartialView("_CreateGroupModal");
        }

        #endregion

        #region Authenticated Methods

        /// <summary>
        ///     Gets all groups.
        ///     Requires admin perms.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <returns>A Json object of all the groups, or a Json with an error message if request is invalid.</returns>
        [HttpGet("Groups/GetAllGroups/{authToken}")]
        public async Task<JsonResult> GetAllGroups(string authToken)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(GetAllGroups)} Got auth token: {authToken}");
                return Json("Not logged in.");
            }
            if (!ActiveEmployee.IsAdmin())
            {
                return Json("Admin permissions required.");
            }

            var groups = await this.constructGroups();
            return Json(groups);
        }

        /// <summary>
        ///     Gets the groups containing the active user.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <returns>A Json object of the groups, or a Json with an error message if request is invalid.</returns>
        [HttpGet("Groups/GetActiveUserGroups/{authToken}")]
        public async Task<JsonResult> GetActiveUserGroups(string authToken)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(GetActiveUserGroups)} Got auth token: {authToken}");
                return Json("Not logged in.");
            }

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

        /// <summary>
        ///     Gets the group with the specified ID.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>A Json object of the group, or a Json with an error message if request is invalid.</returns>
        [HttpGet("Groups/GetGroupById/{authToken}&{id}")]
        public async Task<JsonResult> GetGroupById(string authToken, int id)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(GetGroupById)} Got auth token: {authToken}");
                return Json("Not logged in.");
            }

            var group = await this.context.Groups.Where(g => g.GId == id).Select(g => new
            {
                ActiveEmployee.Employee.EId,
                ActiveEmployee.Employee.IsAdmin,
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

        /// <summary>
        ///     Creates the group.
        ///     Requires admin perms.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <param name="jsonRequest">The json request.</param>
        /// <returns>OK if successful, or a BadRequest with an error message if request is invalid.</returns>
        [HttpPost("Groups/CreateGroup/{authToken}")]
        public async Task<IActionResult> CreateGroup(string authToken, [FromBody] CreateGroupRequest jsonRequest)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(CreateGroup)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }
            if (!ActiveEmployee.IsAdmin())
            {
                return BadRequest(new { message = "Admin permissions required." });
            }

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

        /// <summary>
        ///     Removes the group.
        ///     Requires admin perms.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <param name="request">The request.</param>
        /// <returns>OK if successful, or a BadRequest with an error message if request is invalid.</returns>
        [HttpPost("Groups/RemoveGroup/{authToken}")]
        public async Task<IActionResult> RemoveGroup(string authToken, [FromBody] RemoveGroupRequest request)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(RemoveGroup)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }
            if (!ActiveEmployee.IsAdmin())
            {
                return BadRequest(new { message = "Admin permissions required." });
            }

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

        /// <summary>
        ///     Saves the group edits.
        ///     Requires admin perms.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <param name="jsonRequest">The json request.</param>
        /// <returns>A Json object with a confirmation message, or a Json with an error message if request is invalid.</returns>
        [HttpPost("Groups/SaveGroupEdits/{authToken}")]
        public async Task<IActionResult> SaveGroupEdits(string authToken, [FromBody] CreateGroupRequest jsonRequest)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(SaveGroupEdits)} Got auth token: {authToken}");
                return Json("Not logged in.");
            }
            if (!ActiveEmployee.IsAdmin())
            {
                return Json("Admin permissions required.");
            }

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

        /// <summary>
        ///     Gets all managers.
        ///     Requires admin perms.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <returns>A Json object of all of the managers, or a Json with an error message if request is invalid.</returns>
        [HttpGet("Groups/GetAllManagers/{authToken}")]
        public async Task<JsonResult> GetAllManagers(string authToken)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(GetAllManagers)} Got auth token: {authToken}");
                return Json("Not logged in.");
            }
            if (!ActiveEmployee.IsAdmin())
            {
                return Json("Admin permissions required.");
            }
            var possibleManagers = await this.context.Employees.Where(e => e.IsActive == true).Select(e => new { Id = e.EId, Name = $"{e.FName} {e.LName}" }).AsNoTracking().ToListAsync();

            return Json(possibleManagers);
        }

        /// <summary>
        ///     Gets all employees.
        ///     Requires admin perms.
        /// </summary>
        /// <param name="authToken">The auth token.</param>
        /// <returns>A Json object of all of the employees, or a Json with an error message if request is invalid.</returns>
        [HttpGet("Groups/GetAllEmployees/{authToken}")]
        public async Task<JsonResult> GetAllEmployees(string authToken)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(GetAllEmployees)} Got auth token: {authToken}");
                return Json("Not logged in.");
            }
            if (!ActiveEmployee.IsAdmin())
            {
                return Json("Admin permissions required.");
            }

            var employees = await this.context.Employees
                .Select(employee => new { Id = employee.EId, Name = $"{employee.FName} {employee.LName}" }) // Standardized Id
                .AsNoTracking()
                .ToListAsync();

            return Json(employees);
        }

        #endregion

        #region Helpers

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

        private async Task<List<object>> constructGroups()
        {
            var groups = await this.context.Groups.Include(group => group.Employees).Select(group => new
            {
                group.GId,
                group.GName,
                ManagerName = this.context.Employees.Where(employee => employee.EId == group.ManagerId).Select(employee => employee.FName + " " + employee.LName).FirstOrDefault(),
                group.ManagerId,
                MembersCount = group.Employees.Count() + 1,
                group.GDescription
            }).ToListAsync();
            return groups.Cast<object>().ToList();
        }

        #endregion
    }
}
