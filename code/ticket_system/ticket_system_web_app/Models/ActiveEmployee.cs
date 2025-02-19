namespace ticket_system_web_app.Models
{
    public static class ActiveEmployee
    {
        public static Employee? Employee { get; set; }

        public static void LogInEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "input must not be null");
            }
            Employee = employee;
        }
        public static void LogoutCurrentEmployee()
        {
            Employee = null;
        }

    }
}
