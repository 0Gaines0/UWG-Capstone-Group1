namespace ticket_system_web_app.Models
{
    public class StateAssignedGroup
    {
        public int StateId { get; set; }
        public int GroupId { get; set; }

        public BoardState? BoardState { get; set; }
        public Group? Group { get; set; }

        public StateAssignedGroup() { } 

        public StateAssignedGroup(int stateId, int groupId, BoardState? boardState, Group? group)
        {
            StateId = stateId;
            GroupId = groupId;
            BoardState = boardState;
            Group = group;
        }
    }
}
