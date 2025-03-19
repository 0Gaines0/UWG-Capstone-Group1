using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// TaskChange Class
    /// </summary>
    public class TaskChange
    {
        /// <summary>
        /// Gets or sets the change identifier.
        /// </summary>
        [Key]
        [Column("change_id")]
        public int ChangeId { get; set; }

        /// <summary>
        /// Gets or sets the type of change.
        /// </summary>
        [Column("type")]
        [Required]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of the change.
        /// </summary>
        [Column("changed_date")]
        [Required]
        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the previous value.
        /// </summary>
        [Column("previous_value")]
        public string? PreviousValue { get; set; }

        /// <summary>
        /// Gets or sets the new value.
        /// </summary>
        [Column("new_value")]
        [Required]
        public string NewValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the assignee identifier.
        /// </summary>
        [Column("assignee_id")]
        [Required]
        public int? AssigneeId { get; set; }

        /// <summary>
        /// Navigation property for Employee
        /// </summary>
        [ForeignKey("AssigneeId")]
        public virtual Employee? Assignee { get; set; }
    }
}
