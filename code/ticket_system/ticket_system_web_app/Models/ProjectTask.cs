using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// Task Class
    /// </summary>
    [Table("task")]
    public class ProjectTask
    {
        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        [Key]
        [Column("task_id")]
        public int TaskId { get; set; }

        /// <summary>
        /// Gets or sets the task description.
        /// </summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the task priority.
        /// </summary>
        [Column("priority")]
        [Required]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        [Column("createdDate")]
        [Required]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        [Column("state_id")]
        [Required]
        public int? StateId { get; set; }

        /// <summary>
        /// Gets or sets the assignee identifier.
        /// </summary>
        [Column("assignee_id")]
        [Required]
        public int? AssigneeId { get; set; }

        /// <summary>
        /// Navigation property for BoardState
        /// </summary>
        [ForeignKey("StateId")]
        public virtual BoardState? BoardState { get; set; }

        /// <summary>
        /// Navigation property for Employee
        /// </summary>
        [ForeignKey("AssigneeId")]
        public virtual Employee? Assignee { get; set; }
    }
}
