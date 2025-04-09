using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestProjectBoard
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var boardState = new ProjectBoard();

            Assert.That(boardState.ProjectId, Is.EqualTo(0));
            Assert.That(boardState.BoardId, Is.EqualTo(0));
            Assert.That(boardState.Project, Is.Null);
            Assert.That(boardState.States, Is.Empty);
        }
    }
}
