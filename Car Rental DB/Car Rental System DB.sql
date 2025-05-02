CREATE DATABASE CarRentalDB;
USE CarRentalDB;

-- Users Table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) CHECK (Role IN ('Admin', 'Customer')) NOT NULL
);

-- Cars Table
CREATE TABLE Cars (
    CarID INT IDENTITY(1,1) PRIMARY KEY,
	Brand NVARCHAR(100) NOT NULL,
	Model NVARCHAR(100) NOT NULL,
	Year INT NOT NULL,
	PricePerDay DECIMAL(10,2) NOT NULL,
	IsAvailable BIT DEFAULT 1
);

-- Rentals Table
CREATE TABLE Rentals (
    RentalID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    CarID INT FOREIGN KEY REFERENCES Cars(CarID),
    RentalDate DATE NOT NULL,
    ReturnDate DATE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
	FOREIGN KEY (CarID) REFERENCES Cars(CarID)
);

-- Payments Table
CREATE TABLE Payments (
    PaymentID INT IDENTITY(1,1) PRIMARY KEY,
    RentalID INT FOREIGN KEY REFERENCES Rentals(RentalID),
    Amount DECIMAL(10,2) NOT NULL,
    DatePaid DATE NOT NULL
);

SELECT*
FROM Users

SELECT*
FROM Cars

SELECT*
FROM Rentals 

SELECT*
FROM Payments


DROP TABLE Payments

DROP TABLE Rentals

DROP TABLE Cars

DROP TABLE Users



