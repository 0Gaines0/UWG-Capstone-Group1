using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
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
    public class TestHomeController
    {
        private HomeController controller;
        private DefaultHttpContext httpContext;

        [SetUp]
        public void Setup()
        {
            this.controller = new HomeController(NullLogger<HomeController>.Instance);
            this.httpContext = new DefaultHttpContext();
            this.httpContext.TraceIdentifier = "test-trace";
        }

        [TearDown]
        public void TearDown()
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
        public void TestPrivacyReturnsViewResult()
        {
            var result = this.controller.Privacy();
            ClassicAssert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestErrorReturnsViewResultWithErrorViewModel()
        {
            this.controller.ControllerContext = new ControllerContext() { HttpContext = this.httpContext };
            var result = this.controller.Error();
            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.IsNotNull(viewResult?.Model);
            ClassicAssert.IsInstanceOf<ErrorViewModel>(viewResult.Model);
            var errorViewModel = viewResult.Model as ErrorViewModel;
            Assert.That(errorViewModel?.RequestId, Is.EqualTo("test-trace"));
        }
    }
}
