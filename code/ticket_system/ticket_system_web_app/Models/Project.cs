using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Build.Evaluation;

namespace ticket_system_web_app.Models
{
    /// <summary>Project class</summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets the p identifier.
        /// </summary>
        /// <value>
        /// The p identifier.
        /// </value>
        [Key]
        [Column("p_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PId { get; set; }

        /// <summary>
        /// Gets or sets the project lead identifier.
        /// </summary>
        /// <value>
        /// The project lead identifier.
        /// </value>
        [Column("project_lead_id")]
        public int ProjectLeadId { get; set; }

        /// <summary>
        /// Gets or sets the project lead.
        /// </summary>
        /// <value>
        /// The project lead.
        /// </value>
        [BindNever]
        [Column("project_lead")]
        public Employee? ProjectLead { get; set; }

        /// <summary>
        /// Gets or sets the p title.
        /// </summary>
        /// <value>
        /// The p title.
        /// </value>
        [Column("p_title")]
        public string PTitle { get; set; }

        /// <summary>
        /// Gets or sets the p description.
        /// </summary>
        /// <value>
        /// The p description.
        /// </value>
        [Column("p_description")]
        public string PDescription { get; set; }

        /// <summary>
        /// Gets or sets the assigned groups.
        /// </summary>
        /// <value>
        /// The assigned groups.
        /// </value>
        public ICollection<ProjectGroup> Collaborators { get; set; }

        public ProjectBoard? ProjectBoard { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            this.PTitle = string.Empty;
            this.PDescription = string.Empty;
            this.Collaborators = new List<ProjectGroup>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class with the specified values.
        /// </summary>
        /// <param name="pLead">The project lead.</param>
        /// <param name="pTitle">The project title.</param>
        /// <param name="pDesc">The project desc.</param>
        /// <exception cref="ArgumentException">
        /// Project lead ID must be positive.
        /// or
        /// Title cannot be null or blank.
        /// or
        /// Description cannot be null or blank.
        /// </exception>
        public Project(Employee pLead, string pTitle, string pDesc)
        {
            if (pLead == null)
            {
                throw new ArgumentNullException(nameof(pLead), "Project lead cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(pTitle))
            {
                throw new ArgumentException("Title cannot be null or blank.", nameof(pTitle));
            }
            if (string.IsNullOrEmpty(pDesc))
            {
                throw new ArgumentException("Description cannot be null or blank.", nameof(pDesc));
            }

            this.ProjectLead = pLead;
            this.ProjectLeadId = pLead.EId;
            this.PTitle = pTitle;
            this.PDescription = pDesc;
            this.Collaborators = new List<ProjectGroup>();
        }


    }
}
