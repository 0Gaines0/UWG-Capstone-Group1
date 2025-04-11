using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestCreateProjectRequest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var createProjectRequest = new CreateProjectRequest();

            Assert.That(createProjectRequest.PLeadId, Is.EqualTo(0));
            Assert.That(createProjectRequest.PTitle, Is.Null);
            Assert.That(createProjectRequest.PDescription, Is.Null);
            Assert.That(createProjectRequest.CollaboratingGroupIDs, Is.Empty);
        }
    }
}
