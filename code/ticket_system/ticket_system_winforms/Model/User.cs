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

        public User(int id, string userId, string username, string password)
        {
            this.ID = id;
            this.UserID = userId;
            this.Username = username;
            this.Password = password;
        }
    }
}
