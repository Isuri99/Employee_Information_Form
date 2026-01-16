CREATE DATABASE EmployeeDB;
GO

USE EmployeeDB;
GO

CREATE TABLE Employee (
    EmployeeNumber INT PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    DateOfBirth DATE,
    Salary DECIMAL(10,2)
);
GO

CREATE PROCEDURE PopulateData
    @EmployeeNumber INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATE,
    @Salary DECIMAL(10,2)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Employee WHERE EmployeeNumber = @EmployeeNumber)
    BEGIN
        UPDATE Employee
        SET FirstName = @FirstName,
            LastName = @LastName,
            DateOfBirth = @DateOfBirth,
            Salary = @Salary
        WHERE EmployeeNumber = @EmployeeNumber;
    END
    ELSE
    BEGIN
        INSERT INTO Employee (EmployeeNumber, FirstName, LastName, DateOfBirth, Salary)
        VALUES (@EmployeeNumber, @FirstName, @LastName, @DateOfBirth, @Salary);
    END
END
GO

CREATE PROCEDURE SelectEmployees
AS
BEGIN
    SELECT * FROM Employee;
END
GO

CREATE PROCEDURE DeleteEmployee
    @EmployeeNumber INT
AS
BEGIN
    DELETE FROM Employee
    WHERE EmployeeNumber = @EmployeeNumber;
END
GO
