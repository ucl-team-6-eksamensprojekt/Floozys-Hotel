USE HotelBooking;
GO

CREATE PROCEDURE uspCreateRoom @RoomNumber NVARCHAR(50), @Floor INT, @RoomSize NVARCHAR(50), @Capacity INT, @Status INT

AS
BEGIN

INSERT INTO ROOM (RoomNumber, Floor, RoomSize, Capacity, Status)
VALUES (@RoomNumber, @Floor, @RoomSize, @Capacity, @Status);
END; 
GO

