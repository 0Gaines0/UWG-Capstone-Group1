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
    public class TestRemoveGroupRequest
    {
        [Test]
        public void testDefaultConstructorInitProperties()
        {
            var request = new RemoveGroupRequest();

            Assert.That(request.GroupName, Is.Null, "Expected GroupName to be null by default.");
        }

        [Test]
        public void testPropertiesWorkAsExpected()
        {
            var groupName = "Support Team";

            var request = new RemoveGroupRequest
            {
                GroupName = groupName
            };

            Assert.That(request.GroupName, Is.EqualTo(groupName), "Expected GroupName to be set correctly.");
        }
    }
}
