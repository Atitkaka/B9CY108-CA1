using System;

namespace StudentRecords
{
    // holds the result of one module (module name, marks and grade)
    public class ModuleResult
    {
        public string ModuleName { get; private set; }
        public int Marks { get; private set; }
        public string Grade { get; private set; }

        public ModuleResult(string moduleName, int marks)
        {
            if (marks < 0 || marks > 100)
            {
                throw new ArgumentException("Marks must be between 0 and 100");
            }
            ModuleName = moduleName;
            Marks = marks;
            Grade = CalculateGrade(marks); // grade is worked out from the marks
        }

        // Grading scale I used (it matches the example in the brief: 70 = B+, 65 = B, 55 = C)
        // 80-100 = A, 70-79 = B+, 60-69 = B, 50-59 = C, 40-49 = D, below 40 = F
        public static string CalculateGrade(int marks)
        {
            if (marks >= 80) return "A";
            else if (marks >= 70) return "B+";
            else if (marks >= 60) return "B";
            else if (marks >= 50) return "C";
            else if (marks >= 40) return "D";
            else return "F";
        }
    }
}
