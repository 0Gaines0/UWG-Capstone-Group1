using Microsoft.AspNetCore.Mvc;

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
            return RedirectToAction("Index", "Login");
        }
    }
}
