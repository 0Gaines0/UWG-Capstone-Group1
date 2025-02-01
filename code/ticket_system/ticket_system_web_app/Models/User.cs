namespace ticket_system_web_app.Models
{
    public class User
    {
        public int Id { get; set; }

        public String? UserId { get; set; }
        public String? Username { get; set; }
        public String? Password { get; set; }

        public User()
        {
            this.UserId = null;
            this.Username = null;
            this.Password = null;
        }

        public User(int id, String userId, String username, String password)
        {
            if (id < 0)
            {
                throw new Exception("User: ID param must be greater than 0");
            }
            if (userId == null ||  userId == String.Empty)
            {
                throw new Exception("User: UserID param must not be null or empty");
            }
            if (username == null || username == String.Empty)
            {
                throw new Exception("User: Username param must not be null or empty");
            }
            if (password == null || password == String.Empty)
            {
                throw new Exception("User: Password param must not be null or empty");
            }
            this.Id = id;
            this.UserId = userId;
            this.Username = username;
            this.Password = password;
        }
    }
}
