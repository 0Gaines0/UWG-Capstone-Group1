using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestUpdateStateNameRequest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var updateStateNameRequest = new UpdateStateNameRequest();

            Assert.That(updateStateNameRequest.Id, Is.EqualTo(0));
            Assert.That(updateStateNameRequest.Name, Is.Null);
        }
    }
}
