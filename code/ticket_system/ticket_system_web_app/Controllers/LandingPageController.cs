using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;

namespace ticket_system_web_app.Controllers
{
    /// <summary>
    /// LandingPageController Class
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class LandingPageController : Controller
    {
        private readonly TicketSystemDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="LandingPageController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public LandingPageController(TicketSystemDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "DBContext should not be null");
            }
            _context = context;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResult> Index()
        {
            int numPendingRequests = await this._context.ProjectGroups.Include(collab => collab.Group).CountAsync(collab => collab.Group.ManagerId == ActiveEmployee.Employee.EId && !collab.Accepted);
            return View(numPendingRequests);
        }

        /// <summary>
        /// Logs the out.
        /// </summary>
        /// <returns></returns>
        public IActionResult LogOut()
        {
            ActiveEmployee.LogoutCurrentEmployee();
            return RedirectToAction("Index", "Login");
        }
    }
}
