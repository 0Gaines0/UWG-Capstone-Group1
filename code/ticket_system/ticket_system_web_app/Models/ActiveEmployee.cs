namespace ticket_system_web_app.Models
{
    /// <summary>
    /// ActiveEmployee class
    /// </summary>
    public static class ActiveEmployee
    {
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
        public static void LogInEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "input must not be null");
            }
            Employee = employee;
        }

        /// <summary>
        /// Logouts the current employee.
        /// </summary>
        public static void LogoutCurrentEmployee()
        {
            Employee = null;
        }

    }
}
