using ticket_system_web_app.Models;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestEmployee
    {
        [Test]
        public void TestDefaultConstructor()
        {
            Employee employee = new Employee();

            Assert.That(employee.FName, Is.EqualTo(string.Empty));
            Assert.That(employee.LName, Is.EqualTo(string.Empty));
            Assert.That(employee.Username, Is.EqualTo(string.Empty));
            Assert.That(employee.Email, Is.EqualTo(string.Empty));
            Assert.That(employee.HashedPassword, Is.EqualTo(string.Empty));
            Assert.That(employee.IsActive, Is.Null);
            Assert.That(employee.IsManager, Is.Null);
            Assert.That(employee.IsAdmin, Is.Null);
            Assert.That(employee.GroupsExistingIn, Is.Empty);
            Assert.That(employee.ProjectsLeading, Is.Empty);
        }

        [Test]
        public void TestConstructor()
        {
            Employee employee = new Employee(2, "John", "Smith", "usernameJS", "passwordJS", true, true, true, "emailJS");
            Assert.That(employee.FName, Is.EqualTo("John"));
            Assert.That(employee.LName, Is.EqualTo("Smith"));
            Assert.That(employee.Username, Is.EqualTo("usernameJS"));
            Assert.That(employee.Email, Is.EqualTo("emailJS"));
            Assert.That(employee.HashedPassword, Is.EqualTo("passwordJS"));
            Assert.That(employee.IsActive, Is.True);
            Assert.That(employee.IsManager, Is.True);
            Assert.That(employee.IsAdmin, Is.True);
            Assert.That(employee.GroupsExistingIn, Is.Empty);
            Assert.That(employee.ProjectsLeading, Is.Empty);
        }

        [Test]
        public void TestConstructorDoesNotAllowNegativeEID()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Employee(-1, "Dummy", "Dummy", "Dummy", "Dummy", false, false, false, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("eId"));
            Assert.That(ex.Message, Does.Contain("negative"));
        }

        [Test]
        public void TestConstructorAllowsZeroEID()
        {
            Employee employee = new Employee(0, "Dummy", "Dummy", "Dummy", "Dummy", false, false, false, "Dummy");


            Assert.That(employee.EId, Is.EqualTo(0));
        }

        [Test]
        public void TestConstructorAllowsPositiveEID()
        {
            Employee employee = new Employee(1, "Dummy", "Dummy", "Dummy", "Dummy", false, false, false, "Dummy");

            Assert.That(employee.EId, Is.EqualTo(1));
        }

        [Test]
        public void TestConstructorDoesNotAllowNullFName()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, null, "Dummy", "Dummy", "Dummy", false, false, false, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("fName"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestConstructorDoesNotAllowEmptyFName()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, "", "Dummy", "Dummy", "Dummy", false, false, false, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("fName"));
            Assert.That(ex.Message, Does.Contain("empty"));
        }

        [Test]
        public void TestConstructorAllowsValidFName()
        {
            string fName = "TestFName";

            Employee employee = new Employee(1, fName, "Dummy", "Dummy", "Dummy", false, false, false, "Dummy");


            Assert.That(employee.FName, Is.EqualTo(fName));
        }

        [Test]
        public void TestConstructorDoesNotAllowNullLName()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, "Dummy", null, "Dummy", "Dummy", false, false, false, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("lName"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestConstructorDoesNotAllowEmptyLName()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, "Dummy", "", "Dummy", "Dummy", false, false, false, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("lName"));
            Assert.That(ex.Message, Does.Contain("empty"));
        }

        [Test]
        public void TestConstructorAllowsValidLName()
        {
            string lName = "TestLName";

            Employee employee = new Employee(1, "Dummy", lName, "Dummy", "Dummy", false, false, false, "Dummy");

            Assert.That(employee.LName, Is.EqualTo(lName));
        }

        [Test]
        public void TestConstructorDoesNotAllowNullUsername()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, "Dummy", "Dummy", null, "Dummy", false, false, false, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("username"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestConstructorDoesNotAllowEmptyUsername()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, "Dummy", "Dummy", "", "Dummy", false, false, false, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("username"));
            Assert.That(ex.Message, Does.Contain("empty"));
        }

        [Test]
        public void TestConstructorAllowsValidUsername()
        {
            string username = "TestUsername";

            Employee employee = new Employee(1, "Dummy", "Dummy", username, "Dummy", false, false, false, "Dummy");

            Assert.That(employee.Username, Is.EqualTo(username));
        }

        [Test]
        public void TestConstructorDoesNotAllowNullPassword()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, "Dummy", "Dummy", "Dummy", null, false, false, false, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("hashedPassword"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestConstructorDoesNotAllowEmptyPassword()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, "Dummy", "Dummy", "Dummy", "", false, false, false, "Dummy")
            );
            Assert.That(ex.ParamName, Is.EqualTo("hashedPassword"));
            Assert.That(ex.Message, Does.Contain("empty"));
        }

        [Test]
        public void TestConstructorDoesNotAllowNullEmail()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, "Dummy", "Dummy", "Dummy", "Dummy", false, false, false, null)
            );
            Assert.That(ex.ParamName, Is.EqualTo("email"));
            Assert.That(ex.Message, Does.Contain("null"));
        }

        [Test]
        public void TestConstructorDoesNotAllowEmptyEmail()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new Employee(1, "Dummy", "Dummy", "Dummy", "Dummy", false, false, false, "")
            );
            Assert.That(ex.ParamName, Is.EqualTo("email"));
            Assert.That(ex.Message, Does.Contain("empty"));
        }


    }
}