using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestEditEmployeeRequest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var editEmployeeRequest = new EditEmployeeRequest();

            Assert.That(editEmployeeRequest.IsAdmin, Is.False);
            Assert.That(editEmployeeRequest.OriginalUsername, Is.Null);
            Assert.That(editEmployeeRequest.FirstName, Is.Null);
            Assert.That(editEmployeeRequest.LastName, Is.Null);
            Assert.That(editEmployeeRequest.Username, Is.Null);
            Assert.That(editEmployeeRequest.Email, Is.Null);
        }
    }
}
