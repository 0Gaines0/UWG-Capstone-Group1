using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestProjectTask
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var projectTask = new ProjectTask();

            Assert.That(projectTask.TaskId, Is.EqualTo(0));
            Assert.That(projectTask.Summary, Is.Null);
            Assert.That(projectTask.Description, Is.Null);
            Assert.That(projectTask.Priority, Is.EqualTo(0));
            Assert.That(projectTask.CreatedDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(projectTask.StateId, Is.Null);
            Assert.That(projectTask.AssigneeId, Is.Null);
            Assert.That(projectTask.BoardState, Is.Null);
            Assert.That(projectTask.Assignee, Is.Null);
        }
    }
}
