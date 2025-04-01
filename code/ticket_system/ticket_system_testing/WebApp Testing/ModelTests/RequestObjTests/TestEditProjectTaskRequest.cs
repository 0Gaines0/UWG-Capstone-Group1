using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestEditProjectTaskRequest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var editProjectTaskRequest = new EditProjectTaskRequest();

            Assert.That(editProjectTaskRequest.TaskId, Is.EqualTo(0));
            Assert.That(editProjectTaskRequest.StateId, Is.EqualTo(0));
            Assert.That(editProjectTaskRequest.Priority, Is.EqualTo(0));
            Assert.That(editProjectTaskRequest.Summary, Is.Null);
            Assert.That(editProjectTaskRequest.Description, Is.Null);
            Assert.That(editProjectTaskRequest.AssigneeId, Is.Null);
        }
    }
}
