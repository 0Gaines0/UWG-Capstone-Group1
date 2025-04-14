using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestActiveEmployee
    {
        private TicketSystemDbContext context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketSystemDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            this.context = new TicketSystemDbContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            ActiveEmployee.LogoutCurrentEmployee();
            this.context.Dispose();
        }

        [Test]
        public void TestLogInEmployeeSetsEmployee()
        {
            var employee = new Employee();

            ActiveEmployee.LogInEmployee(employee, this.context);

            Assert.That(ActiveEmployee.Employee, Is.EqualTo(employee), "Expected ActiveEmployee.Employee to be set after login.");
        }

        [Test]
        public void TestLogInEmployeeWithNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => ActiveEmployee.LogInEmployee(null, this.context),
                "Expected LogInEmployee to throw ArgumentNullException when passing null."
            );
        }

        [Test]
        public void TestLogInEmployeeWithNullContextThrowsArgumentNullException()
        {
            var employee = new Employee();

            Assert.Throws<ArgumentNullException>(
                () => ActiveEmployee.LogInEmployee(employee, null)
            );
        }

        [Test]
        public void TestLogoutCurrentEmployeeSetsEmployeeToNull()
        {
            var employee = new Employee();
            ActiveEmployee.LogInEmployee(employee, this.context);

            ActiveEmployee.LogoutCurrentEmployee();

            Assert.That(ActiveEmployee.Employee, Is.Null, "Expected ActiveEmployee.Employee to be null after logout.");
        }

        [Test]
        public void TestIsLoggedInWhenEmployeeShouldBeLoggedIn()
        {
            var employee = new Employee();
            ActiveEmployee.LogInEmployee(employee, this.context);

            Assert.That(ActiveEmployee.IsLoggedIn(), Is.True);
        }

        [Test]
        public void TestIsLoggedInWhenEmployeeShouldBeLoggedOut()
        {
            var employee = new Employee();
            ActiveEmployee.LogInEmployee(employee, this.context);

            ActiveEmployee.LogoutCurrentEmployee();

            Assert.That(ActiveEmployee.IsLoggedIn(), Is.False);
        }

        [Test]
        public void TestIsManagerWhenEmployeeManagesNoGroups()
        {
            var employee = new Employee();
            ActiveEmployee.LogInEmployee(employee, this.context);

            Assert.That(ActiveEmployee.IsManager(), Is.False);
        }

        [Test]
        public void TestIsManagerWhenEmployeeManagesOneGroup()
        {
            var employee = new Employee();
            this.context.Groups.Add(new Group(employee.EId, "test", "test"));
            this.context.SaveChanges();

            ActiveEmployee.LogInEmployee(employee, this.context);

            Assert.That(ActiveEmployee.IsManager(), Is.True);
        }

        [Test]
        public void TestIsManagerWhenEmployeeManagesManyGroups()
        {
            var employee = new Employee();
            this.context.Groups.Add(new Group(employee.EId, "test1", "test1"));
            this.context.Groups.Add(new Group(employee.EId, "test2", "test2"));
            this.context.Groups.Add(new Group(employee.EId, "test3", "test3"));
            this.context.SaveChanges();

            ActiveEmployee.LogInEmployee(employee, this.context);

            Assert.That(ActiveEmployee.IsManager(), Is.True);
        }

        [Test]
        public void TestIsAdminWhenEmployeeIsAdmin()
        {
            var employee = new Employee();
            employee.IsAdmin = true;
            ActiveEmployee.LogInEmployee(employee, this.context);

            Assert.That(ActiveEmployee.IsAdmin(), Is.True);
        }

        [Test]
        public void TestIsAdminWhenEmployeeIsNotAdmin()
        {
            var employee = new Employee();
            employee.IsAdmin = false;
            ActiveEmployee.LogInEmployee(employee, this.context);

            Assert.That(ActiveEmployee.IsAdmin(), Is.False);
        }

        [Test]
        public void TestIsValidRequestWhenEmployeeIsNotLoggedIn()
        {
            var employee = new Employee();
            employee.IsAdmin = false;

            Assert.That(ActiveEmployee.IsValidRequest("test"), Is.False);
        }

        [Test]
        public void TestIsValidRequestWhenAuthTokenIsIncorrect()
        {
            var employee = new Employee();
            employee.IsAdmin = false;
            ActiveEmployee.LogInEmployee(employee, this.context);


            Assert.That(ActiveEmployee.IsValidRequest("test"), Is.False);
        }

        [Test]
        public void TestIsValidRequestWhenAuthTokenIsCorrect()
        {
            var employee = new Employee();
            employee.IsAdmin = false;
            ActiveEmployee.LogInEmployee(employee, this.context);

            Assert.That(ActiveEmployee.IsValidRequest(ActiveEmployee.AuthToken), Is.True);
        }
    }
}
