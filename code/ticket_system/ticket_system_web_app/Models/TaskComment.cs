using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// TaskComment Class
    /// </summary>
    [Table("task_comment")]
    public class TaskComment
    {
        /// <summary>
        /// Gets or sets the comment ID.
        /// </summary>
        [Key]
        [Column("comment_id")]
        public int CommentId { get; set; }

        /// <summary>
        /// Gets or sets the task ID.
        /// </summary>
        [Column("task_id")]
        [Required]
        public int TaskId { get; set; }

        /// <summary>
        /// Gets or sets the commenter (employee) ID.
        /// </summary>
        [Column("commenter_id")]
        [Required]
        public int CommenterId { get; set; }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        [Column("comment_text")]
        [Required]
        public string CommentText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time the comment was made.
        /// </summary>
        [Column("commented_at")]
        [Required]
        public DateTime CommentedAt { get; set; }

        /// <summary>
        /// Navigation property for Task.
        /// </summary>
        [ForeignKey("TaskId")]
        public virtual ProjectTask? Task { get; set; }

        /// <summary>
        /// Navigation property for Employee (Commenter).
        /// </summary>
        [ForeignKey("CommenterId")]
        public virtual Employee? Commenter { get; set; }
    }
}
