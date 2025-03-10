namespace ticket_system_web_app.Models.RequestObj
{
    /// <summary>
    /// UpdateBoardStateO
    /// </summary>
    public class UpdateBoardStateOrderRequest
    {
        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        /// <value>
        /// The state identifier.
        /// </value>
        public int StateId { get; set; }
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int Position { get; set; }
    }

}
