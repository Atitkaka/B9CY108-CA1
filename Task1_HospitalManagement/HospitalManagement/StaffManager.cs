using System;
using System.Collections.Generic;

namespace HospitalManagement
{
    // This class stores all the staff and has the methods to add / search / display them.
    // I used a Dictionary with the Staff ID as the key because:
    //  - every staff ID must be unique, so the dictionary stops duplicates
    //  - finding a staff member by ID is very fast (no need to loop through everything)
    public class StaffManager
    {
        private Dictionary<string, Staff> staffList = new Dictionary<string, Staff>();

        public int Count
        {
            get { return staffList.Count; }
        }

        // returns true if added, false if the ID is already used
        public bool AddStaff(Staff staff)
        {
            if (staff == null)
            {
                return false;
            }
            if (staffList.ContainsKey(staff.StaffId))
            {
                return false;
            }
            staffList.Add(staff.StaffId, staff);
            return true;
        }

        public bool IdExists(string staffId)
        {
            if (string.IsNullOrWhiteSpace(staffId))
            {
                return false;
            }
            return staffList.ContainsKey(staffId.Trim().ToUpper());
        }

        // SearchStaff - returns true if a staff member matches the search, otherwise false.
        // The user can search by Staff ID (exact) or by name (part of the name is ok).
        public bool SearchStaff(string searchText)
        {
            List<Staff> results = FindMatches(searchText);
            if (results.Count > 0)
            {
                return true;
            }
            return false;
        }

        // gives back the list of staff that match the search (used to print the results)
        public List<Staff> FindMatches(string searchText)
        {
            List<Staff> results = new List<Staff>();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return results; // empty search -> nothing found
            }

            string text = searchText.Trim().ToUpper();

            // 1. check the ID first (fast lookup in dictionary)
            if (staffList.ContainsKey(text))
            {
                results.Add(staffList[text]);
                return results;
            }

            // 2. if it is not an ID then search by name
            foreach (Staff s in staffList.Values)
            {
                if (s.Name.ToUpper().Contains(text))
                {
                    results.Add(s);
                }
            }
            return results;
        }

        public void DisplayAllStaff()
        {
            if (staffList.Count == 0)
            {
                Console.WriteLine("No staff added yet.");
                return;
            }

            // polymorphism: s is type Staff but the correct DisplayInfo()
            // of Doctor / Nurse / AdminStaff is called
            foreach (Staff s in staffList.Values)
            {
                s.DisplayInfo();
            }
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Total staff: " + staffList.Count);
        }
    }
}
