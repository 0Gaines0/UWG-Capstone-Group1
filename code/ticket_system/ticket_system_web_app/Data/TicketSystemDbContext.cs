using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using ticket_system_web_app.Models;

namespace ticket_system_web_app.Data
{
    public class TicketSystemDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Group> Groups { get; set; }

        // Constructor for Dependency Injection
        public TicketSystemDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasMany(employee => employee.GroupsExistingIn).WithMany(group => group.Employees).UsingEntity(join => join.ToTable("group_member"));
            modelBuilder.Entity<Project>().HasMany(project => project.AssignedGroups).WithMany(group => group.AssignedProjects).UsingEntity(join => join.ToTable("project_group"));

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

        // Parameterless constructor for design-time migrations
        public TicketSystemDbContext() { }

    }
}
