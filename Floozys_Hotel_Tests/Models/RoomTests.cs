using Microsoft.VisualStudio.TestTools.UnitTesting;
using Floozys_Hotel.Models;
using System.Linq;

namespace Floozys_Hotel_Tests.Models
{
    /// <summary>
    /// Tests for Room model validation logic
    /// FIRST Principles: Fast, Independent, Repeatable, Self-validating, Timely
    /// </summary>
    [TestClass]
    public class RoomTests
    {
        // ============================================
        // CONSTRUCTOR TESTS
        // ============================================

        [TestMethod]
        public void Constructor_WithValidParameters_CreatesRoom()
        {
            // Arrange & Act
            var room = new Room(
                roomNumber: "101",
                floor: 1,
                roomSize: "Double",
                capacity: 2,
                status: RoomStatus.Available
            );

            // Assert
            Assert.AreEqual("101", room.RoomNumber);
            Assert.AreEqual(1, room.Floor);
            Assert.AreEqual("Double", room.RoomSize);
            Assert.AreEqual(2, room.Capacity);
            Assert.AreEqual(RoomStatus.Available, room.Status);
        }

        [TestMethod]
        public void Constructor_Parameterless_CreatesEmptyRoom()
        {
            // Arrange & Act
            var room = new Room();

            // Assert
            Assert.IsNotNull(room, "Parameterless constructor should create room");
            Assert.AreEqual("", room.RoomNumber, "Default room number should be empty");
            Assert.AreEqual(RoomStatus.Available, room.Status, "Default status should be Available");
        }

        // ============================================
        // VALIDATION TESTS
        // ============================================

        [TestMethod]
        public void Validate_ValidRoom_ReturnsNoErrors()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "101",
                Floor = 1,
                RoomSize = "Double",
                Capacity = 2,
                Status = RoomStatus.Available
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count, "Valid room should have no errors");
        }

        [TestMethod]
        public void Validate_MissingRoomNumber_ReturnsError()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "",
                Floor = 1,
                RoomSize = "Double",
                Capacity = 2
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Room number is required")));
        }

        [TestMethod]
        public void Validate_WhitespaceRoomNumber_ReturnsError()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "   ",
                Floor = 1,
                RoomSize = "Double",
                Capacity = 2
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Room number is required")));
        }

        [TestMethod]
        public void Validate_FloorZero_ReturnsError()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "101",
                Floor = 0,
                RoomSize = "Double",
                Capacity = 2
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Floor must be greater than 0")));
        }

        [TestMethod]
        public void Validate_NegativeFloor_ReturnsError()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "101",
                Floor = -1,
                RoomSize = "Double",
                Capacity = 2
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Floor must be greater than 0")));
        }

        [TestMethod]
        public void Validate_MissingRoomSize_ReturnsError()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "101",
                Floor = 1,
                RoomSize = "",
                Capacity = 2
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Room size is required")));
        }

        [TestMethod]
        public void Validate_CapacityZero_ReturnsError()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "101",
                Floor = 1,
                RoomSize = "Double",
                Capacity = 0
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Capacity must be greater than 0")));
        }

        [TestMethod]
        public void Validate_NegativeCapacity_ReturnsError()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "101",
                Floor = 1,
                RoomSize = "Double",
                Capacity = -1
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.IsTrue(errors.Count > 0, "Should have errors");
            Assert.IsTrue(errors.Any(e => e.Contains("Capacity must be greater than 0")));
        }

        [TestMethod]
        public void Validate_MultipleErrors_ReturnsAllErrors()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "",
                Floor = 0,
                RoomSize = "",
                Capacity = 0
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.IsTrue(errors.Count >= 4, "Should have multiple errors");
        }

        // ============================================
        // ROOM STATUS TESTS
        // ============================================

        [TestMethod]
        public void Status_DefaultValue_IsAvailable()
        {
            // Arrange & Act
            var room = new Room();

            // Assert
            Assert.AreEqual(RoomStatus.Available, room.Status, "Default status should be Available");
        }

        [TestMethod]
        public void Status_CanBeSetToOutOfService()
        {
            // Arrange
            var room = new Room { Status = RoomStatus.OutOfService };

            // Act
            var status = room.Status;

            // Assert
            Assert.AreEqual(RoomStatus.OutOfService, status);
        }

        [TestMethod]
        public void Status_CanBeSetToMaintenance()
        {
            // Arrange
            var room = new Room { Status = RoomStatus.Maintenance };

            // Act
            var status = room.Status;

            // Assert
            Assert.AreEqual(RoomStatus.Maintenance, status);
        }

        // ============================================
        // ROOM SIZE TESTS (Common values)
        // ============================================

        [TestMethod]
        public void RoomSize_AcceptsSingle()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "101",
                Floor = 1,
                RoomSize = "Single",
                Capacity = 1
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void RoomSize_AcceptsDouble()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "102",
                Floor = 1,
                RoomSize = "Double",
                Capacity = 2
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void RoomSize_AcceptsSuite()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "103",
                Floor = 1,
                RoomSize = "Suite",
                Capacity = 4
            };

            // Act
            var errors = room.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count);
        }
    }
}