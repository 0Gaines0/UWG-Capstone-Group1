namespace ticket_system_web_app.Models.RequestObj
{
    public class EditProjectTaskRequest
    {
        public int TaskId { get; set; }
        public int StateId { get; set; }
        public int Priority { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public int? AssigneeId { get; set; }
    }
}
