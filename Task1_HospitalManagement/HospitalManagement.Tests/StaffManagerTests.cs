using Microsoft.VisualStudio.TestTools.UnitTesting;
using HospitalManagement;

namespace HospitalManagement.Tests
{
    // Unit tests for the SearchStaff() method
    [TestClass]
    public class StaffManagerTests
    {
        private StaffManager manager;

        // runs before every test, so each test starts with the same data
        [TestInitialize]
        public void Setup()
        {
            manager = new StaffManager();
            manager.AddStaff(new Doctor("John Murphy", "D101", "Cardiology", "Cardiologist", "IMC-45821", 12));
            manager.AddStaff(new Nurse("Mary Byrne", "N201", "Emergency", "A&E", "Night"));
            manager.AddStaff(new AdminStaff("Sean Kelly", "A301", "Administration", "Receptionist", "G-12"));
        }

        [TestMethod]
        public void SearchStaff_ExistingId_ReturnsTrue()
        {
            bool result = manager.SearchStaff("D101");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SearchStaff_IdInLowerCase_ReturnsTrue()
        {
            bool result = manager.SearchStaff("n201");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SearchStaff_ExistingName_ReturnsTrue()
        {
            bool result = manager.SearchStaff("Sean Kelly");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SearchStaff_PartOfName_ReturnsTrue()
        {
            bool result = manager.SearchStaff("murphy");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SearchStaff_IdNotInSystem_ReturnsFalse()
        {
            bool result = manager.SearchStaff("X999");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SearchStaff_NameNotInSystem_ReturnsFalse()
        {
            bool result = manager.SearchStaff("Patrick");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SearchStaff_EmptyText_ReturnsFalse()
        {
            Assert.IsFalse(manager.SearchStaff(""));
            Assert.IsFalse(manager.SearchStaff("   "));
            Assert.IsFalse(manager.SearchStaff(null));
        }

        [TestMethod]
        public void SearchStaff_EmptyList_ReturnsFalse()
        {
            StaffManager emptyManager = new StaffManager();
            Assert.IsFalse(emptyManager.SearchStaff("D101"));
        }

        [TestMethod]
        public void SearchStaff_AfterAddingNewStaff_ReturnsTrue()
        {
            Assert.IsFalse(manager.SearchStaff("N202"));

            manager.AddStaff(new Nurse("Aoife Walsh", "N202", "Paediatrics", "Children", "Day"));

            Assert.IsTrue(manager.SearchStaff("N202"));
        }

        // extra test - the dictionary should not allow two staff with the same ID
        [TestMethod]
        public void AddStaff_DuplicateId_ReturnsFalse()
        {
            bool added = manager.AddStaff(new Doctor("Another Doctor", "D101", "Surgery", "Surgeon", "IMC-11111", 5));
            Assert.IsFalse(added);
            Assert.AreEqual(3, manager.Count);
        }
    }
}
