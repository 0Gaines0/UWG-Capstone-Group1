using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using ticket_system_web_app.Models;

namespace ticket_system_web_app.Data
{
    public class TicketSystemDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        // Constructor for Dependency Injection
        public TicketSystemDbContext(DbContextOptions options) : base(options)
        {
        }

        // Parameterless constructor for design-time migrations
        public TicketSystemDbContext() { }

    }
}
