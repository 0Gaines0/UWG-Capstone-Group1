using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestActiveEmployee
    {
        [SetUp]
        public void Setup()
        {
            ActiveEmployee.LogoutCurrentEmployee();
        }

        [Test]
        public void TestLogInEmployeeSetsEmployee()
        {
            var employee = new Employee();

            ActiveEmployee.LogInEmployee(employee);

            Assert.That(ActiveEmployee.Employee, Is.EqualTo(employee), "Expected ActiveEmployee.Employee to be set after login.");
        }

        [Test]
        public void TestLogInEmployeeWithNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => ActiveEmployee.LogInEmployee(null),
                "Expected LogInEmployee to throw ArgumentNullException when passing null."
            );
        }

        [Test]
        public void TestLogoutCurrentEmployeeSetsEmployeeToNull()
        {
            var employee = new Employee();
            ActiveEmployee.LogInEmployee(employee);

            ActiveEmployee.LogoutCurrentEmployee();

            Assert.That(ActiveEmployee.Employee, Is.Null, "Expected ActiveEmployee.Employee to be null after logout.");
        }
    }
}
