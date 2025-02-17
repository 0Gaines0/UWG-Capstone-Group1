using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Models
{
    public class Employee
    {
        [Key]
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

        public Employee()
        {
            this.FName = string.Empty;
            this.LName = string.Empty;
            this.Username = string.Empty;
            this.HashedPassword = string.Empty;
            this.Email = string.Empty;
        }

        public Employee(int eId, string? fName, string? lName, string? username, string? hashedPassword, bool? isActive, bool? isManager, bool? isAdmin, string? email)
        {
            this.EId = eId;
            this.FName = fName;
            this.LName = lName;
            this.Username = username;
            this.HashedPassword = hashedPassword;
            this.IsActive = isActive;
            this.IsManager = isManager;
            this.IsAdmin = isAdmin;
            this.Email = email;
        }
    }
}
