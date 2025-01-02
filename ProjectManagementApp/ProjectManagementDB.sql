create database ProjectManagementDB;
use ProjectManagementDB;

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    Role NVARCHAR(20) NOT NULL
);

CREATE TABLE Projects (
    ProjectId INT PRIMARY KEY IDENTITY,
    ProjectName NVARCHAR(100) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL
);

CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY,
    TaskName NVARCHAR(100) NOT NULL,
    ProjectId INT FOREIGN KEY REFERENCES Projects(ProjectId),
    AssignedTo NVARCHAR(100),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL
);

CREATE TABLE Employees (
    EmployeeId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Position NVARCHAR(50),
    CompletedTasks INT DEFAULT 0
);
INSERT INTO Users (Username, Password, Role) VALUES ('admin', 'admin123', 'Admin');
INSERT INTO Projects (ProjectName, StartDate, EndDate) VALUES ('Project A', '2023-01-01', '2023-06-01');
INSERT INTO Employees (Name, Position) VALUES ('John Doe', 'Developer');