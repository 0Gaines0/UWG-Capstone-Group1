using System;
using NUnit.Framework;
using ticket_system_web_app.Models;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestProject
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var project = new Project();

            Assert.That(project.PId, Is.EqualTo(0));
            Assert.That(project.PTitle, Is.EqualTo(string.Empty));
            Assert.That(project.PDescription, Is.EqualTo(string.Empty));
            Assert.That(project.ProjectLeadId, Is.EqualTo(0));
            Assert.That(project.ProjectLead, Is.Null);
            Assert.That(project.Collaborators, Is.Empty);
            Assert.That(project.ProjectBoard, Is.Null);
        }

        [Test]
        public void TestConstructor()
        {
            Employee lead = new Employee(3, "Dummy", "Dummy", "Dummy", "Dummy", false, false, false, "Dummy");

            var project = new Project(lead, "ProjectA", "DescA");

            Assert.That(project.PId, Is.EqualTo(0));
            Assert.That(project.PTitle, Is.EqualTo("ProjectA"));
            Assert.That(project.PDescription, Is.EqualTo("DescA"));
            Assert.That(project.ProjectLeadId, Is.EqualTo(lead.EId));
            Assert.That(project.ProjectLead, Is.EqualTo(lead));
            Assert.That(project.Collaborators, Is.Empty);
            Assert.That(project.ProjectBoard, Is.Null);
        }

        [Test]
        public void TestConstructorNullProjectLead()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new Project(null, "Dummy", "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("pLead"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestConstructorNullPTitle()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Project(new Employee(), null, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("pTitle"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestConstructorBlankPTitle()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Project(new Employee(), " ", "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("pTitle"));
            Assert.That(ex.Message, Does.Contain("blank"));
        }

        [Test]
        public void TestConstructorNullPDesc()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Project(new Employee(), "Dummy", null)
            );
            Assert.That(ex.ParamName, Is.EqualTo("pDesc"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestConstructorBlankPDesc()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Project(new Employee(), "Dummy", "")
            );
            Assert.That(ex.ParamName, Is.EqualTo("pDesc"));
            Assert.That(ex.Message, Does.Contain("blank"));
        }

    }
}