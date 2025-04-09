namespace ticket_system_web_app.Models
{
    public class ProjectGroup
    {
        public int ProjectId { get; set; }
        public int GroupId { get; set; }

        public Project? Project { get; set; }
        public Group? Group { get; set; }

        public bool Accepted { get; set; }

        public ProjectGroup() {}

        public ProjectGroup(Project project, Group group)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Parameter must not be null");
            }
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group), "Parameter must not be null");
            }


            this.Project = project;
            this.Group = group;
            this.ProjectId = project.PId;
            this.GroupId = group.GId;
            this.Accepted = false;
        }
    }
}
