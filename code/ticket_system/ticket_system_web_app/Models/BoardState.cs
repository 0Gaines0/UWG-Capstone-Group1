using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// BoardState class
    /// </summary>
    public class BoardState
    {
        [Key]
        [Column("state_id")]
        public int StateId { get; set; }

        [Column("board_id")]
        public int BoardId { get; set; }

        [Column("state_name")]
        public string? StateName { get; set; }

        [Column("position")]
        public int Position { get; set; }

        [ForeignKey("BoardId")]
        public ProjectBoard? ProjectBoard { get; set; }

        public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();

        public List<StateAssignedGroup> AssignedGroups { get; set; } = new List<StateAssignedGroup>();

    }
}
