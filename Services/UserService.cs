using System;
using System.Data;
using System.Data.SqlClient;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public class UserService
    {
        public static bool ValidateUser(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
            SqlParameter[] parameters = {
                new SqlParameter("@username", username),
                new SqlParameter("@password", password)
            };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }

        public static bool RegisterUser(string username, string password, string email)
        {
            try
            {
                string query = "INSERT INTO Users (Username, Password, Email) VALUES (@username, @password, @email)";
                SqlParameter[] parameters = {
                    new SqlParameter("@username", username),
                    new SqlParameter("@password", password),
                    new SqlParameter("@email", email)
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool UserExists(string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @username";
            SqlParameter[] parameters = {
                new SqlParameter("@username", username)
            };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }
    }
}
