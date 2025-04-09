using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestDeleteStateRequest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var deleteStateRequest = new DeleteStateRequest();

            Assert.That(deleteStateRequest.Id, Is.EqualTo(0));
        }
    }
}
