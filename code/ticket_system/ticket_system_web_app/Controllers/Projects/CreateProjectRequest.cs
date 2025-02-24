using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Controllers.Projects
{
    public class CreateProjectRequest
    {
        public string? PTitle { get; set; }
        public string? PDescription { get; set; }
        public int PLeadId { get; set; }
        public ICollection<int> CollaboratingGroupIDs { get; set; } = new List<int>();
    }
}
