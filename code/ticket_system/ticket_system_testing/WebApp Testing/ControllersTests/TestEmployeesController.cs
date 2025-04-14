using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Controllers;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models.RequestObj;
using ticket_system_web_app.Models;

namespace ticket_system_testing.WebApp_Testing.ControllersTests
{
    [TestFixture]
    internal class TestEmployeesController
    {
        private TicketSystemDbContext context;
        private EmployeesController controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketSystemDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            this.context = new TicketSystemDbContext(options);
            this.controller = new EmployeesController(context);
            ActiveEmployee.Employee = null;
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
                this.controller = new EmployeesController(null);
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
                this.controller = new EmployeesController(context);
            });
        }

        [Test]
        public void TestIndexReturnsViewResult()
        {
            var result = this.controller.Index().Result;
            ClassicAssert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestCreateEmployeeModalReturnsPartialViewResult()
        {
            var result = this.controller.CreateEmployeeModal();
            ClassicAssert.IsInstanceOf<PartialViewResult>(result);
        }
    }
}
