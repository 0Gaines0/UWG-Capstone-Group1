using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

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

        private static string VALID_NUM_ERROR_MESSAGE = "Parameter must not be negative";
        private static string VALID_ERROR_MESSAGE = "Parameter must not be null or empty";


        public Group ()
        {
            this.GName = string.Empty;
            this.GDescription = string.Empty;

        }

        public Group(int managerId, string? gName, string? gDescription)
        {
            this.handleValidationOfConstructorInputs(managerId, gName, gDescription);
            this.ManagerId = managerId;
            this.GName = gName;
            this.GDescription = gDescription;
        }

        public Group (int gId, int managerId, string? gName, string? gDescription) : this(managerId, gName, gDescription) 
        {
            if (gId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gId), VALID_NUM_ERROR_MESSAGE);
            }
            this.GId = gId;
        }

        private void handleValidationOfConstructorInputs(int managerId, string? gName, string? gDescription)
        {
            if (managerId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(managerId), VALID_NUM_ERROR_MESSAGE);
            }
            if (string.IsNullOrEmpty(gName))
            {
                throw new ArgumentException(VALID_ERROR_MESSAGE, nameof(gName));
            }
        }


    }
}
