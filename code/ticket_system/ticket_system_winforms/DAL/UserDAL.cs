using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticket_system_winforms.Model;

namespace ticket_system_winforms.DAL
{
    /// <summary>
    /// The Data Access Object for the User class.
    /// </summary>
    /// <author>Jacob Wilson</author>
    /// <version>Spring 2025</version>
    public class UserDAL
    {
        /// <summary>
        /// Creates a new user with the specified information.
        /// </summary>
        /// <precondition>userId != null && username != null && password != null</precondition>
        /// <postcondition>true</postcondition>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void CreateUser(string userId, string username, string password)
        {
            if (userId == null)   { throw new ArgumentNullException("userId"); }
            if (username == null) { throw new ArgumentNullException("username"); }
            if (password == null) { throw new ArgumentNullException("password"); }

            string query = "INSERT INTO User(user_id, username, password) VALUES (@UserID, @Username, @Password)";

            using (SqlConnection connection = new SqlConnection(DBConfig.ConnectionString)) {
            using (SqlCommand command = new SqlCommand(query, connection)) {
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                command.ExecuteNonQuery();
            }
            }
        }

        /// <summary>
        /// Retrieves the specified user from the database and returns it.
        /// </summary>
        /// <precondition>true</precondition>
        /// <postcondition>true</postcondition>
        /// <param name="id">The user's id.</param>
        /// <returns>The user</returns>
        public User RetrieveUser(int id)
        {
            User result = null;
            string query =  "SELECT u.id, u.user_id, u.username, u.password " +
                            "FROM User u " +
                            "WHERE u.id = @ID";

            using (SqlConnection connection = new SqlConnection(DBConfig.ConnectionString)) {
            using (SqlCommand command = new SqlCommand(query, connection)) {
            using (SqlDataReader reader = command.ExecuteReader()) {
                command.Parameters.AddWithValue("@ID", id);

                connection.Open();
                while (reader.Read())
                {
                    int userId = reader.GetInt32(0);
                    string userUserID = reader.GetString(1);
                    string userUsername = reader.GetString(2);
                    string userPassword = reader.GetString(3);
                    result = new User(userId, userUserID, userUsername, userPassword);
                }
            }
            }
            }

            return result;
        }

        /// <summary>
        /// Retrieves the specified user from the database and returns it.
        /// </summary>
        /// <precondition>username != null && password != null</precondition>
        /// <postcondition>true</postcondition>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The user.</returns>
        public User RetrieveUser(string username, string password)
        {
            if (username == null) { throw new ArgumentNullException("username"); }
            if (password == null) { throw new ArgumentNullException("password"); }

            User result = null;
            string query =  "SELECT u.id, u.user_id, u.username, u.password " +
                            "FROM User u " +
                            "WHERE u.username = \"@Username\" AND u.password = \"@Password\"";

            using (SqlConnection connection = new SqlConnection(DBConfig.ConnectionString)) {
            using (SqlCommand command = new SqlCommand(query, connection)) {
            using (SqlDataReader reader = command.ExecuteReader()) {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                while (reader.Read())
                {
                    int userId = reader.GetInt32(0);
                    string userUserID = reader.GetString(1);
                    string userUsername = reader.GetString(2);
                    string userPassword = reader.GetString(3);
                    result = new User(userId, userUserID, userUsername, userPassword);
                }
            }
            }
            }

            return result;
        }

        /// <summary>
        /// Retrieves all users from the database and returns them.
        /// </summary>
        /// <precondition>true</precondition>
        /// <postcondition>true</postcondition>
        /// <returns>The list of users</returns>
        public IList<User> RetrieveAllUsers()
        {
            IList<User> result = new List<User>();
            string query =  "SELECT u.id, u.user_id, u.username, u.password " +
                            "FROM User u";

            using (SqlConnection connection = new SqlConnection(DBConfig.ConnectionString)) {
            using (SqlCommand command = new SqlCommand(query, connection)) {
            using (SqlDataReader reader = command.ExecuteReader()) {
                connection.Open();
                while (reader.Read())
                {
                    int userId = reader.GetInt32(0);
                    string userUserID = reader.GetString(1);
                    string userUsername = reader.GetString(2);
                    string userPassword = reader.GetString(3);
                    result.Add(new User(userId, userUserID, userUsername, userPassword));
                }
            }
            }
            }
            
            return result;
        }

        /// <summary>
        /// Updates the user having the specified id with the specified information.
        /// </summary>
        /// <precondition>userId != null && username != null && password != null</precondition>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void UpdateUser(int id, string userId, string username, string password)
        {
            if (userId == null)   { throw new ArgumentNullException("userId"); }
            if (username == null) { throw new ArgumentNullException("username"); }
            if (password == null) { throw new ArgumentNullException("password"); }

            string query =  "UPDATE User " +
                            "SET id = @ID, user_id = \"@UserID\", username = \"@Username\", password = \"@Password\" " +
                            "WHERE id = @ID";

            using (SqlConnection connection = new SqlConnection(DBConfig.ConnectionString)) {
            using (SqlCommand command = new SqlCommand(query, connection)) {
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                command.ExecuteNonQuery();
            }
            }
        }

        /// <summary>
        /// Deletes the user with the specified id.
        /// </summary>
        /// <precondition>true</precondition>
        /// <postcondition>true</postcondition>
        /// <param name="id">The identifier.</param>
        public void DeleteUser(int id)
        {
            string query =  "DELETE " +
                            "FROM User " +
                            "WHERE id = @ID";

            using (SqlConnection connection = new SqlConnection(DBConfig.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
