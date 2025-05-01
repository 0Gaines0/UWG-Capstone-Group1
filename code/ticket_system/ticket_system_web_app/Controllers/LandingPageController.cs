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
        public async Task<IActionResult> Index()
        {
            // Redirect to login if the user is not logged in or Employee object is null
            if (!ActiveEmployee.IsLoggedIn() || ActiveEmployee.Employee == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int eId = ActiveEmployee.Employee.EId;

            int numPendingRequests = await _context.ProjectGroups
                .Include(collab => collab.Group)
                .CountAsync(collab => collab.Group.ManagerId == eId && !collab.Accepted);

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
