using Microsoft.VisualStudio.TestTools.UnitTesting;
using Floozys_Hotel.Repositories;
using Floozys_Hotel.Models;
using System.Linq;

namespace Floozys_Hotel_Tests.Repositories
{
    /// <summary>
    /// Integration tests for RoomRepo - tests CRUD operations against real database
    /// NOTE: These tests require a test database connection
    /// FIRST Principles: Fast, Independent, Repeatable, Self-validating, Timely
    /// </summary>
    [TestClass]
    public class RoomRepoTests
    {
        private RoomRepo _roomRepo;

        [TestInitialize]
        public void Setup()
        {
            // Arrange - Create repository
            _roomRepo = new RoomRepo();
        }

        // ============================================
        // CREATE TESTS
        // ============================================

        [TestMethod]
        public void CreateRoom_ValidRoom_InsertsIntoDatabase()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "TEST001",
                Floor = 8,
                RoomSize = "Test Single",
                Capacity = 1,
                Status = RoomStatus.Available
            };

            // Act
            _roomRepo.CreateRoom(room);

            // Assert
            var allRooms = _roomRepo.GetAll();
            var created = allRooms.FirstOrDefault(r => r.RoomNumber == "TEST001");
            Assert.IsNotNull(created, "Room should be created");
            Assert.AreEqual(8, created.Floor);
            Assert.AreEqual("Test Single", created.RoomSize);
            Assert.AreEqual(1, created.Capacity);

