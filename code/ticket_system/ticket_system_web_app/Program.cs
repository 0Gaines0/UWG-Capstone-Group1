using ticket_system_web_app.Data;
using Microsoft.EntityFrameworkCore;
using ticket_system_web_app.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TicketSystemDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ticket_system_database;Integrated Security=True;Connect Timeout=30;MultipleActiveResultSets=True"));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TicketSystemDbContext>();
    SeedDatabase(dbContext);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "board",
    pattern: "Projects/BoardPage/{pId?}",
    defaults: new { controller = "Projects", action = "BoardPage" }
);

app.Run();

void SeedDatabase(TicketSystemDbContext context)
{
    try
    {
        if (!context.Employees.Any(e => e.Username == "tempUser")) 
        {
            var admin = context.Employees.FirstOrDefault(e => e.Username == "tempAdmin");
            if (admin != null)
            {
                Console.WriteLine("Admin found! Skipping seeding...");
                return;
            }

            var tempAdmin = new Employee
            {
                FName = "Temp",
                LName = "Admin",
                Username = "tempAdmin",
                HashedPassword = "$2a$11$tqFhRcVPxPe/F7g4i2.9c.tms9AlneY5RDZb1SipsY1FQtMcaaecu",
                Email = "temp@company.com",
                IsActive = true,
                IsAdmin = true,
                IsManager = false
            };

            var tempEmployee = new Employee
            {
                FName = "Temp",
                LName = "Employee",
                Username = "tempUser",
                HashedPassword = "$2a$11$tqFhRcVPxPe/F7g4i2.9c.tms9AlneY5RDZb1SipsY1FQtMcaaecu",
                Email = "temp@company.com",
                IsActive = true,
                IsAdmin = false,
                IsManager = false
            };


            context.Employees.Add(tempAdmin);
            context.Employees.Add(tempEmployee);
            context.SaveChanges();

            admin = context.Employees.FirstOrDefault(e => e.Username == "tempAdmin");

            Console.WriteLine("Temp Employee and Group created successfully!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database seeding failed: {ex.Message}");
    }
}


