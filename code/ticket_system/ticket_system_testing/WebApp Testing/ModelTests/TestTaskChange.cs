using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ticket_system_web_app.Models;

    namespace ticket_system_testing.WebApp_Testing.ModelTests
    {
        [TestFixture]
        public class TestTaskChange
        {
            [Test]
            public void TestDefaultConstructor()
            {
                var taskChange = new TaskChange();

                Assert.That(taskChange.ChangeId, Is.EqualTo(0));
                Assert.That(taskChange.Type, Is.EqualTo(string.Empty));
                Assert.That(taskChange.ChangedDate, Is.EqualTo(DateTime.MinValue));
                Assert.That(taskChange.PreviousValue, Is.Null);
                Assert.That(taskChange.NewValue, Is.EqualTo(string.Empty));
                Assert.That(taskChange.AssigneeId, Is.Null);
                Assert.That(taskChange.Assignee, Is.Null);
            }
        }
    }

}
