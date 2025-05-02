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
        private string connectionString = "Server=DESKTOP-46VMP4S;Database=CarRentalDB;Trusted_Connection=True;TrustServerCertificate=True;";

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
                                Password = reader["Password"].ToString(),
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

        public List<Car> GetAllCars()
        {
            List<Car> cars = new List<Car>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Cars";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car
                            {
                                CarID = (int)reader["CarID"],
                                Brand = reader["Brand"].ToString(),
                                Model = reader["Model"].ToString(),
                                Year = (int)reader["Year"],
                                PricePerDay = (decimal)reader["PricePerDay"],
                                IsAvailable = (bool)reader["IsAvailable"]
                            });
                        }
                    }
                }
            }
            return cars;
        }

        public void UpdateCar(int carID, string brand, string model, int year, decimal pricePerDay)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Cars SET Brand = @Brand, Model = @Model, Year = @Year, PricePerDay = @PricePerDay WHERE CarID = @CarID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Brand", brand);
                    cmd.Parameters.AddWithValue("@Model", model);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@PricePerDay", pricePerDay);
                    cmd.Parameters.AddWithValue("@CarID", carID);
                    cmd.ExecuteNonQuery();
                }

            }
            Console.WriteLine("Car updated successfully!");
        }

        public void DeleteCar(int carID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Cars WHERE CarID = @CarID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CarID", carID);
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Car deleted successfully!");
        }

        public void RentCar(int userId, int carId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //First check if car is available
                string checkQuery = "SELECT IsAvailable FROM Cars WHERE CarID = @CarID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@CarID", carId);
                    var isAvailable = (bool)checkCmd.ExecuteScalar();
                    if (!isAvailable)
                    {
                        Console.WriteLine("Car is not available.");
                        return;
                    }
                }

                //Rent the car
                string insertQuery = "INSERT INTO Rentals (UserID, CarID, RentalDate) VALUES (@UserID, @CarID, @RentalDate)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@CarID", carId);
                    cmd.Parameters.AddWithValue("@RentalDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }

                //Update car availability
                string updateCarQuery = "UPDATE Cars SET IsAvailable = 0 WHERE CarID = @CarID";
                using (SqlCommand updateCmd = new SqlCommand(updateCarQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@CarID", carId);
                    updateCmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Car rented successfully!");
        }

        public List<Rental> GetMyRentals(int userId)
        {
            List<Rental> rentals = new List<Rental>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Rentals WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            rentals.Add(new Rental
                            {
                                RentalID = (int)reader["RentalID"],
                                UserID = (int)reader["UserID"],
                                CarID = (int)reader["CarID"],
                                RentalDate = (DateTime)reader["RentalDate"],
                                ReturnDate = reader["ReturnDate"] == DBNull.Value ? null : (DateTime?)reader["ReturnDate"]
                            });
                        }
                    }
                }
            }
            return rentals;
        }

        public void ReturnCar(int userId, int carId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Step 1: Update rental record with return date
                string updateRental = @" UPDATE Rentals SET ReturnDate = @ReturnDate WHERE UserID = @UserID AND CarID = @CarID AND ReturnDate IS NULL";

                using (SqlCommand cmd = new SqlCommand(updateRental, conn))
                {
                    cmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@CarID", carId);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        Console.WriteLine("No active rental found for that car.");
                        return;
                    }
                }

                // Step 2: Set the car as available again
                string updateCar = "UPDATE Cars SET IsAvailable = 1 WHERE CarID = @CarID";
                using (SqlCommand cmd = new SqlCommand(updateCar, conn))
                {
                    cmd.Parameters.AddWithValue("@CarID", carId);
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Car returned successfully!");
            }
        }

        public void MakePayment(int rentalId, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Payments (RentalID, Amount, DatePaid) VALUES (@RentalID, @Amount, @DatePaid)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RentalID", rentalId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@DatePaid", DateTime.Now);
                    cmd.ExecuteNonQuery();

                }
            }

            Console.WriteLine("Payment successful!");
        }
    }
}
