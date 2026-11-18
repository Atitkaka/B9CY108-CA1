using System;

namespace HospitalManagement
{
    // Doctor inherits from Staff so it gets Name, StaffId and Department
    public class Doctor : Staff
    {
        private int yearsOfExperience;

        public string Specialization { get; set; }
        public string LicenceNumber { get; private set; }

        public int YearsOfExperience
        {
            get { return yearsOfExperience; }
            set
            {
                if (value < 0 || value > 60)
                {
                    throw new ArgumentException("Years of experience must be between 0 and 60");
                }
                yearsOfExperience = value;
            }
        }

        public Doctor(string name, string staffId, string department,
                      string specialization, string licenceNumber, int yearsOfExperience)
            : base(name, staffId, department)
        {
            Specialization = specialization;
            LicenceNumber = licenceNumber;
            YearsOfExperience = yearsOfExperience;
        }

        public override string GetRole()
        {
            return "Doctor";
        }

        // polymorphism - doctor has different duties than nurse and admin
        public override string PerformDuties()
        {
            return "Examines and diagnoses patients, prescribes treatment (" + Specialization + ")";
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo(); // print the common details first
            Console.WriteLine("Specialty  : " + Specialization);
            Console.WriteLine("Licence No : " + LicenceNumber);
            Console.WriteLine("Experience : " + YearsOfExperience + " years");
            PrintDuties();
        }

        // extra method only a doctor has
        public string PrescribeMedicine(string patientName, string medicine)
        {
            return "Dr. " + Name + " prescribed " + medicine + " to " + patientName;
        }
    }
}
