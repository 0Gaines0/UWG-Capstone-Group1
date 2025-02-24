using Microsoft.AspNetCore.Mvc;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;

namespace ticket_system_web_app.Controllers
{
    public class LoginController : Controller
    {
        private static string VALIDATE_PARAMETER_MESSAGE = "input must not be null or empty";
        private static string INVALID_ENTERED_CREDENTIALS = "credentials entered are invalid";
        private readonly TicketSystemDbContext context;
       
        public LoginController(TicketSystemDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), VALIDATE_PARAMETER_MESSAGE);
            }
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
           if (string.IsNullOrEmpty(username))
            {
                TempData["UsernameError"] = nameof(username) + " " + VALIDATE_PARAMETER_MESSAGE;
                return RedirectToAction("Index");
            } 
           else
            {
                TempData["EnteredUsername"] = username;
            }
            if (string.IsNullOrEmpty(password))
            {
                TempData["PasswordError"] = nameof(password) + " " + VALIDATE_PARAMETER_MESSAGE;
                return RedirectToAction("Index");
            }

            var user = this.context.Employees.FirstOrDefault(employee => employee.Username != null && employee.Username.Equals(username));
            var userPassword = user?.HashedPassword;
            if (user == null || userPassword == null || !this.verifyCorrectPassword(password, userPassword))
            {
                TempData["UsernameError"] = INVALID_ENTERED_CREDENTIALS;
                return RedirectToAction("Index");
            }

            TempData["EnteredUsername"] = string.Empty;
            ActiveEmployee.LogInEmployee(user);
            return RedirectToAction("Index", "LandingPage");
        }

        private bool verifyCorrectPassword(string passwordEntered, string hashedPassword) 
        {
            //String password = "password";
            //String newPass = BCrypt.Net.BCrypt.HashPassword("password", BCrypt.Net.BCrypt.GenerateSalt());
            //Boolean test = BCrypt.Net.BCrypt.Verify("password", newPass);
            // password = $2a$11$ra.0VCrMj1kDeCUbQxysner1OH4HOX9EWwWoeoJAL6n.62szpw4q6
            return BCrypt.Net.BCrypt.Verify(passwordEntered, hashedPassword);
        }


    }
}
