using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestRemoveEmployeeRequest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var removeEmloyeeRequest = new RemoveEmployeeRequest();

            Assert.That(removeEmloyeeRequest.username, Is.Null);
        }
    }
}
