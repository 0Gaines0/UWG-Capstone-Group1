﻿using Microsoft.AspNetCore.Mvc;
using ticket_system_web_app.Models;

namespace ticket_system_web_app.Controllers
{
    /// <summary>
    /// LandingPageController Class
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class LandingPageController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
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

        /// <summary>
        /// Redirects the index of to group.
        /// </summary>
        /// <returns></returns>
        public IActionResult RedirectToGroupIndex()
        {
            return RedirectToAction("Index", "Groups");
        }

        /// <summary>
        /// Redirects the index of to employees.
        /// </summary>
        /// <returns></returns>
        public IActionResult RedirectToEmployeesIndex()
        {
            return RedirectToAction("Index", "Employees");
        }
    }
}
