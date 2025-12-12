USE HotelBooking;
GO


CREATE PROCEDURE uspGetAllRooms
AS 
BEGIN


SELECT RoomID, RoomNumber, Floor, RoomSize, Capacity, Status FROM ROOM;
END;
GO

CREATE PROCEDURE uspGetRoomById @RoomID INT
AS
BEGIN


SELECT RoomID, RoomNumber, Floor, RoomSize, Capacity, Status FROM ROOM Where RoomID = @RoomID; 
END;
GO

CREATE PROCEDURE uspGetRoomsFromCriteria @Floor INT = NULL, @RoomSize NVARCHAR(20) = NULL, @Status INT = NULL
AS
BEGIN


SELECT RoomID, RoomNumber, Floor, RoomSize, Capacity, Status FROM ROOM
WHERE (@Floor IS NULL OR Floor = @Floor)
AND (@RoomSize IS NULL OR RoomSize = @RoomSize)
AND (@Status IS NULL OR Status = @Status);
END; 
GO

CREATE PROCEDURE uspUpdateRoom @RoomID INT, @RoomNumber INT, @Floor INT, @RoomSize NVARCHAR(20), @Capacity INT, @Status INT
AS
BEGIN
 

UPDATE ROOM
SET 
RoomNumber = @RoomNumber,
Floor = @Floor,
RoomSize = @RoomSize,
Capacity = @Capacity,
Status = @Status
WHERE RoomID = @RoomID;
END
GO

CREATE PROCEDURE uspDeleteRoom @RoomID INT
AS
BEGIN


DELETE FROM ROOM
WHERE RoomID = @RoomID;
END;
GO
