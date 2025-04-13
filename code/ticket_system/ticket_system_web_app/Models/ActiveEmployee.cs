using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ticket_system_web_app.Data;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// ActiveEmployee class
    /// </summary>
    public static class ActiveEmployee
    {
        private const int AUTH_TOKEN_LENGTH = 32;
        private const string AUTH_TOKEN_VALID_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";

        private static bool hasManagerPerms = false;

        /// <summary>
        /// Gets the authentication token.
        /// </summary>
        /// <value>
        /// The authentication token.
        /// </value>
        public static string AuthToken { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the employee.
        /// </summary>
        /// <value>
        /// The employee.
        /// </value>
        public static Employee? Employee { get; set; }

        /// <summary>
        /// Logs the in employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <exception cref="System.ArgumentNullException">employee - input must not be null</exception>
        public static void LogInEmployee(Employee employee, TicketSystemDbContext context)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "input must not be null");
            }
            if (context == null)
            {
                throw new ArgumentNullException(nameof(employee), "input must not be null");
            }

            Employee = employee;
            hasManagerPerms = context.Groups.Count(group => group.ManagerId == Employee.EId) > 0;
            generateAuthToken(new Random());
        }

        /// <summary>
        /// Logouts the current employee.
        /// </summary>
        public static void LogoutCurrentEmployee()
        {
            Employee = null;
            AuthToken = string.Empty;
        }

        /// <summary>
        /// Determines whether the current user is logged in.
        /// </summary>
        /// <precondition>true</precondition>
        /// <postcondition>true</postcondition>
        /// <returns>
        ///   <c>true</c> if logged in; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLoggedIn()
        {
            return Employee != null;
        }

        /// <summary>
        /// Determines whether the current user is a manager in the specified context.
        /// Admins also have manager permissions.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if they are a manager; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsManager()
        {
            return IsAdmin() || hasManagerPerms;
        }

        /// <summary>
        /// Determines whether the current user is an admin.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is admin; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAdmin()
        {
            return IsLoggedIn() && (Employee.IsAdmin ?? false);
        }

        /// <summary>
        /// Verifies the validity of a request based on the specified token.
        /// </summary>
        /// <precondition>true</precondition>
        /// <postcondition>true</postcondition>
        /// <param name="authToken">The authentication token.</param>
        /// <returns><c>True</c> if the request is valid, <c>false</c> otherwise.</returns>
        public static bool IsValidRequest(string authToken)
        {
            return IsLoggedIn() && !authToken.IsNullOrEmpty() && AuthToken.Equals(authToken);
        }

        private static void generateAuthToken(Random rng)
        {
            string result = string.Empty;
            for (int i = 0; i < AUTH_TOKEN_LENGTH; i++)
            {
                int charIndex = rng.Next(AUTH_TOKEN_VALID_CHARS.Length - 1);
                result += AUTH_TOKEN_VALID_CHARS.Substring(charIndex, 1);
            }
            AuthToken = result;
            Console.WriteLine(result);
        }
    }
}
