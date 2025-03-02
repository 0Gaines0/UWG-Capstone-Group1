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

        public IActionResult CreateEmployeeModal()
        {
            return PartialView("_CreateEmployeeModal");
        }

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

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest jsonRequest)
        {
            if (jsonRequest == null || string.IsNullOrWhiteSpace(jsonRequest.FirstName) || string.IsNullOrWhiteSpace(jsonRequest.LastName) ||
                string.IsNullOrWhiteSpace(jsonRequest.Username) || string.IsNullOrWhiteSpace(jsonRequest.Password) || string.IsNullOrWhiteSpace(jsonRequest.Email))
            {
                return BadRequest(new { message = "Invalid request data" });
            }

            if (this.context.Employees.Where(employee => employee.Username == jsonRequest.Username).Any())
            {
                return BadRequest(new { message = $"Username, {jsonRequest.Username}, already exists. Try Again." });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(jsonRequest.Password, BCrypt.Net.BCrypt.GenerateSalt());

            var newUser = new Employee(fName: jsonRequest.FirstName, lName: jsonRequest.LastName, username: jsonRequest.Username, hashedPassword: hashedPassword, email: jsonRequest.Email, isAdmin: jsonRequest.IsAdmin, isActive: true);

            this.context.Employees.Add(newUser);
            await this.context.SaveChangesAsync();

            return Ok(new { message = "Employee created successfully", employeeId = newUser.EId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveEmployee([FromBody] RemoveEmployeeRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.username)) {
                return BadRequest(new { message = "Invalid request data" });
            }

            String userName = request.username;
            if (!this.context.Employees.Where(employee => employee.Username == userName).Any())
            {
                return BadRequest(new { success = false, message = "User with username does not exist." });

            }
            if (ActiveEmployee.Employee == null)
            {
                return BadRequest(new { success = false, message = "Not logged in." });
            }
            if (userName.Equals(ActiveEmployee.Employee.Username))
            {
                return BadRequest(new { success = false, message = "You can't delete yourself." });

            }

            bool isRemoved = await this.removeEmployeeFromDb(userName);

            if (isRemoved)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to remove Employee." });
            }
        }

        private async Task<bool> removeEmployeeFromDb(String username)
        {
            try
            {
                var employee = await this.context.Employees.FirstOrDefaultAsync(currentEmployee => currentEmployee.Username == username);
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