            // Cleanup
            _roomRepo.DeleteRoom(created.RoomId);
        }

        [TestMethod]
        public void CreateRoom_InvalidRoom_ThrowsException()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "",
                Floor = 0,
                RoomSize = "",
                Capacity = 0
            };
            bool exceptionThrown = false;

            // Act
            try
            {
                _roomRepo.CreateRoom(room);
            }
            catch (System.ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Should throw ArgumentException for invalid room");
        }

        // ============================================
        // READ TESTS
        // ============================================

        [TestMethod]
        public void GetAll_ReturnsAllRooms()
        {
            // Arrange
            var room1 = new Room
            {
                RoomNumber = "TEST101",
                Floor = 1,
                RoomSize = "Single",
                Capacity = 1,
                Status = RoomStatus.Available
            };
            var room2 = new Room
            {
                RoomNumber = "TEST102",
                Floor = 1,
                RoomSize = "Double",
                Capacity = 2,
                Status = RoomStatus.Available
            };

            _roomRepo.CreateRoom(room1);
            _roomRepo.CreateRoom(room2);

            // Act
            var allRooms = _roomRepo.GetAll();

            // Assert
            Assert.IsTrue(allRooms.Count >= 2, "Should retrieve at least test rooms");
            Assert.IsTrue(allRooms.Any(r => r.RoomNumber == "TEST101"), "Should find TEST101");
            Assert.IsTrue(allRooms.Any(r => r.RoomNumber == "TEST102"), "Should find TEST102");

            // Cleanup
            var testRoom1 = allRooms.First(r => r.RoomNumber == "TEST101");
            var testRoom2 = allRooms.First(r => r.RoomNumber == "TEST102");
            _roomRepo.DeleteRoom(testRoom1.RoomId);
            _roomRepo.DeleteRoom(testRoom2.RoomId);
        }

        [TestMethod]
        public void GetById_ExistingRoom_ReturnsRoom()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "TESTFIND",
                Floor = 2,
                RoomSize = "Double",
                Capacity = 2,
                Status = RoomStatus.Available
            };
            _roomRepo.CreateRoom(room);
            var allRooms = _roomRepo.GetAll();
            var created = allRooms.First(r => r.RoomNumber == "TESTFIND");

            // Act
            var retrieved = _roomRepo.GetById(created.RoomId);

            // Assert
            Assert.IsNotNull(retrieved, "Should retrieve room");
            Assert.AreEqual("TESTFIND", retrieved.RoomNumber);
            Assert.AreEqual(2, retrieved.Floor);
            Assert.AreEqual("Double", retrieved.RoomSize);

            // Cleanup
            _roomRepo.DeleteRoom(created.RoomId);
        }

        [TestMethod]
        public void GetById_NonExistentRoom_ReturnsNull()
        {
            // Arrange
            int nonExistentId = 999999;

            // Act
            var retrieved = _roomRepo.GetById(nonExistentId);

            // Assert
            Assert.IsNull(retrieved, "Should return null for non-existent room");
        }

        [TestMethod]
        public void GetAllByAvailability_OnlyReturnsAvailableRooms()
        {
            // Arrange
            var availableRoom = new Room
            {
                RoomNumber = "TESTAVAIL",
                Floor = 3,
                RoomSize = "Single",
                Capacity = 1,
                Status = RoomStatus.Available
            };
            var maintenanceRoom = new Room
            {
                RoomNumber = "TESTMAINT",
                Floor = 3,
                RoomSize = "Single",
                Capacity = 1,
                Status = RoomStatus.Maintenance
            };

            _roomRepo.CreateRoom(availableRoom);
            _roomRepo.CreateRoom(maintenanceRoom);

            // Act
            var availableRooms = _roomRepo.GetAllByAvailability();

            // Assert
            Assert.IsTrue(availableRooms.Any(r => r.RoomNumber == "TESTAVAIL"), "Should find available room");
            Assert.IsFalse(availableRooms.Any(r => r.RoomNumber == "TESTMAINT"), "Should not find maintenance room");

            // Cleanup
            var allRooms = _roomRepo.GetAll();
            var room1 = allRooms.First(r => r.RoomNumber == "TESTAVAIL");
            var room2 = allRooms.First(r => r.RoomNumber == "TESTMAINT");
            _roomRepo.DeleteRoom(room1.RoomId);
            _roomRepo.DeleteRoom(room2.RoomId);
        }

        // ============================================
        // FILTER TESTS (GetRoomsFromCriteria)
        // ============================================

        [TestMethod]
        public void GetRoomsFromCriteria_FilterByFloor_ReturnsMatchingRooms()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "TESTFLOOR5",
                Floor = 5,
                RoomSize = "Single",
                Capacity = 1,
                Status = RoomStatus.Available
            };
            _roomRepo.CreateRoom(room);

            // Act
            var results = _roomRepo.GetRoomsFromCriteria(floor: 5, roomSize: null, status: null);

            // Assert
            Assert.IsTrue(results.Any(r => r.RoomNumber == "TESTFLOOR5"), "Should find room on floor 5");
            Assert.IsTrue(results.All(r => r.Floor == 5), "All results should be from floor 5");

            // Cleanup
            var allRooms = _roomRepo.GetAll();
            var created = allRooms.First(r => r.RoomNumber == "TESTFLOOR5");
            _roomRepo.DeleteRoom(created.RoomId);
        }

        [TestMethod]
        public void GetRoomsFromCriteria_FilterByRoomSize_ReturnsMatchingRooms()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "TESTSUITE",
                Floor = 4,
                RoomSize = "Suite",
                Capacity = 4,
                Status = RoomStatus.Available
            };
            _roomRepo.CreateRoom(room);

            // Act
            var results = _roomRepo.GetRoomsFromCriteria(floor: null, roomSize: "Suite", status: null);

            // Assert
            Assert.IsTrue(results.Any(r => r.RoomNumber == "TESTSUITE"), "Should find Suite room");
            Assert.IsTrue(results.All(r => r.RoomSize == "Suite"), "All results should be Suites");

            // Cleanup
            var allRooms = _roomRepo.GetAll();
            var created = allRooms.First(r => r.RoomNumber == "TESTSUITE");
            _roomRepo.DeleteRoom(created.RoomId);
        }

        [TestMethod]
        public void GetRoomsFromCriteria_FilterByStatus_ReturnsMatchingRooms()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "TESTOOS",
                Floor = 6,
                RoomSize = "Double",
                Capacity = 2,
                Status = RoomStatus.OutOfService
            };
            _roomRepo.CreateRoom(room);

            // Act
            var results = _roomRepo.GetRoomsFromCriteria(floor: null, roomSize: null, status: RoomStatus.OutOfService);

            // Assert
            Assert.IsTrue(results.Any(r => r.RoomNumber == "TESTOOS"), "Should find OutOfService room");
            Assert.IsTrue(results.All(r => r.Status == RoomStatus.OutOfService), "All results should be OutOfService");

            // Cleanup
            var allRooms = _roomRepo.GetAll();
            var created = allRooms.First(r => r.RoomNumber == "TESTOOS");
            _roomRepo.DeleteRoom(created.RoomId);
        }

        [TestMethod]
        public void GetRoomsFromCriteria_MultipleCriteria_ReturnsMatchingRooms()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "TESTMULTI",
                Floor = 7,
                RoomSize = "Double",
                Capacity = 2,
                Status = RoomStatus.Available
            };
            _roomRepo.CreateRoom(room);

            // Act
            var results = _roomRepo.GetRoomsFromCriteria(
                floor: 7,
                roomSize: "Double",
                status: RoomStatus.Available
            );

            // Assert
            Assert.IsTrue(results.Any(r => r.RoomNumber == "TESTMULTI"), "Should find room matching all criteria");

            // Cleanup
            var allRooms = _roomRepo.GetAll();
            var created = allRooms.First(r => r.RoomNumber == "TESTMULTI");
            _roomRepo.DeleteRoom(created.RoomId);
        }

        [TestMethod]
        public void GetRoomsFromCriteria_NoCriteria_ReturnsAllRooms()
        {
            // Arrange & Act
            var results = _roomRepo.GetRoomsFromCriteria(floor: null, roomSize: null, status: null);

            // Assert
            Assert.IsTrue(results.Count > 0, "Should return all rooms when no criteria specified");
        }

        // ============================================
        // UPDATE TESTS
        // ============================================

   

        [TestMethod]
        public void UpdateRoom_InvalidRoom_ThrowsException()
        {
            // Arrange
            var room = new Room
            {
                RoomId = 999999,
                RoomNumber = "",
                Floor = 0,
                RoomSize = "",
                Capacity = 0
            };
            bool exceptionThrown = false;

            // Act
            try
            {
                _roomRepo.UpdateRoom(room);
            }
            catch (System.ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Should throw ArgumentException for invalid room");
        }

        // ============================================
        // DELETE TESTS
        // ============================================

        [TestMethod]
        public void DeleteRoom_ExistingRoom_RemovesFromDatabase()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "TESTDELETE",
                Floor = 9,
                RoomSize = "Single",
                Capacity = 1,
                Status = RoomStatus.Available
            };
            _roomRepo.CreateRoom(room);
            var allRooms = _roomRepo.GetAll();
            var created = allRooms.First(r => r.RoomNumber == "TESTDELETE");
            int roomId = created.RoomId;

            // Act
            _roomRepo.DeleteRoom(roomId);

            // Assert
            var retrieved = _roomRepo.GetById(roomId);
            Assert.IsNull(retrieved, "Deleted room should not exist");
        }

        // ============================================
        // ROOM STATUS TESTS
        // ============================================

        [TestMethod]
        public void CreateRoom_WithDifferentStatuses_StoresCorrectly()
        {
            // Arrange & Act
            var availableRoom = new Room
            {
                RoomNumber = "STATUSAVAIL",
                Floor = 1,
                RoomSize = "Single",
                Capacity = 1,
                Status = RoomStatus.Available
            };
            var maintenanceRoom = new Room
            {
                RoomNumber = "STATUSMAINT",
                Floor = 1,
                RoomSize = "Single",
                Capacity = 1,
                Status = RoomStatus.Maintenance
            };
            var oosRoom = new Room
            {
                RoomNumber = "STATUSOOS",
                Floor = 1,
                RoomSize = "Single",
                Capacity = 1,
                Status = RoomStatus.OutOfService
            };

            _roomRepo.CreateRoom(availableRoom);
            _roomRepo.CreateRoom(maintenanceRoom);
            _roomRepo.CreateRoom(oosRoom);

            // Assert
            var allRooms = _roomRepo.GetAll();
            var room1 = allRooms.First(r => r.RoomNumber == "STATUSAVAIL");
            var room2 = allRooms.First(r => r.RoomNumber == "STATUSMAINT");
            var room3 = allRooms.First(r => r.RoomNumber == "STATUSOOS");

            Assert.AreEqual(RoomStatus.Available, room1.Status);
            Assert.AreEqual(RoomStatus.Maintenance, room2.Status);
            Assert.AreEqual(RoomStatus.OutOfService, room3.Status);

            // Cleanup
            _roomRepo.DeleteRoom(room1.RoomId);
            _roomRepo.DeleteRoom(room2.RoomId);
            _roomRepo.DeleteRoom(room3.RoomId);
        }

        // ============================================
        // DATA INTEGRITY TESTS
        // ============================================

        [TestMethod]
        public void CreateAndRetrieve_AllFields_PreservesData()
        {
            // Arrange
            var room = new Room
            {
                RoomNumber = "TESTFULL",
                Floor = 15,
                RoomSize = "Presidential Suite",
                Capacity = 6,
                Status = RoomStatus.Available
            };

            // Act
            _roomRepo.CreateRoom(room);
            var allRooms = _roomRepo.GetAll();
            var retrieved = allRooms.First(r => r.RoomNumber == "TESTFULL");

            // Assert
            Assert.AreEqual("TESTFULL", retrieved.RoomNumber);
            Assert.AreEqual(15, retrieved.Floor);
            Assert.AreEqual("Presidential Suite", retrieved.RoomSize);
            Assert.AreEqual(6, retrieved.Capacity);
            Assert.AreEqual(RoomStatus.Available, retrieved.Status);

            // Cleanup
            _roomRepo.DeleteRoom(retrieved.RoomId);
        }
    }
}