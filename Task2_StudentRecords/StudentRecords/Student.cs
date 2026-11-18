using System;
using System.Collections.Generic;

namespace StudentRecords
{
    public class Student
    {
        public int StudentId { get; private set; }
        public string Name { get; private set; }

        // a list is used to keep the 5 module results of the student in order
        public List<ModuleResult> Modules { get; private set; }

        public Student(int studentId, string name)
        {
            StudentId = studentId;
            Name = name;
            Modules = new List<ModuleResult>();
        }

        public void AddModule(string moduleName, int marks)
        {
            Modules.Add(new ModuleResult(moduleName, marks));
        }

        public double GetAverage()
        {
            if (Modules.Count == 0)
            {
                return 0;
            }
            int total = 0;
            foreach (ModuleResult m in Modules)
            {
                total += m.Marks;
            }
            return (double)total / Modules.Count;
        }

        // prints the student record in a table like the one in the assignment
        public void DisplayDetails()
        {
            Console.WriteLine("=================================================================");
            Console.WriteLine("Student ID   : " + StudentId);
            Console.WriteLine("Student Name : " + Name);
            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine(string.Format("{0,-8} {1,-28} {2,-8} {3,-6}", "Module", "Module Name", "Marks", "Grade"));
            Console.WriteLine("-----------------------------------------------------------------");

            int number = 1;
            foreach (ModuleResult m in Modules)
            {
                Console.WriteLine(string.Format("{0,-8} {1,-28} {2,-8} {3,-6}", number, m.ModuleName, m.Marks, m.Grade));
                number++;
            }

            Console.WriteLine("-----------------------------------------------------------------");
            double avg = GetAverage();
            Console.WriteLine("Average Marks: " + avg.ToString("0.0") + "  (Overall Grade: " + ModuleResult.CalculateGrade((int)Math.Round(avg)) + ")");
            Console.WriteLine("=================================================================");
        }
    }
}
