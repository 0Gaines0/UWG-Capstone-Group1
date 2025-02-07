using Microsoft.AspNetCore.Mvc;

namespace ticket_system_web_app.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return RedirectToAction("Index", "LandingPage");
        }
    }
}
