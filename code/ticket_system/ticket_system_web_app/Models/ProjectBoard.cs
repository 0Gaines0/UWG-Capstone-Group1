using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// ProjectBoard
    /// </summary>
    public class ProjectBoard
    {
        /// <summary>
        /// Gets or sets the board identifier.
        /// </summary>
        /// <value>
        /// The board identifier.
        /// </value>
        [Key]
        [Column("board_id")]
        public int BoardId { get; set; }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        [Column("p_id")]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }

        /// <summary>
        /// Gets or sets the states.
        /// </summary>
        /// <value>
        /// The states.
        /// </value>
        public List<BoardState> States { get; set; } = new List<BoardState>();
    }
}
