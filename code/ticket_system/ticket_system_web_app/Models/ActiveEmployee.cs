namespace ticket_system_web_app.Models
{
    public class ActiveEmployee
    {
        public static Employee? Employee { get; set; }

        public ActiveEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "the parameter must not be null");
            }
            Employee = employee;
        }

        public void LogoutCurrentEmployee()
        {
            Employee = null;
        }

    }
}
