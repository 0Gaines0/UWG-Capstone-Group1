using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{

    [TestFixture]
    public class TestCreateGroupRequest
    {
        [Test]
        public void testDefaultConstructorInitProperties()
        {
            var request = new CreateGroupRequest();

            Assert.That(request.GroupName, Is.EqualTo(null), "Expected GroupName to be null by default.");
            Assert.That(request.GroupDescription, Is.EqualTo(null), "Expected GroupDescription to be null by default.");
            Assert.That(request.ManagerId, Is.EqualTo(0), "Expected ManagerId to be 0 by default.");
            Assert.That(request.MemberIds, !Is.EqualTo(null), "Expected MemberIds not to be null.");
            Assert.That(request.MemberIds.Count, Is.EqualTo(0), "Expected MemberIds to be empty by default.");
        }

        [Test]
        public void testPropertiesWorkAsExpected()
        {
            var groupName = "Developers";
            var groupDescription = "Group for developers.";
            var managerId = 10;
            var memberIds = new List<int> { 1, 2, 3 };

            var request = new CreateGroupRequest
            {
                GroupName = groupName,
                GroupDescription = groupDescription,
                ManagerId = managerId,
                MemberIds = memberIds
            };

            

            Assert.That(groupName, Is.EqualTo(request.GroupName));
            Assert.That(groupDescription, Is.EqualTo(request.GroupDescription));
            Assert.That(managerId, Is.EqualTo(request.ManagerId));
            Assert.That(memberIds, Is.EqualTo(request.MemberIds));
        }
    }
}
