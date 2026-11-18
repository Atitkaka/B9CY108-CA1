using System;
using System.Collections.Generic;

namespace StudentRecords
{
    class Program
    {
        // Dictionary -> key = Student ID, value = Student object
        // this lets us get a student straight away using the ID
        static Dictionary<int, Student> students = new Dictionary<int, Student>();

        // the 5 modules every student takes (an array because the list of modules is fixed)
        static string[] moduleNames =
        {
            "Advanced Programming",
            "Adv. Databases",
            "Networking & Security",
            "Cloud Computing",
            "Research Methods"
        };

        static void Main(string[] args)
        {
            LoadStudents();
            Console.WriteLine(students.Count + " student records loaded.");

            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("========================================");
                Console.WriteLine("        STUDENT RECORDS SYSTEM");
                Console.WriteLine("========================================");
                Console.WriteLine("  1. Search student by ID");
                Console.WriteLine("  2. Show list of all students");
                Console.WriteLine("  0. Exit");
                Console.WriteLine("========================================");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        SearchStudent();
                        break;
                    case "2":
                        ShowAllStudents();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        static void SearchStudent()
        {
            Console.Write("Enter Student ID: ");
            string input = Console.ReadLine();

            int id;
            if (!int.TryParse(input, out id))
            {
                Console.WriteLine("Student ID should be a number (e.g. 20001010).");
                return;
            }

            Student student;
            if (students.TryGetValue(id, out student))
            {
                student.DisplayDetails();
            }
            else
            {
                Console.WriteLine("No student found with ID " + id);
            }
        }

        static void ShowAllStudents()
        {
            Console.WriteLine(string.Format("{0,-12} {1,-25} {2,-8}", "Student ID", "Name", "Average"));
            Console.WriteLine("----------------------------------------------");
            foreach (KeyValuePair<int, Student> item in students)
            {
                Console.WriteLine(string.Format("{0,-12} {1,-25} {2,-8}", item.Key, item.Value.Name, item.Value.GetAverage().ToString("0.0")));
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Total: " + students.Count + " students");
        }

        // helper method to create a student with 5 marks and put it in the dictionary
        static void AddStudent(int id, string name, int[] marks)
        {
            if (students.ContainsKey(id))
            {
                Console.WriteLine("Duplicate ID " + id + " - student not added");
                return;
            }

            Student s = new Student(id, name);
            for (int i = 0; i < moduleNames.Length; i++)
            {
                s.AddModule(moduleNames[i], marks[i]);
            }
            students.Add(id, s);
        }

        // the 20 student records
        static void LoadStudents()
        {
            AddStudent(20001010, "Charles Severance", new int[] { 70, 65, 55, 62, 74 });
            AddStudent(20001011, "Aoife Murphy",      new int[] { 82, 77, 69, 71, 80 });
            AddStudent(20001012, "Ciaran O'Brien",    new int[] { 58, 61, 49, 55, 63 });
            AddStudent(20001013, "Priya Sharma",      new int[] { 91, 84, 78, 88, 75 });
            AddStudent(20001014, "Liam Walsh",        new int[] { 45, 52, 38, 60, 57 });
            AddStudent(20001015, "Sofia Rossi",       new int[] { 67, 72, 64, 59, 70 });
            AddStudent(20001016, "Arjun Patel",       new int[] { 74, 68, 81, 77, 66 });
            AddStudent(20001017, "Niamh Byrne",       new int[] { 63, 59, 71, 68, 62 });
            AddStudent(20001018, "Lucas Silva",       new int[] { 55, 47, 62, 51, 58 });
            AddStudent(20001019, "Emma Kelly",        new int[] { 79, 85, 73, 80, 82 });
            AddStudent(20001020, "Mohammed Ali",      new int[] { 61, 66, 58, 70, 64 });
            AddStudent(20001021, "Sean Doyle",        new int[] { 42, 39, 50, 46, 55 });
            AddStudent(20001022, "Chen Wei",          new int[] { 88, 92, 79, 85, 90 });
            AddStudent(20001023, "Grace Ryan",        new int[] { 69, 64, 60, 72, 67 });
            AddStudent(20001024, "Rahul Verma",       new int[] { 53, 58, 66, 61, 49 });
            AddStudent(20001025, "Ana Popescu",       new int[] { 76, 70, 68, 74, 79 });
            AddStudent(20001026, "Conor McCarthy",    new int[] { 35, 44, 41, 52, 48 });
            AddStudent(20001027, "Fatima Khan",       new int[] { 84, 78, 86, 81, 73 });
            AddStudent(20001028, "Daniel Nowak",      new int[] { 60, 57, 63, 65, 59 });
            AddStudent(20001029, "Sarah O'Connor",    new int[] { 72, 69, 75, 70, 68 });
        }
    }
}
