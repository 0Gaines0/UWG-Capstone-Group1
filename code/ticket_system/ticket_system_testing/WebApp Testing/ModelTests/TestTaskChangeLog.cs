using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestTaskChangeLog
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var taskChangeLog = new TaskChangeLog();

            Assert.That(taskChangeLog.TaskId, Is.EqualTo(0));
            Assert.That(taskChangeLog.ChangeId, Is.EqualTo(0));
            Assert.That(taskChangeLog.Task, Is.Null);
            Assert.That(taskChangeLog.TaskChange, Is.Null);
        }
    }
}
