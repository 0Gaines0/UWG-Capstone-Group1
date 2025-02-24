using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Controllers;
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
            this.controller = new LandingPageController();
        }

        [TearDown]
        public void Dispose()
        {
            this.controller.Dispose();
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

        [Test]
        public void TestRedirectToGroupIndexRedirectsToGroupsIndex()
        {
            var result = this.controller.RedirectToGroupIndex();
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.IsNotNull(redirectResult);
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Groups"));
        }
    }
}
