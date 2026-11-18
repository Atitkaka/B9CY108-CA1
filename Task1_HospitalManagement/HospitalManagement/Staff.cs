using System;

namespace HospitalManagement
{
    // Staff is the parent (base) class for all the hospital staff.
    // It is abstract because we never create a plain "Staff" object,
    // we only create Doctor, Nurse or AdminStaff objects.
    public abstract class Staff
    {
        // private fields - can only be changed from inside this class (encapsulation)
        private string name;
        private string staffId;
        private string department;

        // static field - shared by all objects, counts how many staff were created
        private static int totalStaffCreated = 0;

        // constructor of the base class, child classes call this using : base(...)
        protected Staff(string name, string staffId, string department)
        {
            Name = name;
            StaffId = staffId;
            Department = department;
            totalStaffCreated++;
        }

        // Properties with validation so wrong data can't be stored
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be empty");
                }
                name = value.Trim();
            }
        }

        // Staff ID can be read from anywhere but only set inside this class
        public string StaffId
        {
            get { return staffId; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Staff ID cannot be empty");
                }
                staffId = value.Trim().ToUpper();
            }
        }

        public string Department
        {
            get { return department; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Department cannot be empty");
                }
                department = value.Trim();
            }
        }

        public static int TotalStaffCreated
        {
            get { return totalStaffCreated; }
        }

        // abstract methods - every child class MUST write its own version
        public abstract string GetRole();
        public abstract string PerformDuties();

        // virtual method - child classes can override it if they want to
        public virtual void DisplayInfo()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Role       : " + GetRole());
            Console.WriteLine("Staff ID   : " + StaffId);
            Console.WriteLine("Name       : " + Name);
            Console.WriteLine("Department : " + Department);
        }

        // protected method - only this class and the child classes can use it
        protected void PrintDuties()
        {
            Console.WriteLine("Duties     : " + PerformDuties());
        }
    }
}
