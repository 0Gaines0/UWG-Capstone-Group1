using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestGetEmployeeRequest
    {
        [Test]
        public void TestNullUsername()
        {
            var getEmployeeRequest = new GetEmployeeRequest
            {
                username = null
            };

            Assert.That(getEmployeeRequest.username, Is.Null);
        }

        [Test]
        public void TestEmptyUsername()
        {
            var getEmployeeRequest = new GetEmployeeRequest
            {
                username = ""
            };

            Assert.That(getEmployeeRequest.username, Is.EqualTo(string.Empty));
        }

        [Test]
        public void TestOtherUsername()
        {
            var getEmployeeRequest = new GetEmployeeRequest
            {
                username = "Test"
            };

            Assert.That(getEmployeeRequest.username, Is.EqualTo("Test"));
        }
    }
}
