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
        #region Fields
        
        private readonly TicketSystemDbContext context;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <precondition>context != null</precondition>
        /// <param name="context">The DB context.</param>
        public EmployeesController(TicketSystemDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Parameter cannot be null");
            }
            this.context = context;
        }

        #endregion

        #region Page Loaders

        /// <summary>
        /// Loads the Employees homepage.
        /// </summary>
        /// <precondition>true</precondition>
        /// <returns>the homepage</returns>
        public async Task<ViewResult> Index()
        {
            var employees = await this.constructEmployees();
            return View(employees);
        }

        /// <summary>
        /// Loads the Employee creation modal.
        /// </summary>
        /// <precondition>true</precondition>
        /// <returns>the modal</returns>
        public IActionResult CreateEmployeeModal()
        {
            return PartialView("_CreateEmployeeModal");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of all the employees in a Json object.
        /// </summary>
        /// <precondition>true</precondition>
        /// <returns>A Json object of all the employees</returns>
        [HttpGet]
        public async Task<JsonResult> GetAllEmployees()
        {
            var employees = await this.constructEmployees();
            return Json(employees);
        }

        /// <summary>
        /// Creates the employee with the given information if it doesn't exist already.
        /// </summary>
        /// <precondition>true</precondition>
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

            var newUser = new Employee(eId: 0, fName: jsonRequest.FirstName, lName: jsonRequest.LastName, username: jsonRequest.Username, hashedPassword: hashedPassword, email: jsonRequest.Email, isAdmin: jsonRequest.IsAdmin, isActive: true, isManager: false);

            this.context.Employees.Add(newUser);
            await this.context.SaveChangesAsync();

            return Ok(new { message = "Employee created successfully", employeeId = newUser.EId });
        }

        /// <summary>
        /// Removes the given employee if they are not the current user and don't manage a group.
        /// </summary>
        /// <precondition>true</precondition>
        /// <param name="request">The username of the employee to be deleted</param>
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
        /// <precondition>true</precondition>
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
        /// Returns the data for the employee with the given username.
        /// </summary>
        /// <precondition>true</precondition>
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

        #endregion

        #region Helper Methods

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

        #endregion

        private bool IsLoggedIn()
        {
            return ActiveEmployee.Employee == null;
        }

        private bool IsAdmin()
        {
            return ActiveEmployee.Employee.IsAdmin ?? false;
        }
    }
}
