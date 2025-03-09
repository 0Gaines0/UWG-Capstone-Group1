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
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Group> Groups { get; set; }

        public DbSet<ProjectBoard> ProjectBoards { get; set; }

        public DbSet<BoardState> BoardStates { get; set; }


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
        public TicketSystemDbContext() { }

     
    }
}
