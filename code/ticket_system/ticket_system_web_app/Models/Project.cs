﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace ticket_system_web_app.Models
{
    public class Project
    {
        [Key]
        [Column("p_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PId { get; set; }

        [Column("project_lead_id")]
        public int ProjectLeadId { get; set; }

        [Column("p_title")]
        public string PTitle { get; set; }

        [Column("p_description")]
        public string PDescription { get; set; }

        public ICollection<Group> AssignedGroups { get; set; } = new List<Group>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            this.PTitle = string.Empty;
            this.PDescription = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class with the specified values.
        /// </summary>
        /// <param name="pLeadID">The project lead identifier.</param>
        /// <param name="pTitle">The project title.</param>
        /// <param name="pDesc">The project desc.</param>
        /// <exception cref="ArgumentException">
        /// Project lead ID must be positive.
        /// or
        /// Title cannot be null or blank.
        /// or
        /// Description cannot be null or blank.
        /// </exception>
        public Project(int pLeadID, string pTitle, string pDesc, ICollection<Group> assignedGroups)
        {
            if (pLeadID <= 0)
            {
                throw new ArgumentException("Project lead ID must be positive.");
            }
            if (string.IsNullOrWhiteSpace(pTitle))
            {
                throw new ArgumentException("Title cannot be null or blank.");
            }
            if (string.IsNullOrEmpty(pDesc))
            {
                throw new ArgumentException("Description cannot be null or blank.");
            }
            if (assignedGroups.IsNullOrEmpty())
            {
                throw new ArgumentException("Must have at least one assigned group.");
            }

            this.ProjectLeadId = pLeadID;
            this.PTitle = pTitle;
            this.PDescription = pDesc;
            this.AssignedGroups = assignedGroups;
        }


    }
}
