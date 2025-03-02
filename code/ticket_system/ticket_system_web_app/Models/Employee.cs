using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("e_id")] 
        public int EId { get; set; }

        [Column("f_name")]
        public string? FName { get; set; }

        [Column("l_name")]
        public string? LName { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("hashed_password")]
        public string? HashedPassword { get; set; }

        [Column("is_active")]
        public bool? IsActive { get; set; }

        [Column("is_manager")]
        public bool? IsManager { get; set; }

        [Column("is_admin")]
        public bool? IsAdmin { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        public ICollection<Group> GroupsExistingIn {  get; set; } = new List<Group>();
        public ICollection<Project> ProjectsLeading { get; set; } = new List<Project>();

        private static string VALID_ERROR_MESSAGE = "Parameter must not be null or empty";

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
        public Employee(int eId = 0, string? fName = "", string? lName = "", string? username = "", string? hashedPassword = "", bool? isActive = false, bool? isManager = false, bool? isAdmin = false, string? email = "")
        {
            this.handleValidationOfConstructorInputs(eId, fName, lName, username, hashedPassword, isActive, isManager, isAdmin, email);
            if (eId != 0)
            {
                EId = eId;
            }
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
