using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Car_Rental_System.Models;

namespace Car_Rental_System
{
    class DatabaseHelper
    {
        private string connectionString = "Server=DESKTOP-46VMP4S;Database=CarRentalDB;Trusted_Connection=True;";
        public void RegisterUser(string name, string email, string password, string role)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@Name, @Email, @Password, @Role)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("User registered successfully!");
                }
            }
        }

        public User LoginUser(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = (int)reader["UserID"],
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }
            return null; // Return null if no user found
        }

        public void AddCar(string brand, string model, int year, decimal pricePerDay)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Cars (Brand, Model, Year, PricePerDay, IsAvailable) VALUES (@Brand, @Model, @Year, @PricePerDay, 1)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Brand", brand);
                    cmd.Parameters.AddWithValue("@Model", model);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@PricePerDay", pricePerDay);
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Car added successfully!");
        }

        //Add View All Cars to the DatabaseHelper
    }
}
