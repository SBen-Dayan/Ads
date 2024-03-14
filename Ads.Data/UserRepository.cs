using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (Name, Email, Password) VALUES (@name, @email, @hash)";
            cmd.Parameters.AddRange(new SqlParameter[]
            {
                new("@name", user.Name  != null ? user.Name : DBNull.Value),
                new("@email", user.Email),
                new("@hash", BCrypt.Net.BCrypt.HashPassword(user.Password))
            });
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public bool Login(string email, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 Password FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return false;
            }

            var hashPassword = reader.GetValue<string>("Password");

            if (hashPassword == null || !BCrypt.Net.BCrypt.Verify(password, hashPassword))
            {
                return false;
            }
            return true;

        }

        public int GetId(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 Id FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            return (int)cmd.ExecuteScalar();
        }
    }
}
