using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Controllers;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;

namespace ticket_system_testing.WebApp_Testing.ControllersTests
{
    public class FakeTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context) => new Dictionary<string, object>();
        public void SaveTempData(HttpContext context, IDictionary<string, object> values) { }
    }

    [TestFixture]
    public class TestLoginController
    {
        private TicketSystemDbContext context;
        private LoginController controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketSystemDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            this.context = new TicketSystemDbContext(options);
            this.controller = new LoginController(this.context);
            this.controller.TempData = new TempDataDictionary(new DefaultHttpContext(), new FakeTempDataProvider());
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
            this.controller.Dispose();
        }

        [Test]
        public void TestNullConstructor()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => {
                new LoginController(null);
            });

            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestValidConstructor()
        {
            Assert.DoesNotThrow(() => {
                this.controller = new LoginController(this.context);
            });
        }

        [Test]
        public void TestIndexReturnsViewResult()
        {
            var result = this.controller.Index();
            ClassicAssert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestLoginWithEmptyUsernameReturnsRedirect()
        {
            var result = this.controller.Login("", "somepassword");
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult?.ActionName, Is.EqualTo("Index"));
            Assert.That(this.controller?.TempData["UsernameError"]?.ToString(), Is.EqualTo("username input must not be null or empty"));
        }

        [Test]
        public void TestLoginWithEmptyPasswordReturnsRedirect()
        {
            var result = this.controller.Login("testuser", "");
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult?.ActionName, Is.EqualTo("Index"));
            Assert.That(this.controller?.TempData["PasswordError"]?.ToString(), Is.EqualTo("password input must not be null or empty"));
        }

        [Test]
        public void TestLoginWithInvalidCredentialsReturnsRedirect()
        {
            var employee = new Employee { EId = 1, Username = "testuser", HashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword") };
            this.context.Employees.Add(employee);
            this.context.SaveChanges();
            var result = this.controller.Login("testuser", "wrongpassword");
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult?.ActionName, Is.EqualTo("Index"));
            Assert.That(this.controller?.TempData["UsernameError"]?.ToString(), Is.EqualTo("credentials entered are invalid"));
        }

        [Test]
        public void TestLoginWithValidCredentialsRedirectsToLandingPage()
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword("correctpassword");
            var employee = new Employee { EId = 2, Username = "validuser", HashedPassword = hashed };
            this.context.Employees.Add(employee);
            this.context.SaveChanges();
            var result = this.controller.Login("validuser", "correctpassword");
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult?.ActionName, Is.EqualTo("Index"));
            Assert.That(redirectResult.ControllerName, Is.EqualTo("LandingPage"));
            Assert.That(ActiveEmployee.Employee, Is.EqualTo(employee));
            Assert.That(this.controller?.TempData["EnteredUsername"]?.ToString(), Is.EqualTo(string.Empty));
        }
    }

}
