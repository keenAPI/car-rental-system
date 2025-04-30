using System.Threading.Channels;
using Car_Rental_System.Models;


namespace Car_Rental_System
{
    internal class Program
    {
        static DatabaseHelper dbHelper = new DatabaseHelper();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n--- Welcome to my Car Rental System ---");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option by number: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void Register()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            Console.Write("Enter Role: ");
            string role = Console.ReadLine();

            dbHelper.RegisterUser(name, email, password, role);
        }

        static void Login()
        {
            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            User loggedInUser = dbHelper.LoginUser(email, password);

            if (loggedInUser != null)
            {
                Console.WriteLine($"\nLogin successful! Welcome, {loggedInUser.Email} ({loggedInUser.Role}).");

                if (loggedInUser.Role == "Admin")
                {
                    AdminMenu(loggedInUser);
                }
                else if (loggedInUser.Role == "Customer")
                {
                    CustomerMenu(loggedInUser);
                }
                else
                {
                    Console.WriteLine("Unknown role. Cannot proceed.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid email or password.");
            }
        }


        static void AdminMenu(User loggedInUser)
        {
            while (true)
            {
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. Add Car");
                Console.WriteLine("2. View Cars");
                Console.WriteLine("3. Edit Car");
                Console.WriteLine("4. Delete Car");
                Console.WriteLine("5. Logout");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCar();
                        break;
                    case "2":
                        ViewCars();
                        break;
                    case "3":
                        EditCar();
                        break;
                    case "4":
                        DeleteCar();
                        break;
                    case "5":
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void CustomerMenu(User loggedInUser)
        {
            while (true)
            {
                Console.WriteLine("\n--- Customer Menu ---");
                Console.WriteLine("1. View Available Cars");
                Console.WriteLine("2. Rent a Car");
                Console.WriteLine("3. View my Rentals");
                Console.WriteLine("4. Logout");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAvailableCars();
                        break;
                    case "2":
                        RentCar(loggedInUser.UserID);
                        break;
                    case "3":
                        ViewMyRentals(loggedInUser.UserID);
                        break;
                    case "4":
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }



        static void AddCar()
        {
            Console.Write("Enter Brand: ");
            string brand = Console.ReadLine();
            Console.Write("Enter Model: ");
            string model = Console.ReadLine();
            Console.Write("Enter Year: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Enter Price Per Day: ");
            decimal pricePerDay = decimal.Parse(Console.ReadLine());

            dbHelper.AddCar(brand, model, year, pricePerDay);
        }

        static void ViewCars()
        {
            List<Car> cars = dbHelper.GetAllCars();
            Console.WriteLine("\n--- Available Cars ---");
            foreach (var car in cars)
            {
                Console.WriteLine($"ID: {car.CarID} | {car.Brand} {car.Model} ({car.Year}) - ${car.PricePerDay}/day | Available: {(car.IsAvailable ? "Yes" : "No")}");
            }
        }

        static void EditCar()
        {
            Console.Write("Enter Car ID to edit: ");
            int carID = int.Parse(Console.ReadLine());

            Console.Write("Enter New Brand: ");
            string brand = Console.ReadLine();
            Console.Write("Enter New Model: ");
            string model = Console.ReadLine();
            Console.Write("Enter New Year: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Enter New Price Per Day: ");
            decimal pricePerDay = decimal.Parse(Console.ReadLine());

            dbHelper.UpdateCar(carID, brand, model, year, pricePerDay);
        }

        static void DeleteCar()
        {
            Console.Write("Enter Car ID to delete: ");
            int carID = int.Parse(Console.ReadLine());
            dbHelper.DeleteCar(carID);
        }

        static void ViewAvailableCars()
        {
            List<Car> cars = dbHelper.GetAllCars();
            Console.WriteLine("\n--- Available Cars ---");
            foreach (var car in cars.Where(c => c.IsAvailable))
            {
                Console.WriteLine($"ID: {car.CarID} | {car.Brand} {car.Model} ({car.Year}) - ${car.PricePerDay}/day");
            }
        }

        static void RentCar(int userId)
        {
            ViewAvailableCars();

            Console.Write("Enter the Car ID to rent: ");
            int carId = int.Parse(Console.ReadLine());

            dbHelper.RentCar(userId, carId);
        }

        static void ViewMyRentals(int userId)
        {
            List<Rental> rentals = dbHelper.GetMyRentals(userId);
            Console.WriteLine("\n--- My Rentals ---");
            foreach (var rental in rentals)
            {
                Console.WriteLine($"Rental ID: {rental.RentalID} | Car ID: {rental.CarID} | Rented On: {rental.RentalDate.ToShortDateString()} | Returned: {(rental.ReturnDate.HasValue ? rental.ReturnDate.Value.ToShortDateString() : "Not yet")}");
            }
        }
    }
}
