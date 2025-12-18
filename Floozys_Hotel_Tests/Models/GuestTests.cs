using Microsoft.VisualStudio.TestTools.UnitTesting;
using Floozys_Hotel.Models;
using System.Linq;

namespace Floozys_Hotel_Tests.Models
{
    /// <summary>
    /// Tests for Guest model validation logic
    /// FIRST Principles: Fast, Independent, Repeatable, Self-validating, Timely
    /// </summary>
    [TestClass]
    public class GuestTests
    {
        // ============================================
        // CONSTRUCTOR TESTS
        // ============================================

        [TestMethod]
        public void Constructor_WithValidParameters_CreatesGuest()
        {
            // Arrange & Act
            var guest = new Guest(
                firstName: "John",
                lastName: "Smith",
                email: "john@example.com",
                phoneNumber: "+45 12345678",
                country: "Denmark",
                passportNumber: "DK123456"
            );

            // Assert
            Assert.AreEqual("John", guest.FirstName);
            Assert.AreEqual("Smith", guest.LastName);
            Assert.AreEqual("john@example.com", guest.Email);
            Assert.AreEqual("+45 12345678", guest.PhoneNumber);
            Assert.AreEqual("Denmark", guest.Country);
            Assert.AreEqual("DK123456", guest.PassportNumber);
        }

        [TestMethod]
        public void Constructor_Parameterless_CreatesEmptyGuest()
        {
            // Arrange & Act
            var guest = new Guest();

            // Assert
            Assert.IsNotNull(guest, "Parameterless constructor should create guest");
        }

        // ============================================
        // VALIDATION TESTS
        // ============================================

        [TestMethod]
        public void Validate_ValidGuest_ReturnsNoErrors()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john@example.com",
                PhoneNumber = "+45 12345678",
                Country = "Denmark"
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count, "Valid guest should have no errors");
        }

        [TestMethod]
        public void Validate_MissingFirstName_ReturnsError()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "",
                LastName = "Smith",
                Email = "john@example.com",
                PhoneNumber = "+45 12345678",
                Country = "Denmark"
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("First name is required")));
        }

        [TestMethod]
        public void Validate_WhitespaceFirstName_ReturnsError()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "   ",
                LastName = "Smith",
                Email = "john@example.com",
                PhoneNumber = "+45 12345678",
                Country = "Denmark"
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("First name is required")));
        }

        [TestMethod]
        public void Validate_MissingLastName_ReturnsError()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "John",
                LastName = "",
                Email = "john@example.com",
                PhoneNumber = "+45 12345678",
                Country = "Denmark"
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Last name is required")));
        }

        [TestMethod]
        public void Validate_MissingEmail_ReturnsError()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "",
                PhoneNumber = "+45 12345678",
                Country = "Denmark"
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("valid email is required")));
        }

        [TestMethod]
        public void Validate_EmailWithoutAtSign_ReturnsError()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "johnexample.com",
                PhoneNumber = "+45 12345678",
                Country = "Denmark"
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("valid email is required")));
        }

        [TestMethod]
        public void Validate_EmailWithoutDot_ReturnsError()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john@examplecom",
                PhoneNumber = "+45 12345678",
                Country = "Denmark"
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("valid email is required")));
        }

        [TestMethod]
        public void Validate_MissingPhoneNumber_ReturnsError()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john@example.com",
                PhoneNumber = "",
                Country = "Denmark"
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Phone number is required")));
        }

        [TestMethod]
        public void Validate_MissingCountry_ReturnsError()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john@example.com",
                PhoneNumber = "+45 12345678",
                Country = ""
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Country is required")));
        }

        [TestMethod]
        public void Validate_MultipleErrors_ReturnsAllErrors()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "",
                LastName = "",
                Email = "invalid",
                PhoneNumber = "",
                Country = ""
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.IsTrue(errors.Count >= 5, "Should have multiple errors");
        }

        [TestMethod]
        public void Validate_PassportNumberOptional_NoError()
        {
            // Arrange
            var guest = new Guest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john@example.com",
                PhoneNumber = "+45 12345678",
                Country = "Denmark",
                PassportNumber = null
            };

            // Act
            var errors = guest.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count, "Passport number is optional");
        }
    }
}