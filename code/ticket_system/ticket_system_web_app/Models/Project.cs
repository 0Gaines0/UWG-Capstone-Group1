using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Models
{
    public class Project
    {
        [Key]
        [Column("p_id")]
        public int PId { get; set; }

        [Column("project_lead_id")]
        public int ProjectLeadId { get; set; }

        [Column("p_title")]
        public string? PTitle { get; set; }

        [Column("p_description")]
        public string? PDescription { get; set; }


        public ICollection<Group> AssignedGroups { get; set; } = new List<Group>();

        public Project()
        {
            this.PTitle = string.Empty;
            this.PDescription = string.Empty;
        }


    }
}
