﻿using Microsoft.AspNetCore.Mvc;
using ticket_system_web_app.Data;

namespace ticket_system_web_app.Controllers
{
    public class LoginController : Controller
    {
        private static string VALIDATE_PARAMETER_MESSAGE = "input must not be null or empty";
        private static string INVALID_ENTERED_CREDENTIALS = "credentials entered are invalid";
        private readonly TicketSystemDbContext context;
       
        public LoginController(TicketSystemDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), VALIDATE_PARAMETER_MESSAGE);
            }
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
           if (string.IsNullOrEmpty(username))
            {
                TempData["UsernameError"] = nameof(username) + " " + VALIDATE_PARAMETER_MESSAGE;
                return RedirectToAction("Index");
            } 
           else
            {
                TempData["EnteredUsername"] = username;
            }
            if (string.IsNullOrEmpty(password))
            {
                TempData["PasswordError"] = nameof(password) + " " + VALIDATE_PARAMETER_MESSAGE;
                return RedirectToAction("Index");
            }

            var user = this.context.Employees.FirstOrDefault(employee => employee.Username != null && employee.Username.Equals(username));
            var userPassword = user?.HashedPassword;
            if (user == null || userPassword == null || !this.verifyCorrectPassword(password, userPassword))
            {
                TempData["UsernameError"] = INVALID_ENTERED_CREDENTIALS;
                return RedirectToAction("Index");
            }

            TempData["EnteredUsername"] = string.Empty;
            return RedirectToAction("Index", "LandingPage");





        }

        private bool verifyCorrectPassword(string passwordEntered, string hashedPassword) 
        {
            return BCrypt.Net.BCrypt.Verify(passwordEntered, hashedPassword);
        }
    }
}
