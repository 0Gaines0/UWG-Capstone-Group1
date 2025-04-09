using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_testing.WebApp_Testing.ModelTests.RequestObjTests
{
    [TestFixture]
    public class TestCreateEmployeeRequest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var createEmployeeRequest = new CreateEmployeeRequest();

            Assert.That(createEmployeeRequest.IsAdmin, Is.False);
            Assert.That(createEmployeeRequest.FirstName, Is.Null);
            Assert.That(createEmployeeRequest.LastName, Is.Null);
            Assert.That(createEmployeeRequest.Username, Is.Null);
            Assert.That(createEmployeeRequest.Password, Is.Null);
            Assert.That(createEmployeeRequest.Email, Is.Null);
        }
    }
}
