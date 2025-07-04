-- Create database
CREATE DATABASE ParkingManagementDB;
USE ParkingManagementDB;

-- Users table for login/signup
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Parking Places table
CREATE TABLE ParkingPlaces (
    PlaceID INT PRIMARY KEY,
    PlaceNumber NVARCHAR(10) NOT NULL, -- P001, P002, etc.
    IsOccupied BIT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Vehicles table for parked vehicles
CREATE TABLE Vehicles (
    VehicleID INT IDENTITY(1,1) PRIMARY KEY,
    TicketID NVARCHAR(20) UNIQUE NOT NULL,
    OwnerName NVARCHAR(100) NOT NULL,
    VehicleNumber NVARCHAR(20) NOT NULL,
    VehicleType NVARCHAR(20) NOT NULL, -- 'Motor' or 'Vehicle'
    ParkingPlaceID INT NOT NULL,
    EntryTime DATETIME NOT NULL,
    ExitTime DATETIME NULL,
    AmountPerMinute DECIMAL(10,2) NOT NULL, -- 20 for Motor, 30 for Vehicle
    TotalAmount DECIMAL(10,2) NULL,
    IsPaid BIT DEFAULT 0,
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (ParkingPlaceID) REFERENCES ParkingPlaces(PlaceID)
);

-- Insert default admin user
INSERT INTO Users (Username, Password, Email) 
VALUES ('admin', 'admin123', 'admin@safeparking.com');

-- Insert 50 parking places
DECLARE @i INT = 1;
WHILE @i <= 50
BEGIN
    INSERT INTO ParkingPlaces (PlaceID, PlaceNumber) 
    VALUES (@i, 'P' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3));
    SET @i = @i + 1;
END

-- Trigger to update parking place status when vehicle is added
CREATE TRIGGER trg_VehicleAdded ON Vehicles
AFTER INSERT
AS
BEGIN
    UPDATE ParkingPlaces 
    SET IsOccupied = 1 
    WHERE PlaceID IN (SELECT ParkingPlaceID FROM inserted WHERE IsActive = 1);
END

-- Trigger to free parking place when vehicle exits
CREATE TRIGGER trg_VehicleExited ON Vehicles
AFTER UPDATE
AS
BEGIN
    IF UPDATE(IsActive)
    BEGIN
        UPDATE ParkingPlaces 
        SET IsOccupied = 0 
        WHERE PlaceID IN (
            SELECT ParkingPlaceID FROM inserted 
            WHERE IsActive = 0 AND ParkingPlaceID IN (
                SELECT ParkingPlaceID FROM deleted WHERE IsActive = 1
            )
        );
    END
END
