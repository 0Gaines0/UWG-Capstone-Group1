using NUnit.Framework;
using System;
using ticket_system_web_app.Models;

namespace ticket_system_testing
{
    [TestFixture]
    public class UserTests
    {

        // Setup
        // dotnet add package coverlet.collector
        // dotnet tool install -g dotnet-reportgenerator-globaltool

        // dotnet test --collect:"xplat code coverage" --settings coverlet.runsettings
        // add specific filename ex. 6299b382-8b19-4328-be79-2faf3349a1f0.
        // reportgenerator -reports:"testresults\5ec82b4c-8e17-44e7-aa9c-2b9efa1600bf\coverage.cobertura.xml" -targetdir:"coverageresults" -reporttypes:html

        [Test]
        public void testConstructor()
        {
            // arrange
            int id = 1;
            string userid = "2";
            string username = "username";
            string password = "password";

            // act
            User user = new User(id, userid, username, password);

            // assert
            Assert.That(user.Id, Is.EqualTo(id));
            Assert.That(user.UserId, Is.EqualTo(userid));
            Assert.That(user.Username, Is.EqualTo(username));
            Assert.That(user.Password, Is.EqualTo(password));
        }


        [Test]
        public void testDefaultConstructor()
        {
            // act
            User user = new User();

            // assert
            Assert.That(user.Id, Is.EqualTo(0));
            Assert.That(user.UserId, Is.EqualTo(null));
            Assert.That(user.Username, Is.EqualTo(null));
            Assert.That(user.Password, Is.EqualTo(null));
        }


        [Test]
        public void testNegativeId()
        {
            // arrange
            int id = -1;
            string userid = "2";
            string username = "username";
            string password = "password";

            // act
            // assert
            Assert.Throws<Exception>(() => new User(id, userid, username, password));
        }

        [Test]
        public void testEmptyUserId()
        {
            // arrange
            int id = 1;
            string userid = "";
            string username = "username";
            string password = "password";

            // act
            // assert
            Assert.Throws<Exception>(() => new User(id, userid, username, password));
        }

        [Test]
        public void testEmptyUsername()
        {
            // arrange
            int id = 1;
            string userid = "2";
            string username = "";
            string password = "password";

            // act
            // assert
            Assert.Throws<Exception>(() => new User(id, userid, username, password));
        }

        [Test]
        public void testEmptyPassword()
        {
            // arrange
            int id = 1;
            string userid = "2";
            string username = "username";
            string password = "";

            // act
            // assert
            Assert.Throws<Exception>(() => new User(id, userid, username, password));
        }

        [Test]
        public void testNullUserId()
        {
            // arrange
            int id = 1;
            string userid = null;
            string username = "username";
            string password = "password";

            // act
            // assert
            Assert.Throws<Exception>(() => new User(id, userid, username, password));
        }

        [Test]
        public void testNullUsername()
        {
            // arrange
            int id = 1;
            string userid = "2";
            string username = null;
            string password = "password";

            // act
            // assert
            Assert.Throws<Exception>(() => new User(id, userid, username, password));
        }

        [Test]
        public void testNullPassword()
        {
            // arrange
            int id = 1;
            string userid = "2";
            string username = "username";
            string password = null;

            // act
            // assert
            Assert.Throws<Exception>(() => new User(id, userid, username, password));
        }
    }


}