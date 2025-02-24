using ticket_system_web_app.Data;
using Microsoft.EntityFrameworkCore;
using ticket_system_web_app.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TicketSystemDbContext>(options =>
    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ticket_system_database;Integrated Security=True;Connect Timeout=30"));


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

app.Run();

void SeedDatabase(TicketSystemDbContext context)
{
    try
    {
        if (!context.Employees.Any(e => e.Username == "tempUser")) 
        {
            var admin = context.Employees.FirstOrDefault(e => e.Username == "tempAdmin");
            if (admin == null)
            {
                Console.WriteLine("Admin not found! Skipping seeding...");
                return;
            }

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

            context.Employees.Add(tempEmployee);
            context.SaveChanges(); 

            var newGroup = new Group
            {
                ManagerId = admin.EId,
                GName = "Temp Group",
                GDescription = "This is a temporary group for testing."
            };

            context.Groups.Add(newGroup);
            context.SaveChanges();

            newGroup.Employees.Add(tempEmployee);
            context.SaveChanges();

            Console.WriteLine("Temp Employee and Group created successfully!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database seeding failed: {ex.Message}");
    }
}


