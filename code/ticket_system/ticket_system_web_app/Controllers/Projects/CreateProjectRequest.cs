using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ticket_system_web_app.Controllers.Projects
{
    /// <summary>
    /// CreateProjectRequest
    /// </summary>
    public class CreateProjectRequest
    {
        /// <summary>
        /// Gets or sets the p title.
        /// </summary>
        /// <value>
        /// The p title.
        /// </value>
        public string? PTitle { get; set; }
        /// <summary>
        /// Gets or sets the p description.
        /// </summary>
        /// <value>
        /// The p description.
        /// </value>
        public string? PDescription { get; set; }
        /// <summary>
        /// Gets or sets the p lead identifier.
        /// </summary>
        /// <value>
        /// The p lead identifier.
        /// </value>
        public int PLeadId { get; set; }
        /// <summary>
        /// Gets or sets the collaborating group i ds.
        /// </summary>
        /// <value>
        /// The collaborating group i ds.
        /// </value>
        public ICollection<int> CollaboratingGroupIDs { get; set; } = new List<int>();
    }
}
