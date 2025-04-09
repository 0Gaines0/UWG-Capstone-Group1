using Microsoft.AspNetCore.Mvc;
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
    [TestFixture]
    public class TestLandingPageController
    {
        private LandingPageController controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketSystemDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var context = new TicketSystemDbContext(options);
            this.controller = new LandingPageController(context);
        }

        [TearDown]
        public void Dispose()
        {
            this.controller.Dispose();
        }

        [Test]
        public void TestNullConstructor()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => {
                this.controller = new LandingPageController(null);
            });
            Assert.That(ex.ParamName, Is.EqualTo("context"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestValidConstructor()
        {
            var options = new DbContextOptionsBuilder<TicketSystemDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var context = new TicketSystemDbContext(options);
            Assert.DoesNotThrow(() => {
                this.controller = new LandingPageController(context);
            });
        }

        [Test]
        public void TestIndexReturnsViewResult()
        {
            var result = this.controller.Index();
            ClassicAssert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestLogOutRedirectsToLoginIndex()
        {
            ActiveEmployee.Employee = new Employee { EId = 1, FName = "Test", LName = "User" };
            var result = this.controller.LogOut();
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.IsNotNull(redirectResult);
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Login"));
            Assert.That(ActiveEmployee.Employee, Is.Null);
        }
    }
}
