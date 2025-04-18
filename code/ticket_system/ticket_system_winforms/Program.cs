using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_system_web_app.Data;
using ticket_system_winforms.DAL;
using ticket_system_web_app.Models;
using ticket_system_winforms.View;

namespace ticket_system_winforms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string connectionString = DBConfig.ConnectionString;
            var serviceProvider = new ServiceCollection()
                .AddDbContext<TicketSystemDbContext>(options =>
                    options.UseLazyLoadingProxies().UseSqlServer(connectionString)) 
                .BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<TicketSystemDbContext>();
            context.Database.EnsureCreated();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginPage(context));
        }
    }
}
