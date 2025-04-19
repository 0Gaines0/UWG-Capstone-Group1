using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Models
{
    public class StateAssignedGroup
    {
        [Key, Column(Order = 0)]
        public int StateId { get; set; }

        [Key, Column(Order = 1)]
        public int GroupId { get; set; }

        [ForeignKey("StateId")]
        public virtual BoardState? BoardState { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group? Group { get; set; }
    }
}
