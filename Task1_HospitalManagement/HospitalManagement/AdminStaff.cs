using System;

namespace HospitalManagement
{
    public class AdminStaff : Staff
    {
        public string JobTitle { get; set; }   // e.g. Receptionist, Accounts, HR
        public string OfficeNumber { get; set; }

        public AdminStaff(string name, string staffId, string department, string jobTitle, string officeNumber)
            : base(name, staffId, department)
        {
            JobTitle = jobTitle;
            OfficeNumber = officeNumber;
        }

        public override string GetRole()
        {
            return "Administrative Staff";
        }

        public override string PerformDuties()
        {
            return "Works as " + JobTitle + " - manages records, appointments and billing";
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("Job Title  : " + JobTitle);
            Console.WriteLine("Office No  : " + OfficeNumber);
            PrintDuties();
        }

        public string ScheduleAppointment(string patientName, string date)
        {
            return JobTitle + " " + Name + " booked an appointment for " + patientName + " on " + date;
        }
    }
}
