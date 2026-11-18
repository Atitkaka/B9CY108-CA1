using System;

namespace HospitalManagement
{
    public class Nurse : Staff
    {
        private string shift;

        public string Ward { get; set; }

        // only Day or Night shift is allowed
        public string Shift
        {
            get { return shift; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Shift must be Day or Night");
                }
                string s = value.Trim().ToLower();
                if (s == "day")
                {
                    shift = "Day";
                }
                else if (s == "night")
                {
                    shift = "Night";
                }
                else
                {
                    throw new ArgumentException("Shift must be Day or Night");
                }
            }
        }

        public Nurse(string name, string staffId, string department, string ward, string shift)
            : base(name, staffId, department)
        {
            Ward = ward;
            Shift = shift;
        }

        public override string GetRole()
        {
            return "Nurse";
        }

        public override string PerformDuties()
        {
            return "Cares for patients in " + Ward + " ward, gives medication and checks vitals (" + Shift + " shift)";
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("Ward       : " + Ward);
            Console.WriteLine("Shift      : " + Shift);
            PrintDuties();
        }

        public string CheckVitals(string patientName)
        {
            return "Nurse " + Name + " checked the vitals of " + patientName;
        }
    }
}
