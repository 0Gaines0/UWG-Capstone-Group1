using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// Employee Class
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the e identifier.
        /// </summary>
        /// <value>
        /// The e identifier.
        /// </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("e_id")] 
        public int EId { get; set; }

        /// <summary>
        /// Gets or sets the name of the f.
        /// </summary>
        /// <value>
        /// The name of the f.
        /// </value>
        [Column("f_name")]
        public string? FName { get; set; }

        /// <summary>
        /// Gets or sets the name of the l.
        /// </summary>
        /// <value>
        /// The name of the l.
        /// </value>
        [Column("l_name")]
        public string? LName { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [Column("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the hashed password.
        /// </summary>
        /// <value>
        /// The hashed password.
        /// </value>
        [Column("hashed_password")]
        public string? HashedPassword { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>
        /// The is active.
        /// </value>
        [Column("is_active")]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the is manager.
        /// </summary>
        /// <value>
        /// The is manager.
        /// </value>
        [Column("is_manager")]
        public bool? IsManager { get; set; }

        /// <summary>
        /// Gets or sets the is admin.
        /// </summary>
        /// <value>
        /// The is admin.
        /// </value>
        [Column("is_admin")]
        public bool? IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Column("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the groups existing in.
        /// </summary>
        /// <value>
        /// The groups existing in.
        /// </value>
        public virtual ICollection<Group> GroupsExistingIn {  get; set; } = new List<Group>();
        /// <summary>
        /// Gets or sets the projects leading.
        /// </summary>
        /// <value>
        /// The projects leading.
        /// </value>
        public virtual ICollection<Project> ProjectsLeading { get; set; } = new List<Project>();

        /// <summary>
        /// Gets or sets the managed groups.
        /// </summary>
        /// <value>
        /// The managed groups.
        /// </value>
        public virtual ICollection<Group> ManagedGroups { get; set; } = new List<Group>();


        private static string VALID_ERROR_MESSAGE = "Parameter must not be null or empty";

        /// <summary>
        /// Initializes a new instance of the <see cref="Employee"/> class.
        /// </summary>
        public Employee()
        {
            this.FName = string.Empty;
            this.LName = string.Empty;
            this.Username = string.Empty;
            this.HashedPassword = string.Empty;
            this.Email = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employee"/> class.
        /// </summary>
        /// <param name="eId">The e identifier.</param>
        /// <param name="fName">Name of the f.</param>
        /// <param name="lName">Name of the l.</param>
        /// <param name="username">The username.</param>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="isActive">The is active.</param>
        /// <param name="isManager">The is manager.</param>
        /// <param name="isAdmin">The is admin.</param>
        /// <param name="email">The email.</param>
        public Employee(int eId, string? fName, string? lName, string? username, string? hashedPassword, bool? isActive, bool? isManager, bool? isAdmin, string? email)
        {
            this.handleValidationOfConstructorInputs(eId, fName, lName, username, hashedPassword, isActive, isManager, isAdmin, email);
            EId = eId;
            this.FName = fName;
            this.LName = lName;
            this.Username = username;
            this.HashedPassword = hashedPassword;
            this.IsActive = isActive;
            this.IsManager = isManager;
            this.IsAdmin = isAdmin;
            this.Email = email;
        }

        private void handleValidationOfConstructorInputs(int eId, string? fName, string? lName, string? username, string? hashedPassword, bool? isActive, bool? isManager, bool? isAdmin, string? email)
        {
            if (eId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(eId), "Parameter must not be negative");
            }
            if (string.IsNullOrEmpty(fName))
            {
                throw new ArgumentException(VALID_ERROR_MESSAGE, nameof(fName));
            }
            if (string.IsNullOrEmpty(lName))
            {
                throw new ArgumentException(VALID_ERROR_MESSAGE, nameof(lName));
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException(VALID_ERROR_MESSAGE, nameof(username));
            }
            if (string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentException(VALID_ERROR_MESSAGE, nameof(hashedPassword));
            }
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException(VALID_ERROR_MESSAGE, nameof(email));
            }
            if (isActive == null)
            {
                throw new ArgumentException(VALID_ERROR_MESSAGE, nameof(isActive));
            }
            if (isManager == null)
            {
                throw new ArgumentException(VALID_ERROR_MESSAGE, nameof(isManager));
            }
            if (isAdmin == null)
            {
                throw new ArgumentException(VALID_ERROR_MESSAGE, nameof(isAdmin));
            }
        }
    }
}
