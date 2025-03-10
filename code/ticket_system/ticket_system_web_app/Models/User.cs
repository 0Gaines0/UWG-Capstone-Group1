namespace ticket_system_web_app.Models
{
    /// <summary>
    /// User class
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public String? UserId { get; set; }
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public String? Username { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public String? Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.UserId = null;
            this.Username = null;
            this.Password = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="System.Exception">
        /// User: ID param must be greater than 0
        /// or
        /// User: UserID param must not be null or empty
        /// or
        /// User: Username param must not be null or empty
        /// or
        /// User: Password param must not be null or empty
        /// </exception>
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
