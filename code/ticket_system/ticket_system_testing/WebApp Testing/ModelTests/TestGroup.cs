using System;
using NUnit.Framework;
using ticket_system_web_app.Models;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestGroup
    {
        [Test]
        public void TestDefaultConstructorInitProperties()
        {
            var group = new Group();

            Assert.That(group.GName, Is.EqualTo(string.Empty), "Expected GName to be empty by default.");
            Assert.That(group.GDescription, Is.EqualTo(string.Empty), "Expected GDescription to be empty by default.");
            Assert.That(group.ManagerId, Is.EqualTo(0), "Expected ManagerId to be 0 by default.");
            Assert.That(group.GId, Is.EqualTo(0), "Expected GId to be 0 by default.");
            Assert.That(group.Employees, Is.Not.Null, "Expected Employees collection to be initialized.");
            Assert.That(group.Employees, Is.Empty, "Expected Employees collection to be empty by default.");
            Assert.That(group.Collaborations, Is.Not.Null, "Expected AssignedProjects collection to be initialized.");
            Assert.That(group.Collaborations, Is.Empty, "Expected AssignedProjects collection to be empty by default.");
        }

        [Test]
        public void TestValidConstructorManagerIdGNameGDescription()
        {
            int managerId = 10;
            string gName = "Test Group";
            string gDescription = "Group for testing";

            var group = new Group(managerId, gName, gDescription);

            Assert.That(group.ManagerId, Is.EqualTo(managerId), "ManagerId should be set correctly.");
            Assert.That(group.GName, Is.EqualTo(gName), "GName should be set correctly.");
            Assert.That(group.GDescription, Is.EqualTo(gDescription), "GDescription should be set correctly.");
        }

        [Test]
        public void TestValidConstructorGIdManagerIdGNameGDescription()
        {
            int gId = 1;
            int managerId = 10;
            string gName = "Test Group";
            string gDescription = "Group for testing";

            var group = new Group(gId, managerId, gName, gDescription);

            Assert.That(group.GId, Is.EqualTo(gId), "GId should be set correctly.");
            Assert.That(group.ManagerId, Is.EqualTo(managerId), "ManagerId should be set correctly.");
            Assert.That(group.GName, Is.EqualTo(gName), "GName should be set correctly.");
            Assert.That(group.GDescription, Is.EqualTo(gDescription), "GDescription should be set correctly.");
        }

        [Test]
        public void TestConstructorNegativeManagerIdThrowsArgumentOutOfRangeException()
        {
            int managerId = -5;
            string gName = "Valid Name";
            string gDescription = "Valid Description";

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Group(managerId, gName, gDescription)
            );
            Assert.That(ex.ParamName, Is.EqualTo("managerId"));
            Assert.That(ex.Message, Does.Contain("Parameter must not be negative"));
        }

        [Test]
        public void TestConstructorNullGNameThrowsArgumentException()
        {
            int managerId = 10;
            string? gName = null;
            string gDescription = "Valid Description";

            var ex = Assert.Throws<ArgumentException>(
                () => new Group(managerId, gName, gDescription)
            );
            Assert.That(ex.ParamName, Is.EqualTo("gName"));
            Assert.That(ex.Message, Does.Contain("Parameter must not be null or empty"));
        }

        [Test]
        public void TestConstructorEmptyGNameThrowsArgumentException()
        {
            int managerId = 10;
            string gName = "";
            string gDescription = "Valid Description";

            var ex = Assert.Throws<ArgumentException>(
                () => new Group(managerId, gName, gDescription)
            );
            Assert.That(ex.ParamName, Is.EqualTo("gName"));
            Assert.That(ex.Message, Does.Contain("Parameter must not be null or empty"));
        }

        [Test]
        public void TestConstructorNegativeGIdThrowsArgumentOutOfRangeException()
        {
            int gId = -1;
            int managerId = 10;
            string gName = "Valid Name";
            string gDescription = "Valid Description";

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Group(gId, managerId, gName, gDescription)
            );
            Assert.That(ex.ParamName, Is.EqualTo("gId"));
            Assert.That(ex.Message, Does.Contain("Parameter must not be negative"));
        }
    }
}
