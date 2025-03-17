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
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            User user = dbHelper.LoginUser(email, password);

            if (user != null)
            {
                Console.WriteLine($"\nWelcome, {user.Name}! You are logged in as {user.Role}.");

                if (user.Role == "Admin")
                    AdminMenu();
                else
                    CustomerMenu();
            }
            else
            {
                Console.WriteLine("Invalid email or password. Please try again.");
            }
        }

        static void AdminMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. Manage Cars");
                Console.WriteLine("2. View Rentals");
                Console.WriteLine("3. Logout");
                Console.WriteLine("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Car management feature coming soon");
                        break;
                    case "2":
                        Console.WriteLine("Rental management feature coming soon");
                        break;
                    case "3":
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void CustomerMenu()
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
                        Console.WriteLine("Car viewing feature coming soon...");
                        break;
                    case "2":
                        Console.WriteLine("Car rental feature coming soon...");
                        break;
                    case "3":
                        Console.WriteLine("Rental history feature coming soon...");
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
    }
}
