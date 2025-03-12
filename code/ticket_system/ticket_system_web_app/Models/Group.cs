using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// Group Class
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or sets the g identifier.
        /// </summary>
        /// <value>
        /// The g identifier.
        /// </value>
        [Key]
        [Column("g_id")]
        public int GId { get; set; }

        /// <summary>
        /// Gets or sets the manager identifier.
        /// </summary>
        /// <value>
        /// The manager identifier.
        /// </value>
        [Column("manager_id")]
        public int ManagerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the g.
        /// </summary>
        /// <value>
        /// The name of the g.
        /// </value>
        [Column("g_name")]
        public string? GName { get; set; }

        /// <summary>
        /// Gets or sets the g description.
        /// </summary>
        /// <value>
        /// The g description.
        /// </value>
        [Column("g_description")]
        public string? GDescription { get; set; }
        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        /// <value>
        /// The employees.
        /// </value>
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        /// <summary>
        /// Gets or sets the assigned projects.
        /// </summary>
        /// <value>
        /// The assigned projects.
        /// </value>
        public ICollection<Project> AssignedProjects { get; set; } = new List<Project>();

        private static string VALID_NUM_ERROR_MESSAGE = "Parameter must not be negative";
        private static string VALID_ERROR_MESSAGE = "Parameter must not be null or empty";


        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        public Group ()
        {
            this.GName = string.Empty;
            this.GDescription = string.Empty;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="managerId">The manager identifier.</param>
        /// <param name="gName">Name of the g.</param>
        /// <param name="gDescription">The g description.</param>
        public Group(int managerId, string? gName, string? gDescription)
        {
            this.handleValidationOfConstructorInputs(managerId, gName, gDescription);
            this.ManagerId = managerId;
            this.GName = gName;
            this.GDescription = gDescription;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="gId">The g identifier.</param>
        /// <param name="managerId">The manager identifier.</param>
        /// <param name="gName">Name of the g.</param>
        /// <param name="gDescription">The g description.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">gId</exception>
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
