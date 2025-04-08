namespace ticket_system_web_app.Models.RequestObj
{
    public class GroupAssignmentRequest
    {
        public int StateId { get; set; }
        public List<int>? GroupIds { get; set; }
    }
}
