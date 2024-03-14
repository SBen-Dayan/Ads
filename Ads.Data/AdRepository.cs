using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data
{
    public class AdRepository
    {
        private readonly string _connectionString;

        public AdRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Ad> GetAds()
        {
            return GetAdsInternal(null);
        }

        public List<Ad> GetAds(int userId)
        {
            return GetAdsInternal(userId);
        }

        private List<Ad> GetAdsInternal(int? userId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            var sql = @"SELECT a.*, u.Name FROM Ads a
                        JOIN Users u ON u.Id = a.UserId";
            if (userId != null)
            {
                sql += " WHERE UserId = @id";
                cmd.Parameters.AddWithValue("@id", userId);
            }
            cmd.CommandText = sql;
            connection.Open();
            using var reader = cmd.ExecuteReader();

            List<Ad> ads = new();
            while (reader.Read())
            {
                ads.Add(new()
                {
                    Id = reader.GetValue<int>("Id"),
                    UserId = reader.GetValue<int>("UserId"),
                    Name = reader.GetValue<string>("Name"),
                    PhoneNumber = reader.GetValue<string>("PhoneNumber"),
                    Date = reader.GetValue<DateTime>("Date"),
                    Details = reader.GetValue<string>("Details")
                });
            }
            return ads;
        }

        public void Insert(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Ads (PhoneNumber, Date, Details, UserId)
                                VALUES (@number, @date, @details, @userId)
                                SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddRange(new SqlParameter[]
            {
                new("@number", ad.PhoneNumber),
                new("@date", ad.Date),
                new("@details", ad.Details),
                new("@userId", ad.UserId)
            });
            connection.Open();
            ad.Id = (int)(decimal)cmd.ExecuteScalar();
        }

        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Ads WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
