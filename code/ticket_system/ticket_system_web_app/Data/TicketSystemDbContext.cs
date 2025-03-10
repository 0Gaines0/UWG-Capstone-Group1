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
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public DbSet<User> Users { get; set; }
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
            modelBuilder.Entity<Project>().HasMany(project => project.AssignedGroups).WithMany(group => group.AssignedProjects).UsingEntity(join => join.ToTable("project_group"));
            modelBuilder.Entity<Project>().HasOne(e => e.ProjectLead).WithMany(e => e.ProjectsLeading).HasForeignKey(e => e.ProjectLeadId).IsRequired();

            modelBuilder.Entity<Employee>().HasData(tempAdminCreation());

        }

        private static Employee tempAdminCreation()
        {
            var password = "$2a$11$tqFhRcVPxPe/F7g4i2.9c.tms9AlneY5RDZb1SipsY1FQtMcaaecu";
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
