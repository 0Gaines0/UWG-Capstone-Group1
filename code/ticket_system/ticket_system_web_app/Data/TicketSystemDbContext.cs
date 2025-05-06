using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection.Metadata;
using ticket_system_web_app.Models;

namespace ticket_system_web_app.Data
{
    public class TicketSystemDbContext : DbContext
    {

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public DbSet<Models.ProjectTask> Tasks { get; set; }
        /// <summary>
        /// Gets or sets the task changes.
        /// </summary>
        /// <value>
        /// The task changes.
        /// </value>
        public DbSet<TaskChange> TaskChanges { get; set; }

        /// <summary>
        /// Gets or sets the task change logs.
        /// </summary>
        /// <value>
        /// The task change logs.
        /// </value>
        public DbSet<TaskChangeLog> TaskChangeLogs { get; set; }
        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        /// <value>
        /// The employees.
        /// </value>
        public DbSet<Employee> Employees { get; set; }
        /// <summary>
        /// Gets or sets the projects.
        /// </summary>
        /// <value>
        /// The projects.
        /// </value>
        public DbSet<Project> Projects { get; set; }
        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        /// <value>
        /// The groups.
        /// </value>
        public DbSet<Group> Groups { get; set; }

        public DbSet<ProjectGroup> ProjectGroups { get; set; }

        /// <summary>
        /// Gets or sets the project boards.
        /// </summary>
        /// <value>
        /// The project boards.
        /// </value>
        public DbSet<ProjectBoard> ProjectBoards { get; set; }

        /// <summary>
        /// Gets or sets the board states.
        /// </summary>
        /// <value>
        /// The board states.
        /// </value>
        public DbSet<BoardState> BoardStates { get; set; }

        /// <summary>
        /// Gets or sets the state assigned groups.
        /// </summary>
        /// <value>
        /// The state assigned groups.
        /// </value>
        public DbSet<StateAssignedGroup> StateAssignedGroups { get; set; }

        /// <summary>
        /// Gets or sets the task comments.
        /// </summary>
        /// <value>
        /// The task comments.
        /// </value>
        public DbSet<TaskComment> TaskComments { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TicketSystemDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <remarks>
        /// See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see> and
        /// <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see> for more information and examples.
        /// </remarks>
        public TicketSystemDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasMany(employee => employee.GroupsExistingIn).WithMany(group => group.Employees).UsingEntity(join => join.ToTable("group_member"));
            //modelBuilder.Entity<Project>().HasMany(project => project.AssignedGroups).WithMany(group => group.AssignedProjects).UsingEntity(join => join.ToTable("project_group"));
            modelBuilder.Entity<Project>().HasOne(e => e.ProjectLead).WithMany(e => e.ProjectsLeading).HasForeignKey(e => e.ProjectLeadId).IsRequired();

            modelBuilder.Entity<ProjectGroup>().HasKey(collab => new { collab.ProjectId, collab.GroupId });
            modelBuilder.Entity<ProjectGroup>().HasOne(pg => pg.Project).WithMany(project => project.Collaborators).HasForeignKey(collab => collab.ProjectId);
            modelBuilder.Entity<ProjectGroup>().HasOne(pg => pg.Group).WithMany(group => group.Collaborations).HasForeignKey(collab => collab.GroupId);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectLead)
                .WithMany()
                .HasForeignKey(p => p.ProjectLeadId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<TaskChange>()
                .HasOne(tc => tc.Assignee)
                .WithMany()
                .HasForeignKey(tc => tc.AssigneeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskChangeLog>()
            .HasKey(tcl => new { tcl.TaskId, tcl.ChangeId });


            modelBuilder.Entity<Employee>().HasData(tempAdminCreation());

            modelBuilder.Entity<StateAssignedGroup>()
      .HasKey(sg => new { sg.StateId, sg.GroupId });

            modelBuilder.Entity<StateAssignedGroup>()
                .HasOne(sg => sg.BoardState)
                .WithMany(bs => bs.AssignedGroups)
                .HasForeignKey(sg => sg.StateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StateAssignedGroup>()
                .HasOne(sg => sg.Group)
                .WithMany()
                .HasForeignKey(sg => sg.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
            .HasOne(g => g.Manager)
            .WithMany(e => e.ManagedGroups)
            .HasForeignKey(g => g.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskComment>()
        .HasOne(tc => tc.Task)
        .WithMany() 
        .HasForeignKey(tc => tc.TaskId)
        .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.Commenter)
                .WithMany() 
                .HasForeignKey(tc => tc.CommenterId)
                .OnDelete(DeleteBehavior.Cascade);

        }

        private static Employee tempAdminCreation()
        {
            // The tempAdmin password is now password.
            var password = "$2a$11$wQMl3xgNyJ.8MfbEmSI8SuypYgyefazrYrGRAHRz.Kts7AuSk77.e";
            return new Employee
            {
                EId = 1,
                Email = "admin@temp.com",
                FName = "admin",
                HashedPassword = password,
                IsActive = true,
                IsAdmin = true,
                IsManager = true,
                LName = "admin",
                Username = "tempAdmin"
            };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketSystemDbContext"/> class.
        /// </summary>
        /// <remarks>
        /// See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
        /// for more information and examples.
        /// </remarks>
        public TicketSystemDbContext() { }

     
    }
}
