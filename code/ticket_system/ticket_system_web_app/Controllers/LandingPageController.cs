using Microsoft.AspNetCore.Mvc;
using ticket_system_web_app.Models;

namespace ticket_system_web_app.Controllers
{
    public class LandingPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            ActiveEmployee.LogoutCurrentEmployee();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult RedirectToGroupIndex()
        {
            return RedirectToAction("Index", "Groups");
        }
    }
}
