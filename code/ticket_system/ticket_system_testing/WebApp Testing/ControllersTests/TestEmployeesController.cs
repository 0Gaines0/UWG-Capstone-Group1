using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using ticket_system_web_app.Controllers;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models.RequestObj;
using ticket_system_web_app.Models;
using Newtonsoft.Json;
using Group = ticket_system_web_app.Models.Group;

namespace ticket_system_testing.WebApp_Testing.ControllersTests
{
    [TestFixture]
    internal class TestEmployeesController
    {
        private TicketSystemDbContext context;
        private EmployeesController controller;

        private void setActiveUserPermsToManager()
        {
            ActiveEmployee.LogoutCurrentEmployee();
            Employee employee = new Employee();
            ticket_system_web_app.Models.Group group = new Group();
            group.ManagerId = employee.EId;
            this.context.Groups.Add(group);
            this.context.SaveChanges();
            ActiveEmployee.LogInEmployee(employee, context);
        }

        private class JsonFailure
        {
            public string Message { get; set; }
        }

        private class JsonAllEmployees
        {
            string ID { get; set; }
            string Name { get; set; }
            string Username { get; set; }
            string Email { get; set; }
            string IsAdmin { get; set; }
        }

        private class JsonEmployee
        {
            public string FName { get; set; }
            public string LName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public bool IsAdmin { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketSystemDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            this.context = new TicketSystemDbContext(options);
            this.controller = new EmployeesController(context);
            ActiveEmployee.LogInEmployee(new Employee(), this.context);
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



        [Test]
        public void TestGetAllEmployeesNotLoggedIn()
        {
            ActiveEmployee.LogoutCurrentEmployee();
            var response = this.controller.GetAllEmployees(ActiveEmployee.AuthToken).Result;

            string json = JsonConvert.SerializeObject(response.Value);
            var result = JsonConvert.DeserializeObject<JsonFailure>(json);


            Assert.That(result.Message.ToLower(), Does.Contain("not logged in"));
        }

        [Test]
        public void TestGetAllEmployeesNotManager()
        {
            var response = this.controller.GetAllEmployees(ActiveEmployee.AuthToken).Result;

            string json = JsonConvert.SerializeObject(response.Value);
            var result = JsonConvert.DeserializeObject<JsonFailure>(json);

            Assert.That(result.Message.ToLower(), Does.Contain("manager permissions"));
        }

        [Test]
        public void TestGetAllEmployeesIsManagerNoEmployees()
        {
            this.setActiveUserPermsToManager();

            var response = this.controller.GetAllEmployees(ActiveEmployee.AuthToken).Result;

            string json = JsonConvert.SerializeObject(response.Value);
            var result = JsonConvert.DeserializeObject<List<JsonAllEmployees>>(json);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void TestGetAllEmployeesIsManagerOneEmployee()
        {
            this.setActiveUserPermsToManager();

            Employee employee = new Employee();
            this.context.Employees.Add(employee);
            this.context.SaveChanges();
            var response = this.controller.GetAllEmployees(ActiveEmployee.AuthToken).Result;

            string json = JsonConvert.SerializeObject(response.Value);
            var result = JsonConvert.DeserializeObject<List<JsonAllEmployees>>(json);

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestGetAllEmployeesIsManagerSomeEmployees()
        {
            this.setActiveUserPermsToManager();

            Employee employee1 = new Employee();
            Employee employee2 = new Employee();
            Employee employee3 = new Employee();
            this.context.Employees.Add(employee1);
            this.context.Employees.Add(employee2);
            this.context.Employees.Add(employee3);
            this.context.SaveChanges();
            var response = this.controller.GetAllEmployees(ActiveEmployee.AuthToken).Result;

            string json = JsonConvert.SerializeObject(response.Value);
            var result = JsonConvert.DeserializeObject<List<JsonAllEmployees>>(json);

            Assert.That(result.Count, Is.EqualTo(3));
        }



        [Test]
        public void TestCreateEmployeeNotLoggedIn()
        {
            ActiveEmployee.LogoutCurrentEmployee();
            var result = this.controller.CreateEmployee(ActiveEmployee.AuthToken, new CreateEmployeeRequest()).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestCreateEmployeeNotAdmin()
        {
            var result = this.controller.CreateEmployee(ActiveEmployee.AuthToken, new CreateEmployeeRequest()).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(null, "lName", "username", "password", "email", false)]
        [TestCase("fName", null, "username", "password", "email", false)]
        [TestCase("fName", "lName", null, "password", "email", false)]
        [TestCase("fName", "lName", "username", null, "email", false)]
        [TestCase("fName", "lName", "username", "password", null, false)]
        [TestCase(" ", "lName", "username", "password", "email", false)]
        [TestCase("fName", " ", "username", "password", "email", false)]
        [TestCase("fName", "lName", " ", "password", "email", false)]
        [TestCase("fName", "lName", "username", " ", "email", false)]
        [TestCase("fName", "lName", "username", "password", " ", false)]
        public void TestCreateEmployeeMissingData(string? fName, string? lName, string? username, string? password, string? email, bool isAdmin)
        {
            ActiveEmployee.Employee.IsAdmin = true;
            var result = this.controller.CreateEmployee(ActiveEmployee.AuthToken, new CreateEmployeeRequest
            {
                FirstName = fName,
                LastName = lName,
                Username = username,
                Password = password,
                Email = email,
                IsAdmin = isAdmin
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task TestCreateEmployeeDuplicateUsernames()
        {
            ActiveEmployee.Employee.IsAdmin = true;
            await this.controller.CreateEmployee(ActiveEmployee.AuthToken, new CreateEmployeeRequest
            {
                FirstName = "test1",
                LastName = "test1",
                Username = "username",
                Password = "test1",
                Email = "test1",
                IsAdmin = false
            });
            var result = this.controller.CreateEmployee(ActiveEmployee.AuthToken, new CreateEmployeeRequest
            {
                FirstName = "test2",
                LastName = "test2",
                Username = "username",
                Password = "test2",
                Email = "test2",
                IsAdmin = true
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase("FName1", "LName1", "Username1", "Password1", "Email1", false)]
        [TestCase("FName2", "LName2", "Username2", "Password2", "Email2", true)]
        public void TestCreateEmployeeValid(string? fName, string? lName, string? username, string? password, string? email, bool isAdmin)
        {
            ActiveEmployee.Employee.IsAdmin = true;

            var result = this.controller.CreateEmployee(ActiveEmployee.AuthToken, new CreateEmployeeRequest
            {
                FirstName = fName,
                LastName = lName,
                Username = username,
                Password = password,
                Email = email,
                IsAdmin = isAdmin
            }).Result;

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(this.context.Employees.Count(), Is.EqualTo(1));
            Employee employee = this.context.Employees.FirstOrDefault();
            Assert.That(employee.FName, Is.EqualTo(fName));
            Assert.That(employee.LName, Is.EqualTo(lName));
            Assert.That(employee.Username, Is.EqualTo(username));
            Assert.That(BCrypt.Net.BCrypt.Verify(password, employee.HashedPassword));
            Assert.That(employee.Email, Is.EqualTo(email));
            Assert.That(employee.IsAdmin, Is.EqualTo(isAdmin));
        }



        [Test]
        public void TestRemoveEmployeeNotLoggedIn()
        {
            ActiveEmployee.LogoutCurrentEmployee();
            var result = this.controller.RemoveEmployee(ActiveEmployee.AuthToken, new RemoveEmployeeRequest()).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestRemoveEmployeeNotAdmin()
        {
            var result = this.controller.RemoveEmployee(ActiveEmployee.AuthToken, new RemoveEmployeeRequest()).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void TestRemoveEmployeeMissingData(string? username)
        {
            ActiveEmployee.Employee.IsAdmin = true;
            var result = this.controller.RemoveEmployee(ActiveEmployee.AuthToken, new RemoveEmployeeRequest
            {
                username = username
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestRemoveEmployeeUsernameNotFound()
        {
            ActiveEmployee.Employee.IsAdmin = true;
            var result = this.controller.RemoveEmployee(ActiveEmployee.AuthToken, new RemoveEmployeeRequest
            {
                username = "username"
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestRemoveEmployeeUsernameIsSelf()
        {
            ActiveEmployee.Employee.IsAdmin = true;
            ActiveEmployee.Employee.Username = "username";
            this.context.Employees.Add(ActiveEmployee.Employee);
            this.context.SaveChanges();
            var result = this.controller.RemoveEmployee(ActiveEmployee.AuthToken, new RemoveEmployeeRequest
            {
                username = "username"
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestRemoveEmployeeWhoIsManager()
        {
            ActiveEmployee.Employee.IsAdmin = true;
            Employee employee = new Employee
            {
                Username = "username"
            };
            this.context.Employees.Add(employee);

            Group group = new Group(employee.EId, "test", "test");
            this.context.Groups.Add(group);
            this.context.SaveChanges();

            var result = this.controller.RemoveEmployee(ActiveEmployee.AuthToken, new RemoveEmployeeRequest
            {
                username = "username"
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestRemoveEmployeeValidOnlyEmployee()
        {
            ActiveEmployee.Employee.IsAdmin = true;

            Employee employee = new Employee
            {
                Username = "username"
            };

            this.context.Employees.Add(employee);
            this.context.SaveChanges();

            var result = this.controller.RemoveEmployee(ActiveEmployee.AuthToken, new RemoveEmployeeRequest
            {
                username = "username"
            }).Result;

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(this.context.Employees.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestRemoveEmployeeValidOtherEmployees()
        {
            ActiveEmployee.Employee.IsAdmin = true;

            Employee employee1 = new Employee
            {
                Username = "username1"
            };
            Employee employee2 = new Employee
            {
                Username = "username2"
            };
            Employee employee3 = new Employee
            {
                Username = "username3"
            };

            this.context.Employees.Add(employee1);
            this.context.Employees.Add(employee2);
            this.context.Employees.Add(employee3);
            this.context.SaveChanges();

            var result = this.controller.RemoveEmployee(ActiveEmployee.AuthToken, new RemoveEmployeeRequest
            {
                username = "username1"
            }).Result;

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(this.context.Employees.Count(), Is.EqualTo(2));
            Assert.That(this.context.Employees.Count(employee => employee.Username == "username1"), Is.EqualTo(0));
        }



        [Test]
        public void TestEditEmployeeNotLoggedIn()
        {
            ActiveEmployee.LogoutCurrentEmployee();
            var result = this.controller.EditEmployee(ActiveEmployee.AuthToken, new EditEmployeeRequest()).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestEditEmployeeNotManager()
        {
            var result = this.controller.EditEmployee(ActiveEmployee.AuthToken, new EditEmployeeRequest()).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(null, "lName", "username", "originalUsername", "email", false)]
        [TestCase("fName", null, "username", "originalUsername", "email", false)]
        [TestCase("fName", "lName", null, "originalUsername", "email", false)]
        [TestCase("fName", "lName", "username", null, "email", false)]
        [TestCase("fName", "lName", "username", "originalUsername", null, false)]
        [TestCase(" ", "lName", "username", "originalUsername", "email", false)]
        [TestCase("fName", " ", "username", "originalUsername", "email", false)]
        [TestCase("fName", "lName", " ", "originalUsername", "email", false)]
        [TestCase("fName", "lName", "username", " ", "email", false)]
        [TestCase("fName", "lName", "username", "originalUsername", " ", false)]
        public void TestEditEmployeeMissingData(string? fName, string? lName, string? username, string? originalUsername, string? email, bool isAdmin)
        {
            ActiveEmployee.Employee.IsAdmin = true;
            var result = this.controller.EditEmployee(ActiveEmployee.AuthToken, new EditEmployeeRequest
            {
                FirstName = fName,
                LastName = lName,
                Username = username,
                OriginalUsername = originalUsername,
                Email = email,
                IsAdmin = isAdmin
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestEditEmployeeUsernameNotFound()
        {
            ActiveEmployee.Employee.IsAdmin = true;
            var result = this.controller.EditEmployee(ActiveEmployee.AuthToken, new EditEmployeeRequest
            {
                FirstName = "fName",
                LastName = "lName",
                Username = "username",
                OriginalUsername = "originalUsername",
                Email = "email",
                IsAdmin = false
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestEditEmployeeNewUsernameAlreadyBeingUsed()
        {
            ActiveEmployee.Employee.IsAdmin = true;
            Employee employee1 = new Employee{
                Username = "oldUsername",
            };
            Employee employee2 = new Employee
            {
                Username = "newUsername"
            };
            this.context.Add(employee1);
            this.context.Add(employee2);
            this.context.SaveChanges();

            var result = this.controller.EditEmployee(ActiveEmployee.AuthToken, new EditEmployeeRequest
            {
                FirstName = "fName",
                LastName = "lName",
                Username = "newUsername",
                OriginalUsername = "oldUsername",
                Email = "email",
                IsAdmin = false
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestEditEmployeeShouldNotAllowSelfDemotion()
        {
            ActiveEmployee.Employee.IsAdmin = true;
            ActiveEmployee.Employee.Username = "username";
            this.context.Add(ActiveEmployee.Employee);
            this.context.SaveChanges();

            var result = this.controller.EditEmployee(ActiveEmployee.AuthToken, new EditEmployeeRequest
            {
                FirstName = "fName",
                LastName = "lName",
                Username = "username",
                OriginalUsername = "username",
                Email = "email",
                IsAdmin = false
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestEditEmployeeAllowsEditingSelf()
        {
            ActiveEmployee.Employee = new Employee {
                FName = "fName1",
                LName = "lName1",
                Username = "username1",
                Email = "email1",
                IsAdmin = true
            };

            this.context.Add(ActiveEmployee.Employee);
            this.context.SaveChanges();

            var result = this.controller.EditEmployee(ActiveEmployee.AuthToken, new EditEmployeeRequest
            {
                FirstName = "fName2",
                LastName = "lName2",
                Username = "username2",
                OriginalUsername = "username1",
                Email = "email2",
                IsAdmin = true
            }).Result;

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var updatedUser = this.context.Employees.First(emp => emp.Username.Equals("username2"));
            Assert.That(updatedUser.FName, Is.EqualTo("fName2"));
            Assert.That(updatedUser.LName, Is.EqualTo("lName2"));
            Assert.That(updatedUser.Username, Is.EqualTo("username2"));
            Assert.That(updatedUser.Email, Is.EqualTo("email2"));
            Assert.That(updatedUser.IsAdmin, Is.True);
        }

        [Test]
        public void TestEditEmployeeAllowsEditingOthers()
        {
            ActiveEmployee.Employee.IsAdmin = true;

            Employee employee = new Employee
            {
                FName = "fName1",
                LName = "lName1",
                Username = "username1",
                Email = "email1",
                IsAdmin = true
            };

            this.context.Add(employee);
            this.context.SaveChanges();

            var result = this.controller.EditEmployee(ActiveEmployee.AuthToken, new EditEmployeeRequest
            {
                FirstName = "fName2",
                LastName = "lName2",
                Username = "username2",
                OriginalUsername = "username1",
                Email = "email2",
                IsAdmin = true
            }).Result;

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var updatedUser = this.context.Employees.First(emp => emp.Username.Equals("username2"));
            Assert.That(updatedUser.FName, Is.EqualTo("fName2"));
            Assert.That(updatedUser.LName, Is.EqualTo("lName2"));
            Assert.That(updatedUser.Username, Is.EqualTo("username2"));
            Assert.That(updatedUser.Email, Is.EqualTo("email2"));
            Assert.That(updatedUser.IsAdmin, Is.True);
        }

        [Test]
        public void TestEditEmployeeAllowsDemotingOtherAdmins()
        {
            ActiveEmployee.Employee.IsAdmin = true;

            Employee employee = new Employee {
                Username = "username",
                IsAdmin = true
            };
            this.context.Add(employee);
            this.context.SaveChanges();

            var result = this.controller.EditEmployee(ActiveEmployee.AuthToken, new EditEmployeeRequest
            {
                FirstName = "fName",
                LastName = "lName",
                Username = "username",
                OriginalUsername = "username",
                Email = "email",
                IsAdmin = false
            }).Result;

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(employee.IsAdmin, Is.False);
        }



        [Test]
        public void TestGetEmployeeNotLoggedIn()
        {
            ActiveEmployee.LogoutCurrentEmployee();
            var result = this.controller.GetEmployee(ActiveEmployee.AuthToken, new GetEmployeeRequest { username = "test" }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestGetEmployeeNotAdmin()
        {
            var result = this.controller.GetEmployee(ActiveEmployee.AuthToken, new GetEmployeeRequest { username = "test" }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestGetEmployeeNullRequest()
        {
            ActiveEmployee.Employee.IsAdmin = true;
            var result = this.controller.GetEmployee(ActiveEmployee.AuthToken, null).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void TestGetEmployeeMissingData(string? username)
        {
            ActiveEmployee.Employee.IsAdmin = true;
            var result = this.controller.RemoveEmployee(ActiveEmployee.AuthToken, new RemoveEmployeeRequest
            {
                username = username
            }).Result;

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestGetEmployeeUsernameNotFound()
        {
            ActiveEmployee.Employee.IsAdmin = true;
            var result = this.controller.GetEmployee(ActiveEmployee.AuthToken, new GetEmployeeRequest
            {
                username = "username"
            }).Result;

            ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void TestGetEmployeeOnlyEmployee()
        {
            ActiveEmployee.Employee.IsAdmin = true;

            Employee employee = new Employee
            {
                FName = "fName",
                LName = "lName",
                Username = "username",
                Email = "email",
                IsAdmin = true
            };
            this.context.Add(employee);
            this.context.SaveChanges();

            var response = this.controller.GetEmployee(ActiveEmployee.AuthToken, new GetEmployeeRequest {
                username = "username"
            }).Result;

            ClassicAssert.IsInstanceOf<OkObjectResult>(response);
            var responseVal = ((OkObjectResult)response).Value;
            string json = JsonConvert.SerializeObject(responseVal);
            var result = JsonConvert.DeserializeObject<JsonEmployee>(json);

            Assert.That(result.FName, Is.EqualTo(employee.FName));
            Assert.That(result.LName, Is.EqualTo(employee.LName));
            Assert.That(result.Username, Is.EqualTo(employee.Username));
            Assert.That(result.Email, Is.EqualTo(employee.Email));
            Assert.That(result.IsAdmin, Is.EqualTo(employee.IsAdmin));
        }

        [Test]
        public void TestGetEmployeeManyEmployees()
        {
            ActiveEmployee.Employee.IsAdmin = true;

            Employee employee1 = new Employee
            {
                FName = "fName1",
                LName = "lName1",
                Username = "username1",
                Email = "email1",
                IsAdmin = false
            };
            Employee employee2 = new Employee
            {
                FName = "fName2",
                LName = "lName2",
                Username = "username2",
                Email = "email2",
                IsAdmin = true
            };
            Employee employee3 = new Employee
            {
                FName = "fName3",
                LName = "lName3",
                Username = "username3",
                Email = "email3",
                IsAdmin = true
            };
            this.context.Add(employee1);
            this.context.Add(employee2);
            this.context.Add(employee3);
            this.context.SaveChanges();

            var response = this.controller.GetEmployee(ActiveEmployee.AuthToken, new GetEmployeeRequest
            {
                username = "username1"
            }).Result;

            ClassicAssert.IsInstanceOf<OkObjectResult>(response);
            var responseVal = ((OkObjectResult)response).Value;
            string json = JsonConvert.SerializeObject(responseVal);
            var result = JsonConvert.DeserializeObject<JsonEmployee>(json);

            Assert.That(result.FName, Is.EqualTo(employee1.FName));
            Assert.That(result.LName, Is.EqualTo(employee1.LName));
            Assert.That(result.Username, Is.EqualTo(employee1.Username));
            Assert.That(result.Email, Is.EqualTo(employee1.Email));
            Assert.That(result.IsAdmin, Is.EqualTo(employee1.IsAdmin));
        }
    }
}
