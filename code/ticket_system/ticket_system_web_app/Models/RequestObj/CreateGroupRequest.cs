namespace ticket_system_web_app.Models.RequestObj
{
    public class CreateGroupRequest
    {
        public string? GroupName { get; set; }
        public string? GroupDescription { get; set; }
        public int ManagerId { get; set; }
        public List<int> MemberIds { get; set; } = new List<int>();
    }
}
