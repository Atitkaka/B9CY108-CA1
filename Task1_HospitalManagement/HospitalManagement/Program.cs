using System;
using System.Collections.Generic;

namespace HospitalManagement
{
    class Program
    {
        static StaffManager manager = new StaffManager();

        static void Main(string[] args)
        {
            AddSampleStaff();

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddDoctor();
                        break;
                    case "2":
                        AddNurse();
                        break;
                    case "3":
                        AddAdminStaff();
                        break;
                    case "4":
                        manager.DisplayAllStaff();
                        break;
                    case "5":
                        Search();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Exiting the program. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option, please enter a number from the menu.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("           STAFF MANAGEMENT");
            Console.WriteLine("========================================");
            Console.WriteLine("  1. Add Doctor");
            Console.WriteLine("  2. Add Nurse");
            Console.WriteLine("  3. Add Administrative Staff");
            Console.WriteLine("  4. Display All Staff");
            Console.WriteLine("  5. Search Staff");
            Console.WriteLine("  0. Exit");
            Console.WriteLine("========================================");
            Console.Write("Enter your choice: ");
        }

        // a few staff already in the system so display/search can be tested straight away
        static void AddSampleStaff()
        {
            manager.AddStaff(new Doctor("John Murphy", "D101", "Cardiology", "Cardiologist", "IMC-45821", 12));
            manager.AddStaff(new Nurse("Mary Byrne", "N201", "Emergency", "A&E", "Night"));
            manager.AddStaff(new AdminStaff("Sean Kelly", "A301", "Administration", "Receptionist", "G-12"));
        }

        static void AddDoctor()
        {
            Console.WriteLine("--- Add Doctor ---");
            string id = ReadNewStaffId();
            string name = ReadText("Enter name: ");
            string dept = ReadText("Enter department: ");
            string spec = ReadText("Enter specialization: ");
            string licence = ReadText("Enter licence number: ");
            int years = ReadNumber("Enter years of experience: ", 0, 60);

            try
            {
                Doctor d = new Doctor(name, id, dept, spec, licence, years);
                manager.AddStaff(d);
                Console.WriteLine("Doctor added successfully!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void AddNurse()
        {
            Console.WriteLine("--- Add Nurse ---");
            string id = ReadNewStaffId();
            string name = ReadText("Enter name: ");
            string dept = ReadText("Enter department: ");
            string ward = ReadText("Enter ward: ");

            string shift = ReadText("Enter shift (Day/Night): ");
            while (shift.ToLower() != "day" && shift.ToLower() != "night")
            {
                Console.WriteLine("Shift must be Day or Night.");
                shift = ReadText("Enter shift (Day/Night): ");
            }

            try
            {
                Nurse n = new Nurse(name, id, dept, ward, shift);
                manager.AddStaff(n);
                Console.WriteLine("Nurse added successfully!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void AddAdminStaff()
        {
            Console.WriteLine("--- Add Administrative Staff ---");
            string id = ReadNewStaffId();
            string name = ReadText("Enter name: ");
            string dept = ReadText("Enter department: ");
            string title = ReadText("Enter job title (e.g. Receptionist): ");
            string office = ReadText("Enter office number: ");

            try
            {
                AdminStaff a = new AdminStaff(name, id, dept, title, office);
                manager.AddStaff(a);
                Console.WriteLine("Administrative staff added successfully!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void Search()
        {
            Console.WriteLine("--- Search Staff ---");
            string text = ReadText("Enter Staff ID or name to search: ");

            bool found = manager.SearchStaff(text);
            if (found)
            {
                List<Staff> results = manager.FindMatches(text);
                Console.WriteLine("Search successful - " + results.Count + " record(s) found:");
                foreach (Staff s in results)
                {
                    s.DisplayInfo();
                }
            }
            else
            {
                Console.WriteLine("Search failed - no staff found for \"" + text + "\"");
            }
        }

        // ---------- helper methods for reading input ----------

        // keeps asking until the user types something
        static string ReadText(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("This field cannot be empty. " + message);
                input = Console.ReadLine();
            }
            return input.Trim();
        }

        // keeps asking until the ID is not already used
        static string ReadNewStaffId()
        {
            string id = ReadText("Enter staff ID: ");
            while (manager.IdExists(id))
            {
                Console.WriteLine("Staff ID " + id.ToUpper() + " already exists, try another one.");
                id = ReadText("Enter staff ID: ");
            }
            return id;
        }

        // keeps asking until a valid whole number between min and max is entered
        static int ReadNumber(string message, int min, int max)
        {
            int number;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out number) || number < min || number > max)
            {
                Console.Write("Please enter a number between " + min + " and " + max + ": ");
            }
            return number;
        }
    }
}
