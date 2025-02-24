using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_web_app.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly TicketSystemDbContext context;

        public EmployeesController(TicketSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await this.constructEmployees();
            return View(employees);
        }

        //public IActionResult CreateGroupModal()
        //{
        //    return PartialView("_CreateGroupModal");
        //}

        [HttpGet]
        public async Task<JsonResult> GetAllEmployees()
        {
            var employees = await this.constructEmployees();
            return Json(employees);
            //var employees = await this.context.Employees
            //    .Select(employee => new { Id = employee.EId, Name = $"{employee.FName} {employee.LName}", employee.Username, employee.Email, Role = employee.IsAdmin }) // Standardized Id
            //    .AsNoTracking()
            //    .ToListAsync();

            //return Json(employees);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest jsonRequest)
        //{
        //    if (jsonRequest == null || string.IsNullOrWhiteSpace(jsonRequest.GroupName) || jsonRequest.ManagerId == 0)
        //    {
        //        return BadRequest(new { message = "Invalid request data" });
        //    }

        //    if (this.context.Groups.Where(group => group.GName == jsonRequest.GroupName).Any())
        //    {
        //        return BadRequest(new { message = $"Group name, {jsonRequest.GroupName}, already exists. Try Again." });
        //    }

        //    var newGroup = new Group(jsonRequest.ManagerId, jsonRequest.GroupName, jsonRequest.GroupDescription);

        //    if (jsonRequest.MemberIds != null && jsonRequest.MemberIds.Any())
        //    {
        //        var groupEmployees = await this.context.Employees.Where(employee => jsonRequest.MemberIds.Contains(employee.EId)).ToListAsync();
        //        newGroup.Employees = groupEmployees;
        //    }

        //    this.context.Groups.Add(newGroup);
        //    await this.context.SaveChangesAsync();

        //    return Ok(new { message = "Group created successfully", groupId = newGroup.GId });


        //}

        [HttpPost]
        public async Task<IActionResult> RemoveEmployee([FromBody] RemoveEmployeeRequest request)
        {
            int employeeId = request.EmployeeId.GetValueOrDefault();
            if (!this.context.Employees.Where(employee => employee.EId == employeeId).Any())
            {
                return BadRequest(new { success = false, message = "User id does not exist." });

            }

            bool isRemoved = await this.removeEmployeeFromDb(employeeId);

            if (isRemoved)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to remove Employee." });
            }
        }

        private async Task<bool> removeEmployeeFromDb(int employeeId)
        {
            try
            {
                var employee = await this.context.Employees.FirstOrDefaultAsync(currentEmployee => currentEmployee.EId == employeeId);
                this.context.Employees.Remove(employee);
                await this.context.SaveChangesAsync();
                return true;
                
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<List<object>> constructEmployees()
        {
            var employees = await this.context.Employees.Select(employee => new
            {
                Id = employee.EId,
                Name = employee.FName + " " + employee.LName,
                employee.Username,
                employee.Email,
                employee.IsAdmin
            }).ToListAsync();
            return employees.Cast<object>().ToList();
        }


    }
}
