using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Models
{
    public class Group
    {
        [Key]
        [Column("g_id")]
        public int GId { get; set; }

        [Column("manager_id")]
        public int ManagerId { get; set; }

        [Column("g_name")]
        public string? GName { get; set; }

        [Column("g_description")]
        public string? GDescription { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Project> AssignedProjects { get; set; } = new List<Project>();

        public Group ()
        {
            this.GName = string.Empty;
            this.GDescription = string.Empty;

        }


    }
}
