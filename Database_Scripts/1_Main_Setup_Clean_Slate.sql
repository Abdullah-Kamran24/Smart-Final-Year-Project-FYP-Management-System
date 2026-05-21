-- ============================================================
-- Smart Final Year Project Management System
-- Microsoft SQL Server setup script
-- Group: Mehaal Khan (23P-0544), Abdullah Kamran (23P-0612),
--        Mustafa Naeem (23P-0501)
-- Preferred setup for the app is: dotnet ef database update.
--
-- ============================================================

IF DB_ID('FYPDB') IS NULL
BEGIN
    CREATE DATABASE FYPDB;
END
GO

USE FYPDB;
GO

IF OBJECT_ID('Deliverables', 'U') IS NOT NULL DROP TABLE Deliverables;
IF OBJECT_ID('Evaluations', 'U') IS NOT NULL DROP TABLE Evaluations;
IF OBJECT_ID('Progress', 'U') IS NOT NULL DROP TABLE Progress;
IF OBJECT_ID('Proposals', 'U') IS NOT NULL DROP TABLE Proposals;
IF OBJECT_ID('Projects', 'U') IS NOT NULL DROP TABLE Projects;
IF OBJECT_ID('GroupMembers', 'U') IS NOT NULL DROP TABLE GroupMembers;
IF OBJECT_ID('Groups', 'U') IS NOT NULL DROP TABLE Groups;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
GO

CREATE TABLE Users (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Name      NVARCHAR(100) NOT NULL,
    Email     NVARCHAR(100) NOT NULL UNIQUE,
    Password  NVARCHAR(MAX) NOT NULL,
    Role      NVARCHAR(20) NOT NULL DEFAULT 'Student',
    Expertise NVARCHAR(200) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_Users_Role CHECK (Role IN ('Student', 'Supervisor', 'Admin'))
);

CREATE TABLE Groups (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    GroupName   NVARCHAR(100) NOT NULL,
    Description NVARCHAR(200) NULL,
    CreatedAt   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE GroupMembers (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    GroupId   INT NOT NULL,
    StudentId INT NOT NULL,
    JoinedAt  DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_GroupMembers_Groups FOREIGN KEY (GroupId) REFERENCES Groups(Id) ON DELETE CASCADE,
    CONSTRAINT FK_GroupMembers_Students FOREIGN KEY (StudentId) REFERENCES Users(Id),
    CONSTRAINT UQ_GroupMembers_GroupStudent UNIQUE (GroupId, StudentId)
);

CREATE TABLE Projects (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Title           NVARCHAR(200) NOT NULL,
    Description     NVARCHAR(MAX) NULL,
    TechnologyStack NVARCHAR(200) NULL,
    GroupId         INT NOT NULL,
    SupervisorId    INT NULL,
    Status          NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    CreatedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Projects_Groups FOREIGN KEY (GroupId) REFERENCES Groups(Id),
    CONSTRAINT FK_Projects_Supervisors FOREIGN KEY (SupervisorId) REFERENCES Users(Id),
    CONSTRAINT CK_Projects_Status CHECK (Status IN ('Pending', 'Active', 'Completed', 'Rejected'))
);

CREATE TABLE Proposals (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId   INT NOT NULL,
    Status      NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    Remarks     NVARCHAR(MAX) NULL,
    SubmittedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Proposals_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    CONSTRAINT CK_Proposals_Status CHECK (Status IN ('Pending', 'Approved', 'Rejected'))
);

CREATE TABLE Progress (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId     INT NOT NULL,
    Report        NVARCHAR(MAX) NULL,
    FilePath      NVARCHAR(300) NULL,
    DateSubmitted DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Progress_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
);

CREATE TABLE Evaluations (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId   INT NOT NULL,
    Marks       INT NOT NULL,
    Feedback    NVARCHAR(MAX) NULL,
    EvaluatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Evaluations_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    CONSTRAINT CK_Evaluations_Marks CHECK (Marks BETWEEN 0 AND 100)
);

CREATE TABLE Deliverables (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId   INT NOT NULL,
    Title       NVARCHAR(150) NOT NULL,
    Type        NVARCHAR(30) NOT NULL,
    Status      NVARCHAR(30) NOT NULL DEFAULT 'Pending',
    DueDate     DATETIME2 NULL,
    Description NVARCHAR(500) NULL,
    FilePath    NVARCHAR(300) NULL,
    SubmittedAt DATETIME2 NULL,
    CreatedAt   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Deliverables_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    CONSTRAINT CK_Deliverables_Type CHECK (Type IN ('Milestone', 'Final Report', 'Presentation')),
    CONSTRAINT CK_Deliverables_Status CHECK (Status IN ('Pending', 'In Progress', 'Submitted', 'Approved', 'Rejected', 'Completed'))
);
GO

CREATE INDEX IX_GroupMembers_StudentId ON GroupMembers(StudentId);
CREATE INDEX IX_Projects_GroupId ON Projects(GroupId);
CREATE INDEX IX_Projects_SupervisorId ON Projects(SupervisorId);
CREATE INDEX IX_Proposals_ProjectId ON Proposals(ProjectId);
CREATE INDEX IX_Progress_ProjectId ON Progress(ProjectId);
CREATE INDEX IX_Evaluations_ProjectId ON Evaluations(ProjectId);
CREATE INDEX IX_Deliverables_ProjectId ON Deliverables(ProjectId);
GO

-- 1. Insert Essential System Users
-- BCrypt hashes generated by the ASP.NET seed logic.
-- Plain-text viva demo passwords:
-- Admin: admin@fyp.com / admin123
-- Supervisor: saleem@fyp.com / supervisor123
-- Student: mehaal22@fyp.com / student123

INSERT INTO Users (Name, Email, Password, Role, Expertise, CreatedAt) VALUES
-- Admin
('Admin User', 'admin@fyp.com', '$2a$11$9iEZrGFF/YjgBcUBcrQWrOYRMcymvV930FiXq/ggNSDJziJj1XLZ6', 'Admin', NULL, '2024-01-01'),

-- Supervisor
('Dr. Saleem Ahmed', 'saleem@fyp.com', '$2a$11$sEaesDi3bTQ3KGdc4tqXxepxI08m7tAN16SN9Z49It4TQ49QOVMJu', 'Supervisor', 'Machine Learning & AI', '2024-01-01'),

-- Student (Mehaal)
('Mehaal Khan', 'mehaal22@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-01');

-- No initial groups, projects, or deliverables. 
-- The system is now a clean slate, ready for you to create everything live during the viva!

GO

PRINT 'FYPDB setup completed with a clean slate for Microsoft SQL Server.';
