using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestAddStateRequest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var addStateRequest = new AddStateRequest();

            Assert.That(addStateRequest.BoardId, Is.EqualTo(0));
            Assert.That(addStateRequest.Name, Is.Null);
        }
    }
}
