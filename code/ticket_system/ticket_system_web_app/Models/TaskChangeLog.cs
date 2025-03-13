using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Models
{
    /// <summary>
    /// TaskChangeLog Class
    /// </summary>
    public class TaskChangeLog
    {
        [Key]
        [Column("task_id")]
        public int TaskId { get; set; }

        [Column("change_id")]
        public int ChangeId { get; set; }

        [ForeignKey("TaskId")]
        public virtual ProjectTask? Task { get; set; }

        [ForeignKey("ChangeId")]
        public virtual TaskChange? TaskChange { get; set; }
    }

}
