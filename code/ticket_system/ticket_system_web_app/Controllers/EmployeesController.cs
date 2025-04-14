using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_web_app.Controllers
{
    /// <summary>
    /// The controller/server class for the manage employees page.
    /// All client method calls require auth token validation.
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

        #region View Loaders

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

        #region Authenticated Methods

        /// <summary>
        ///     Returns the following information, in a Json object, for all employees:
        ///     Id, full name, username, email address, and admin status.
        ///     Requires admin perms.
        /// </summary>
        /// <precondition>true</precondition>
        /// <param name="authToken">The auth token.</param>
        /// <returns>A Json object of all the employees, or a Json with an error message if request is invalid.</returns>
        [HttpGet("Employees/GetAllEmployees/{authToken}")]
        public async Task<JsonResult> GetAllEmployees(string authToken)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(GetAllEmployees)} Got auth token: {authToken}");
                return Json(new { message = "Not logged in." });
            }
            if (!ActiveEmployee.IsManager())
            {
                return Json(new { message = "Manager permissions required." });
            }

            var employees = await this.constructEmployees();
            return Json(employees);
        }

        /// <summary>
        ///     Creates the employee with the given information if it doesn't exist already.
        ///     Requires admin perms.
        /// </summary>
        /// <precondition>true</precondition>
        /// <param name="authToken">The auth token.</param>
        /// <param name="jsonRequest">The new employee information</param>
        /// <returns>Ok if employee was created, BadRequest otherwise</returns>
        [HttpPost("Employees/CreateEmployee/{authToken}")]
        public async Task<IActionResult> CreateEmployee(string authToken, [FromBody] CreateEmployeeRequest jsonRequest)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(CreateEmployee)} Got auth token: {authToken}");
                return BadRequest(new { success = false, message = "Not logged in." });
            }
            if (!ActiveEmployee.IsAdmin())
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
        ///     Removes the given employee if they are not the current user and don't manage a group.
        ///     Requires admin perms.
        /// </summary>
        /// <precondition>true</precondition>
        /// <param name="authToken">The auth token.</param>
        /// <param name="request">The employee to be deleted username</param>
        /// <returns>Ok if employee was removed, BadRequest otherwise</returns>
        [HttpPost("Employees/RemoveEmployee/{authToken}")]
        public async Task<IActionResult> RemoveEmployee(string authToken, [FromBody] RemoveEmployeeRequest request)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(RemoveEmployee)} Got auth token: {authToken}");
                return BadRequest(new { success = false, message = "Not logged in." });
            }
            if (!ActiveEmployee.IsAdmin())
            {
                return BadRequest(new { success = false, message = "Admin permissions required." });
            }

            if (request == null || string.IsNullOrWhiteSpace(request.username))
            {
                return BadRequest(new { message = "Invalid request data" });
            }
            string userName = request.username;
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

            await this.removeEmployeeFromDb(userName);
            return Ok(new { success = true });
        }

        /// <summary>
        ///     Edits the given employee's data.
        ///     Requires admin perms.
        /// </summary>
        /// <precondition>true</precondition>
        /// <param name="authToken">The auth token.</param>
        /// <param name="jsonRequest">The employee username and updated employee information</param>
        /// <returns>Ok if successful, BadRequest otherwise</returns>
        [HttpPost("Employees/EditEmployee/{authToken}")]
        public async Task<IActionResult> EditEmployee(string authToken, [FromBody] EditEmployeeRequest jsonRequest)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(EditEmployee)} Got auth token: {authToken}");
                return BadRequest(new { success = false, message = "Not logged in." });
            }
            if (!ActiveEmployee.IsAdmin())
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

            return Ok(new { message = "Employee created successfully" });
        }

        /// <summary>
        ///     Returns the data for the employee with the given username.
        ///     Requires admin perms.
        /// </summary>
        /// <precondition>true</precondition>
        /// <param name="authToken">The auth token.</param>
        /// <param name="data">The employee's username</param>
        /// <returns>
        ///     If authToken or data are invalid, BadRequest.
        ///     If the employee is not found, NotFound.
        ///     Otherwise, returns the employee data for this username
        /// </returns>
        [HttpPost("Employees/GetEmployee/{authToken}")]
        public async Task<IActionResult> GetEmployee(string authToken, [FromBody] GetEmployeeRequest data)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(GetEmployee)} Got auth token: {authToken}");
                return BadRequest(new { success = false, message = "Not logged in." });
            }
            if (!ActiveEmployee.IsAdmin())
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

        #region Helpers

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
        
        private async Task<bool> removeEmployeeFromDb(string username)
        {
            var employee = await this.context.Employees.FirstOrDefaultAsync(currentEmployee => currentEmployee.Username == username);
            this.context.Employees.Remove(employee);
            await this.context.SaveChangesAsync();
            return true;
        }       

        #endregion
    }
}
