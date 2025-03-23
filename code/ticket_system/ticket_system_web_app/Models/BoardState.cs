using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// BoardState class
    /// </summary>
    public class BoardState
    {

        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        /// <value>
        /// The state identifier.
        /// </value>
        [Key]
        [Column("state_id")]
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the board identifier.
        /// </summary>
        /// <value>
        /// The board identifier.
        /// </value>
        [Column("board_id")]
        public int BoardId { get; set; }

        /// <summary>
        /// Gets or sets the name of the state.
        /// </summary>
        /// <value>
        /// The name of the state.
        /// </value>
        [Column("state_name")]
        public string? StateName { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [Column("position")]
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the project board.
        /// </summary>
        /// <value>
        /// The project board.
        /// </value>
        [ForeignKey("BoardId")]
        public ProjectBoard? ProjectBoard { get; set; }

        public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();

    }
}
