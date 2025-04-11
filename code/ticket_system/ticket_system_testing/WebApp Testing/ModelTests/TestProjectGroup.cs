using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_web_app.Models;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestProjectGroup
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var projectGroup = new ProjectGroup();

            Assert.That(projectGroup.ProjectId, Is.EqualTo(0));
            Assert.That(projectGroup.GroupId, Is.EqualTo(0));
            Assert.That(projectGroup.Project, Is.Null);
            Assert.That(projectGroup.Group, Is.Null);
            Assert.That(projectGroup.Accepted, Is.False);
        }

        [Test]
        public void TestConstructor()
        {
            Project project = new Project();
            Group group = new Group();

            var projectGroup = new ProjectGroup(project, group);

            Assert.That(projectGroup.ProjectId, Is.EqualTo(project.PId));
            Assert.That(projectGroup.GroupId, Is.EqualTo(group.GId));
            Assert.That(projectGroup.Project, Is.EqualTo(project));
            Assert.That(projectGroup.Group, Is.EqualTo(group));
            Assert.That(projectGroup.Accepted, Is.False);
        }

        [Test]
        public void TestNullProject()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new ProjectGroup(null, new Group())
            );
            Assert.That(ex.ParamName, Is.EqualTo("project"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestNullGroup()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new ProjectGroup(new Project(), null)
            );
            Assert.That(ex.ParamName, Is.EqualTo("group"));
            Assert.That(ex.Message, Does.Contain("null"));
        }


    }
}
