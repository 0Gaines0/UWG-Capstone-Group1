namespace ticket_system_web_app.Models.RequestObj
{
    public class EditEmployeeRequest
    {
        public string? OriginalUsername { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
