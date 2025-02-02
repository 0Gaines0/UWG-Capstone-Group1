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
    internal class UserDAL
    {
        internal void CreateUser(string userId, string username, string password)
        {
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

        internal User RetrieveUser(int id)
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

        internal User RetrieveUser(string username, string password)
        {
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

        internal IList<User> RetrieveAllUsers()
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

        internal void UpdateUser(int id, string userId, string username, string password)
        {
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
    }
}
