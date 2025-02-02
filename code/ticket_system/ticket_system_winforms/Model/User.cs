using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_winforms.DAL;

namespace ticket_system_winforms.Model
{
    public class User
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <precondition>userId != null && username != null && password != null</precondition>
        /// <postcondition>this.ID == id && this.UserID == userId && this.Username == username && this.Password == password</postcondition>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public User(int id, string userId, string username, string password)
        {
            this.ID = id;
            this.UserID = userId;
            this.Username = username;
            this.Password = password;
        }
    }
}
