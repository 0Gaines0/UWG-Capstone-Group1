using ticket_system_winforms.Model;

namespace ticket_system_winforms
{
    [TestFixture]
    internal class UserTest
    {
        [Test]
        public void testDefaultConstructor()
        {
            int id = 1;
            String userId = "1";
            String username = "username";
            String password = "password";

            User user = new User(id, userId, username, password);

            Assert.That(user.ID, Is.EqualTo(id));
            Assert.That(user.UserID, Is.EqualTo(userId));
            Assert.That(user.Username, Is.EqualTo(username));
            Assert.That(user.Password, Is.EqualTo(password));
        }
    }
}