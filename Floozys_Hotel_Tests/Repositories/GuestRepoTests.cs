using Microsoft.VisualStudio.TestTools.UnitTesting;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Models;
using System.Linq;

namespace Floozys_Hotel_Tests.Repositories
{
    /// <summary>
    /// Integration tests for GuestRepo - tests CRUD operations against real database
    /// NOTE: These tests require a test database connection
    /// FIRST Principles: Fast, Independent, Repeatable, Self-validating, Timely
    /// </summary>
    [TestClass]
    public class GuestRepoTests
    {
        private GuestRepo _guestRepo;

        [TestInitialize]
        public void Setup()
        {
            // Arrange - Create repository
            _guestRepo = new GuestRepo();
        }

        // ============================================
        // CREATE TESTS
        // ============================================

        [TestMethod]
        public void AddGuest_ValidGuest_InsertsIntoDatabase()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test.user@example.com",
                PhoneNumber = "+45 12345678",
                Country = "Denmark",
                PassportNumber = "TEST001"
            };

            // Act
            int guestId = _guestRepo.AddGuest(guest);

            // Assert
            Assert.IsTrue(guestId > 0, "GuestID should be assigned after insert");

            // Verify in database
            var retrieved = _guestRepo.GetByID(guestId);
            Assert.IsNotNull(retrieved, "Should retrieve created guest");
            Assert.AreEqual("Test", retrieved.FirstName);
            Assert.AreEqual("User", retrieved.LastName);
            Assert.AreEqual("test.user@example.com", retrieved.Email);

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }

        [TestMethod]
        public void AddGuest_WithoutPassportNumber_InsertsSuccessfully()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "Test",
                LastName = "NoPassport",
                Email = "nopassport@example.com",
                PhoneNumber = "+45 87654321",
                Country = "Denmark",
                PassportNumber = null
            };

            // Act
            int guestId = _guestRepo.AddGuest(guest);

            // Assert
            Assert.IsTrue(guestId > 0, "GuestID should be assigned");

            var retrieved = _guestRepo.GetByID(guestId);
            Assert.IsNull(retrieved.PassportNumber, "Passport number should be null");

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }

        // ============================================
        // READ TESTS
        // ============================================

        [TestMethod]
        public void GetAll_ReturnsAllGuests()
        {
            // Arrange
            var guest1 = new Guest
            {
                FirstName = "Guest",
                LastName = "One",
                Email = "guest1@example.com",
                PhoneNumber = "+45 11111111",
                Country = "Denmark"
            };
            var guest2 = new Guest
            {
                FirstName = "Guest",
                LastName = "Two",
                Email = "guest2@example.com",
                PhoneNumber = "+45 22222222",
                Country = "Sweden"
            };

            int id1 = _guestRepo.AddGuest(guest1);
            int id2 = _guestRepo.AddGuest(guest2);

            // Act
            var allGuests = _guestRepo.GetAll();

            // Assert
            Assert.IsTrue(allGuests.Count >= 2, "Should retrieve at least the test guests");
            Assert.IsTrue(allGuests.Any(g => g.GuestID == id1), "Should find guest 1");
            Assert.IsTrue(allGuests.Any(g => g.GuestID == id2), "Should find guest 2");

            // Cleanup
            _guestRepo.DeleteGuest(id1);
            _guestRepo.DeleteGuest(id2);
        }

        [TestMethod]
        public void GetByID_ExistingGuest_ReturnsGuest()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "FindMe",
                LastName = "Test",
                Email = "findme@example.com",
                PhoneNumber = "+45 99999999",
                Country = "Denmark"
            };
            int guestId = _guestRepo.AddGuest(guest);

            // Act
            var retrieved = _guestRepo.GetByID(guestId);

            // Assert
            Assert.IsNotNull(retrieved, "Should retrieve guest");
            Assert.AreEqual(guestId, retrieved.GuestID);
            Assert.AreEqual("FindMe", retrieved.FirstName);
            Assert.AreEqual("Test", retrieved.LastName);

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }

        [TestMethod]
        public void GetByID_NonExistentGuest_ReturnsNull()
        {
            // Arrange
            int nonExistentId = 999999;

            // Act
            var retrieved = _guestRepo.GetByID(nonExistentId);

            // Assert
            Assert.IsNull(retrieved, "Should return null for non-existent guest");
        }

        [TestMethod]
        public void GetAllByName_MatchingFirstName_ReturnsGuests()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "UniqueNameTest",
                LastName = "Smith",
                Email = "unique@example.com",
                PhoneNumber = "+45 33333333",
                Country = "Denmark"
            };
            int guestId = _guestRepo.AddGuest(guest);

            // Act
            var results = _guestRepo.GetAllByName("UniqueNameTest");

            // Assert
            Assert.IsTrue(results.Count > 0, "Should find guests with matching name");
            Assert.IsTrue(results.Any(g => g.GuestID == guestId), "Should find our test guest");

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }

        [TestMethod]
        public void GetAllByName_MatchingLastName_ReturnsGuests()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "John",
                LastName = "UniqueLastNameTest",
                Email = "lastname@example.com",
                PhoneNumber = "+45 44444444",
                Country = "Denmark"
            };
            int guestId = _guestRepo.AddGuest(guest);

            // Act
            var results = _guestRepo.GetAllByName("UniqueLastNameTest");

            // Assert
            Assert.IsTrue(results.Count > 0, "Should find guests with matching last name");
            Assert.IsTrue(results.Any(g => g.GuestID == guestId), "Should find our test guest");

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }

        [TestMethod]
        public void GetAllByName_PartialMatch_ReturnsGuests()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "XYZTestABC",
                LastName = "Smith",
                Email = "partial@example.com",
                PhoneNumber = "+45 55555555",
                Country = "Denmark"
            };
            int guestId = _guestRepo.AddGuest(guest);

            // Act
            var results = _guestRepo.GetAllByName("Test");

            // Assert
            Assert.IsTrue(results.Any(g => g.GuestID == guestId), "Should find guest with partial name match");

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }

        // ============================================
        // UPDATE TESTS
        // ============================================

        [TestMethod]
        public void UpdateGuest_ExistingGuest_UpdatesDatabase()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "Original",
                LastName = "Name",
                Email = "original@example.com",
                PhoneNumber = "+45 66666666",
                Country = "Denmark",
                PassportNumber = "OLD123"
            };
            int guestId = _guestRepo.AddGuest(guest);

            // Act
            var retrieved = _guestRepo.GetByID(guestId);
            retrieved.FirstName = "Updated";
            retrieved.LastName = "NewName";
            retrieved.Email = "updated@example.com";
            retrieved.PhoneNumber = "+45 77777777";
            retrieved.Country = "Sweden";
            retrieved.PassportNumber = "NEW456";

            _guestRepo.UpdateGuest(retrieved);

            // Assert
            var updated = _guestRepo.GetByID(guestId);
            Assert.AreEqual("Updated", updated.FirstName);
            Assert.AreEqual("NewName", updated.LastName);
            Assert.AreEqual("updated@example.com", updated.Email);
            Assert.AreEqual("+45 77777777", updated.PhoneNumber);
            Assert.AreEqual("Sweden", updated.Country);
            Assert.AreEqual("NEW456", updated.PassportNumber);

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }

        [TestMethod]
        public void UpdateGuest_RemovePassportNumber_UpdatesToNull()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "Test",
                LastName = "PassportRemoval",
                Email = "passport@example.com",
                PhoneNumber = "+45 88888888",
                Country = "Denmark",
                PassportNumber = "REMOVE123"
            };
            int guestId = _guestRepo.AddGuest(guest);

            // Act
            var retrieved = _guestRepo.GetByID(guestId);
            retrieved.PassportNumber = null;
            _guestRepo.UpdateGuest(retrieved);

            // Assert
            var updated = _guestRepo.GetByID(guestId);
            Assert.IsNull(updated.PassportNumber, "Passport number should be null after update");

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }

        // ============================================
        // DELETE TESTS
        // ============================================

        [TestMethod]
        public void DeleteGuest_ExistingGuest_RemovesFromDatabase()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "ToDelete",
                LastName = "Test",
                Email = "delete@example.com",
                PhoneNumber = "+45 00000000",
                Country = "Denmark"
            };
            int guestId = _guestRepo.AddGuest(guest);

            // Act
            _guestRepo.DeleteGuest(guestId);

            // Assert
            var retrieved = _guestRepo.GetByID(guestId);
            Assert.IsNull(retrieved, "Deleted guest should not exist");
        }

        // ============================================
        // DATA INTEGRITY TESTS
        // ============================================

        [TestMethod]
        public void AddGuest_AllFields_PreservesData()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "FullData",
                LastName = "TestUser",
                Email = "fulldata@example.com",
                PhoneNumber = "+45 12121212",
                Country = "Norway",
                PassportNumber = "FULL789"
            };

            // Act
            int guestId = _guestRepo.AddGuest(guest);
            var retrieved = _guestRepo.GetByID(guestId);

            // Assert
            Assert.AreEqual("FullData", retrieved.FirstName);
            Assert.AreEqual("TestUser", retrieved.LastName);
            Assert.AreEqual("fulldata@example.com", retrieved.Email);
            Assert.AreEqual("+45 12121212", retrieved.PhoneNumber);
            Assert.AreEqual("Norway", retrieved.Country);
            Assert.AreEqual("FULL789", retrieved.PassportNumber);

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }

        [TestMethod]
        public void GetAll_ReturnsGuestsWithAllProperties()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "Properties",
                LastName = "Check",
                Email = "props@example.com",
                PhoneNumber = "+45 13131313",
                Country = "Finland"
            };
            int guestId = _guestRepo.AddGuest(guest);

            // Act
            var allGuests = _guestRepo.GetAll();
            var found = allGuests.FirstOrDefault(g => g.GuestID == guestId);

            // Assert
            Assert.IsNotNull(found, "Should find guest in GetAll");
            Assert.IsFalse(string.IsNullOrEmpty(found.FirstName), "FirstName should be populated");
            Assert.IsFalse(string.IsNullOrEmpty(found.LastName), "LastName should be populated");
            Assert.IsFalse(string.IsNullOrEmpty(found.Email), "Email should be populated");
            Assert.IsFalse(string.IsNullOrEmpty(found.PhoneNumber), "PhoneNumber should be populated");
            Assert.IsFalse(string.IsNullOrEmpty(found.Country), "Country should be populated");

            // Cleanup
            _guestRepo.DeleteGuest(guestId);
        }
    }
}