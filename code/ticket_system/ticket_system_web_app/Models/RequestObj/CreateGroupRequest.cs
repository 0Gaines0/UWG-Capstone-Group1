namespace ticket_system_web_app.Models.RequestObj
{
    /// <summary>
    /// CreateGroupRequest class
    /// </summary>
    public class CreateGroupRequest
    {
        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        public int GroupId { get; set; }
        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public string? GroupName { get; set; }
        /// <summary>
        /// Gets or sets the group description.
        /// </summary>
        /// <value>
        /// The group description.
        /// </value>
        public string? GroupDescription { get; set; }
        /// <summary>
        /// Gets or sets the manager identifier.
        /// </summary>
        /// <value>
        /// The manager identifier.
        /// </value>
        public int ManagerId { get; set; }
        /// <summary>
        /// Gets or sets the member ids.
        /// </summary>
        /// <value>
        /// The member ids.
        /// </value>
        public List<int> MemberIds { get; set; } = new List<int>();
    }
}
