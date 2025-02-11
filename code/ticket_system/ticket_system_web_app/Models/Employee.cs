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

        public ICollection<Group> GroupsExistingIn {  get; set; } = new List<Group>();

        public Employee()
        {
            this.FName = string.Empty;
            this.LName = string.Empty;
            this.Username = string.Empty;
            this.HashedPassword = string.Empty;
        }
    
    
    }
}
