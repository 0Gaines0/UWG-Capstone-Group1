using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_web_app.Controllers
{
    /// <summary>
    /// The controller/server class for the manage employees page.
    /// </summary>
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

        /// <summary>
        /// Returns a list of all the employees in a Json object.
        /// </summary>
        /// <returns>A Json object of all the employees</returns>
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

        /// <summary>
        /// Creates the employee with the given information if it doesn't exist already.
        /// </summary>
        /// <param name="jsonRequest">The new employee information</param>
        /// <returns>Ok if employee was created, BadRequest otherwise</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest jsonRequest)
        {
            if (!IsLoggedIn())
            {
                return BadRequest(new { success = false, message = "Not logged in." });
            }
            if (!IsAdmin())
            {
                return BadRequest(new { success = false, message = "Admin permissions required." });
            }
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

        /// <summary>
        /// Removes the given employee if they are not the current user and don't manage a group.
        /// </summary>
        /// <param name="request">The employee to be deleted username</param>
        /// <returns>Ok if employee was removed, BadRequest otherwise</returns>
        [HttpPost]
        public async Task<IActionResult> RemoveEmployee([FromBody] RemoveEmployeeRequest request)
        {
            if (!IsLoggedIn())
            {
                return BadRequest(new { success = false, message = "Not logged in." });
            }
            if (!IsAdmin())
            {
                return BadRequest(new { success = false, message = "Admin permissions required." });
            }
            if (request == null || string.IsNullOrWhiteSpace(request.username)) {
                return BadRequest(new { message = "Invalid request data" });
            }

            String userName = request.username;
            if (!this.context.Employees.Where(employee => employee.Username == userName).Any())
            {
                return BadRequest(new { success = false, message = "Employee with username does not exist." });
            }
            if (userName.Equals(ActiveEmployee.Employee.Username))
            {
                return BadRequest(new { success = false, message = "You can't delete yourself." });
            }
            var employee = await this.context.Employees.Where(employee => employee.Username == userName).Select(employee => new
            {
                employee.EId
            }).FirstOrDefaultAsync();
            if (this.context.Groups.Where(group => group.ManagerId == employee.EId).Any())
            {
                return BadRequest(new { success = false, message = "Employee can't be deleted while being a group manager" });
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

        /// <summary>
        /// Edits the given employee's data.
        /// </summary>
        /// <param name="jsonRequest">The employee username and updated employee information</param>
        /// <returns>Ok if successfull, BadRequest otherwise</returns>
        [HttpPost]
        public async Task<IActionResult> EditEmployee([FromBody] EditEmployeeRequest jsonRequest)
        {
            if (!IsLoggedIn())
            {
                return BadRequest(new { success = false, message = "Not logged in." });
            }
            if (!IsAdmin())
            {
                return BadRequest(new { success = false, message = "Admin permissions required." });
            }
            if (jsonRequest == null || string.IsNullOrWhiteSpace(jsonRequest.FirstName) || string.IsNullOrWhiteSpace(jsonRequest.LastName) ||
                string.IsNullOrWhiteSpace(jsonRequest.Username) || string.IsNullOrWhiteSpace(jsonRequest.Email) || string.IsNullOrWhiteSpace(jsonRequest.OriginalUsername))
            {
                return BadRequest(new { message = "Invalid request data" });
            }
            if (!this.context.Employees.Where(employee => employee.Username == jsonRequest.OriginalUsername).Any())
            {
                return BadRequest(new { message = $"Employee with username, {jsonRequest.OriginalUsername}, doesn't exists." });
            }
            if (jsonRequest.OriginalUsername != jsonRequest.Username && this.context.Employees.Where(employee => employee.Username == jsonRequest.Username).Any())
            {
                return BadRequest(new { message = $"Username, {jsonRequest.Username}, is already being used. Try a different username." });
            }
            if (jsonRequest.OriginalUsername.Equals(ActiveEmployee.Employee.Username) && !jsonRequest.IsAdmin)
            {
                return BadRequest(new { message = "Can't change your own admin permissions" });
            }

            var existingEmployee = await this.context.Employees.FirstOrDefaultAsync(e => e.Username == jsonRequest.OriginalUsername);
            existingEmployee.FName = jsonRequest.FirstName;
            existingEmployee.LName = jsonRequest.LastName;
            existingEmployee.Email = jsonRequest.Email;
            existingEmployee.Username = jsonRequest.Username;
            existingEmployee.IsAdmin = jsonRequest.IsAdmin;

            await this.context.SaveChangesAsync();

            return Ok(new { message = "Employee created successfully"});
        }

        /// <summary>
        /// Removes employee with the given username from the database.
        /// </summary>
        /// <param name="username">The username of the employee to be removed</param>
        /// <returns>True if employee was successfully removed, false otherwise</returns>
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

        /// <summary>
        /// Creates a list of all the employees.
        /// </summary>
        /// <returns>List of all employees</returns>
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

        /// <summary>
        /// Returns the employee data for the given username.
        /// </summary>
        /// <param name="data">The employee's username</param>
        /// <returns>The employee data for this username</returns>
        [HttpPost]
        public async Task<object> GetEmployee([FromBody] GetEmployeeRequest data)
        {
            if (!IsLoggedIn())
            {
                return BadRequest(new { success = false, message = "Not logged in." });
            }
            if (!IsAdmin())
            {
                return BadRequest(new { success = false, message = "Admin permissions required." });
            }
            if (data == null)
            {
                return BadRequest(new { message = "Bad request" });
            }
            string username = data.username;
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new { message = "Invalid username" });
            }
            var employee = await this.context.Employees.Where(employee => employee.Username == username).Select(employee => new
            {
                employee.FName,
                employee.LName,
                employee.Username,
                employee.Email,
                employee.IsAdmin
            }).FirstOrDefaultAsync();
            if (employee == null)
            {
                return NotFound(new { success = false, message = "Employee not found." });
            }
            return Ok(employee);
        }

        /// <summary>
        /// Checks if there is a session employee.
        /// </summary>
        /// <returns>True if there is a session employee, false otherwise</returns>
        private bool IsLoggedIn()
        {
            return ActiveEmployee.Employee != null;
        }

        /// <summary>
        /// Checks if the session user is logged in and is an admin.
        /// </summary>
        /// <returns>True if admin, false otherwise</returns>
        private bool IsAdmin()
        {
            if (!IsLoggedIn())
            {
                return false;
            }
            var adminStatus = ActiveEmployee.Employee.IsAdmin;
            return adminStatus.HasValue && adminStatus.Value;
        }
    }
}
