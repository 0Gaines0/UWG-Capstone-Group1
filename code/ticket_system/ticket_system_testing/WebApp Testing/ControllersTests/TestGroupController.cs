using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using ticket_system_web_app.Controllers;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace ticket_system_testing.WebApp_Testing.ControllerTests
{
    [TestFixture]
    public class TestGroupsController
    {
        private TicketSystemDbContext context;
        private GroupsController controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketSystemDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.context = new TicketSystemDbContext(options);
            this.controller = new GroupsController(context);
            ActiveEmployee.LogInEmployee(new Employee(), this.context);
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
            this.controller.Dispose();
        }

        [Test]
        public async Task TestIndexReturnsViewResultWithGroups()
        {
            var employee = new Employee { EId = 10, FName = "John", LName = "Doe" };
            this.context.Employees.Add(employee);
            var group = new Group(employee.EId, "TestGroup", "A test group");
            this.context.Groups.Add(group);
            await this.context.SaveChangesAsync();

            var result = await controller.Index();

            ClassicAssert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            ClassicAssert.IsNotNull(viewResult?.Model);
            var model = viewResult.Model as List<object>;
            ClassicAssert.IsNotNull(model);
            Assert.That(model.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestCreateGroupModalReturnsPartialViewResult()
        {
            var result = this.controller.CreateGroupModal();

            ClassicAssert.IsInstanceOf<PartialViewResult>(result);
            var partialResult = result as PartialViewResult;
            ClassicAssert.That(partialResult?.ViewName, Is.EqualTo("_CreateGroupModal"));
        }

        [Test]
        public async Task TestGetAllGroupsReturnsJsonResult()
        {
            var group = new Group(10, "TestGroup", "A test group");
            this.context.Groups.Add(group);
            await context.SaveChangesAsync();

            var result = await this.controller.GetAllGroups(ActiveEmployee.AuthToken);

            ClassicAssert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            ClassicAssert.IsNotNull(jsonResult.Value);
            var groups = jsonResult.Value as List<object>;
            ClassicAssert.IsNotNull(groups);
            ClassicAssert.That(groups.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TestGetActiveUserGroupsNoActiveEmployeeReturnsJsonNull()
        {
            var result = await controller.GetActiveUserGroups(ActiveEmployee.AuthToken);

            ClassicAssert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            ClassicAssert.IsNull(jsonResult.Value);
        }

        [Test]
        public async Task TestGetActiveUserGroupsWithActiveEmployeeReturnsUserGroups()
        {
            var activeEmployee = new Employee { EId = 1, FName = "Active", LName = "User" };
            ActiveEmployee.Employee = activeEmployee;
            context.Employees.Add(activeEmployee);

            var groupAsManager = new Group(activeEmployee.EId, "ManagerGroup", "Group managed by active employee");
            context.Groups.Add(groupAsManager);

            var groupAsMember = new Group(10, 999, "MemberGroup", "Group where active is a member");
            groupAsMember.Employees.Add(activeEmployee);
            context.Groups.Add(groupAsMember);

            await this.context.SaveChangesAsync();

            var result = await this.controller.GetActiveUserGroups(ActiveEmployee.AuthToken);

            ClassicAssert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            ClassicAssert.IsNotNull(jsonResult.Value);
            var userGroups = jsonResult.Value as List<string>;
            ClassicAssert.IsNotNull(userGroups);
            Assert.That(userGroups, Does.Contain("ManagerGroup"));
            Assert.That(userGroups, Does.Contain("MemberGroup"));
        }

        [Test]
        public async Task TestCreateGroupInvalidRequestReturnsBadRequest()
        {
            var resultNull = await this.controller.CreateGroup(ActiveEmployee.AuthToken, null);
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(resultNull);

            var invalidRequest = new CreateGroupRequest
            {
                GroupName = "   ",
                ManagerId = 5
            };
            var resultInvalid = await controller.CreateGroup(ActiveEmployee.AuthToken, invalidRequest);
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(resultInvalid);

            invalidRequest = new CreateGroupRequest
            {
                GroupName = "ValidGroup",
                ManagerId = 0
            };
            resultInvalid = await this.controller.CreateGroup(ActiveEmployee.AuthToken, invalidRequest);
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(resultInvalid);
        }

        [Test]
        public async Task TestCreateGroupDuplicateGroupReturnsBadRequest()
        {
            var existingGroup = new Group(10, "DuplicateGroup", "Existing group");
            this.context.Groups.Add(existingGroup);
            await context.SaveChangesAsync();

            var request = new CreateGroupRequest
            {
                GroupName = "DuplicateGroup",
                ManagerId = 10,
                GroupDescription = "New description",
                MemberIds = new List<int>()
            };
            var result = await this.controller.CreateGroup(ActiveEmployee.AuthToken, request);

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task TestCreateGroupValidRequestReturnsOk()
        {
            var manager = new Employee { EId = 20, FName = "Manager", LName = "User" };
            this.context.Employees.Add(manager);
            await this.context.SaveChangesAsync();

            var request = new CreateGroupRequest
            {
                GroupName = "NewGroup",
                ManagerId = manager.EId,
                GroupDescription = "A new group",
                MemberIds = new List<int>()
            };

            var result = await this.controller.CreateGroup(ActiveEmployee.AuthToken, request);

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult?.Value);
            var createdGroup = await context.Groups.FirstOrDefaultAsync(g => g.GName == "NewGroup");
            ClassicAssert.IsNotNull(createdGroup);
            Assert.That(createdGroup.ManagerId, Is.EqualTo(manager.EId));
        }

        [Test]
        public async Task TestCreateGroupValidRequestWithMembers()
        {
            var manager = new Employee { EId = 1, FName = "Manager", LName = "User" };
            this.context.Employees.Add(manager);
            var member1 = new Employee { EId = 2, FName = "Alice", LName = "Smith" };
            var member2 = new Employee { EId = 3, FName = "Bob", LName = "Jones" };
            this.context.Employees.AddRange(member1, member2);
            await this.context.SaveChangesAsync();

            var request = new CreateGroupRequest
            {
                GroupName = "TestGroupWithMembers",
                ManagerId = manager.EId,
                GroupDescription = "A test group with members",
                MemberIds = new List<int> { member1.EId, member2.EId }
            };

            var result = await this.controller.CreateGroup(ActiveEmployee.AuthToken, request);

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult?.Value);
            var createdGroup = await this.context.Groups.FirstOrDefaultAsync(g => g.GName == request.GroupName);
            ClassicAssert.IsNotNull(createdGroup);
            Assert.That(createdGroup.ManagerId, Is.EqualTo(manager.EId));
            ClassicAssert.IsNotNull(createdGroup.Employees);
            var memberIds = createdGroup.Employees.Select(e => e.EId).ToList();
            Assert.That(memberIds, Is.EquivalentTo(request.MemberIds));
        }

        [Test]
        public async Task TestRemoveGroupInvalidRequestReturnsBadRequest()
        {
            var request = new RemoveGroupRequest
            {
                GroupName = "   "
            };

            var result = await this.controller.RemoveGroup(ActiveEmployee.AuthToken, request);

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task TestRemoveGroupNonExistentReturnsBadRequest()
        {
            var request = new RemoveGroupRequest
            {
                GroupName = "NonExistentGroup"
            };

            var result = await this.controller.RemoveGroup(ActiveEmployee.AuthToken, request);

            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }




        [Test]
        public async Task TestRemoveGroupValidRequestReturnsOk()
        {
            var group = new Group(10, "GroupToRemove", "Group to be removed");
            this.context.Groups.Add(group);
            await context.SaveChangesAsync();

            var request = new RemoveGroupRequest
            {
                GroupName = "GroupToRemove"
            };

            var result = await controller.RemoveGroup(ActiveEmployee.AuthToken, request);

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var removedGroup = await context.Groups.FirstOrDefaultAsync(g => g.GName == "GroupToRemove");
            ClassicAssert.IsNull(removedGroup);
        }

        [Test]
        public async Task TestGetAllManagersReturnsJsonResult()
        {
            var manager1 = new Employee { EId = 1, FName = "Alice", LName = "Smith", IsManager = true };
            var admin1 = new Employee { EId = 2, FName = "Bob", LName = "Jones", IsAdmin = true };
            var employee = new Employee { EId = 3, FName = "Charlie", LName = "Brown", IsManager = false, IsAdmin = false };
            this.context.Employees.AddRange(manager1, admin1, employee);
            await context.SaveChangesAsync();

            var result = await controller.GetAllManagers(ActiveEmployee.AuthToken);

            ClassicAssert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            ClassicAssert.IsNotNull(jsonResult.Value);
            var managers = jsonResult.Value as IEnumerable<dynamic>;
            ClassicAssert.IsNotNull(managers);
            var managersList = managers.ToList();
            Assert.That(managersList.Count, Is.EqualTo(2));
            var manager = managersList[0].GetType().GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(managersList[0]);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(managersList.Any(m => m.GetType().GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(m) == "Alice Smith"));
                Assert.That(managersList.Any(m => m.GetType().GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(m) == "Bob Jones"));
            }
        }

        [Test]
        public async Task TestGetAllEmployeesReturnsJsonResult()
        {
            var employee1 = new Employee { EId = 1, FName = "Alice", LName = "Smith" };
            var employee2 = new Employee { EId = 2, FName = "Bob", LName = "Jones" };
            context.Employees.AddRange(employee1, employee2);
            await context.SaveChangesAsync();

            var result = await controller.GetAllEmployees(ActiveEmployee.AuthToken);

            ClassicAssert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            ClassicAssert.IsNotNull(jsonResult.Value);
            var employees = jsonResult.Value as IEnumerable<dynamic>;
            ClassicAssert.IsNotNull(employees);
            var employeesList = employees.ToList();
            Assert.That(employeesList.Count, Is.EqualTo(2));
            Assert.That(employeesList.Any(e => e.GetType().GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(e) == "Alice Smith"));
            Assert.That(employeesList.Any(e => e.GetType().GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(e) == "Bob Jones"));
        }
    }
}
