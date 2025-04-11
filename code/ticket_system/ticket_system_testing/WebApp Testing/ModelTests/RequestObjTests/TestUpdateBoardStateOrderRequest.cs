using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestUpdateBoardStateOrderRequest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var updateBoardStateOrderRequest = new UpdateBoardStateOrderRequest();

            Assert.That(updateBoardStateOrderRequest.StateId, Is.EqualTo(0));
            Assert.That(updateBoardStateOrderRequest.Position, Is.EqualTo(0));
        }
    }
}
