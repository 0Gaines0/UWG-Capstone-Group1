using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticket_system_web_app.Models
{
    public class ProjectBoard
    {
        [Key]
        [Column("board_id")]
        public int BoardId { get; set; }

        [Column("p_id")]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }

        public List<BoardState> States { get; set; } = new List<BoardState>();
    }
}
