-- Task Manager Database Setup Script
-- Run this script to create the TaskManagerDB database and tables

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TaskManagerDB')
BEGIN
    CREATE DATABASE TaskManagerDB;
END
GO

USE TaskManagerDB;
GO

-- Create Users Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        CreatedDate DATETIME2 DEFAULT GETDATE()
    );
END
GO

-- Create Tasks Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tasks')
BEGIN
    CREATE TABLE Tasks (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000),
        Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
        UserId INT NOT NULL,
        CreatedDate DATETIME2 DEFAULT GETDATE(),
        DueDate DATETIME2,
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );
END
GO

-- Create Indexes for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Tasks_UserId')
BEGIN
    CREATE INDEX IX_Tasks_UserId ON Tasks(UserId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Tasks_Status')
BEGIN
    CREATE INDEX IX_Tasks_Status ON Tasks(Status);
END
GO

-- Insert Sample Data
IF NOT EXISTS (SELECT * FROM Users)
BEGIN
    INSERT INTO Users (Name, Email) VALUES 
    ('John Doe', 'john.doe@example.com'),
    ('Jane Smith', 'jane.smith@example.com'),
    ('Mike Johnson', 'mike.johnson@example.com');
END
GO

IF NOT EXISTS (SELECT * FROM Tasks)
BEGIN
    INSERT INTO Tasks (Title, Description, Status, UserId, DueDate) VALUES 
    ('Setup Development Environment', 'Install all necessary tools and dependencies', 'Completed', 1, DATEADD(day, -2, GETDATE())),
    ('Design Database Schema', 'Create tables and relationships for the task management system', 'Completed', 1, DATEADD(day, -1, GETDATE())),
    ('Implement API Endpoints', 'Create RESTful API endpoints for CRUD operations', 'In Progress', 1, DATEADD(day, 1, GETDATE())),
    ('Build Vue.js Frontend', 'Create responsive web interface using Vue 3', 'Pending', 2, DATEADD(day, 2, GETDATE())),
    ('Develop Mobile App', 'Build .NET MAUI app with WebView integration', 'Pending', 2, DATEADD(day, 3, GETDATE())),
    ('Write Documentation', 'Create comprehensive documentation for the project', 'Pending', 3, DATEADD(day, 4, GETDATE()));
END
GO

PRINT 'Database setup completed successfully!';