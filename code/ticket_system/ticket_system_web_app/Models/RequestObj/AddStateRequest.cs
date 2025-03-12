namespace ticket_system_web_app.Models.RequestObj
{
    /// <summary>
    /// AddStateRequest
    /// </summary>
    public class AddStateRequest
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the board identifier.
        /// </summary>
        /// <value>
        /// The board identifier.
        /// </value>
        public int BoardId { get; set; }
    }
}
