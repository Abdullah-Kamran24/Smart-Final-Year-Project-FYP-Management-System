-- ==============================================================================
-- 🎓 FYP Management System - Professional Massive Dataset (50+ Entries)
-- ==============================================================================
-- This script contains over 50 realistic, professional data entries to thoroughly 
-- populate your dashboard for a rigorous viva presentation. 
-- 
-- ⚠️ INSTRUCTIONS:
-- 1. Ensure your database is created and the tables exist (using your main setup).
-- 2. Run this script in SQL Server Management Studio (SSMS) against 'FYPDB' 
--    IF you want to instantly fill it with a large amount of realistic data.
-- ==============================================================================

USE FYPDB;
GO

-- ------------------------------------------------------------------------------
-- 1. USERS (20 Entries: 1 Admin, 4 Supervisors, 15 Students)
-- ------------------------------------------------------------------------------
-- Passwords: 'admin123' for Admin, 'supervisor123' for Supervisors, 'student123' for Students.

INSERT INTO Users (Name, Email, Password, Role, Expertise, CreatedAt) VALUES
-- Admin [Id: 1]
('Admin Coordinator', 'admin@fyp.com', '$2a$11$9iEZrGFF/YjgBcUBcrQWrOYRMcymvV930FiXq/ggNSDJziJj1XLZ6', 'Admin', NULL, '2024-01-01'),

-- Supervisors [Id: 2-5]
('Dr. Saleem Ahmed', 'saleem@fyp.com', '$2a$11$sEaesDi3bTQ3KGdc4tqXxepxI08m7tAN16SN9Z49It4TQ49QOVMJu', 'Supervisor', 'Machine Learning & AI', '2024-01-01'),
('Dr. Faisal Khan', 'faisal@fyp.com', '$2a$11$sEaesDi3bTQ3KGdc4tqXxepxI08m7tAN16SN9Z49It4TQ49QOVMJu', 'Supervisor', 'Cloud Infrastructure', '2024-01-01'),
('Dr. Ayesha Tariq', 'ayesha@fyp.com', '$2a$11$sEaesDi3bTQ3KGdc4tqXxepxI08m7tAN16SN9Z49It4TQ49QOVMJu', 'Supervisor', 'Cybersecurity & Networks', '2024-01-01'),
('Prof. Omer Farooq', 'omer@fyp.com', '$2a$11$sEaesDi3bTQ3KGdc4tqXxepxI08m7tAN16SN9Z49It4TQ49QOVMJu', 'Supervisor', 'Software Engineering & Agile', '2024-01-01'),

-- Students (Group 1 - Mehaal's Group) [Id: 6-8]
('Mehaal Khan', 'mehaal22@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-02'),
('Abdullah Kamran', 'abdullah23@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-02'),
('Mustafa Naeem', 'mustafa24@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-02'),

-- Students (Group 2) [Id: 9-11]
('Zain Ali', 'zain@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-03'),
('Hassan Raza', 'hassan@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-03'),
('Fatima Noor', 'fatima@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-03'),

-- Students (Group 3) [Id: 12-14]
('Bilal Ahmed', 'bilal@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-04'),
('Saad Malik', 'saad@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-04'),
('Sarah Iqbal', 'sarah@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-04'),

-- Students (Group 4) [Id: 15-17]
('Usman Tariq', 'usman@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-05'),
('Khadija Riaz', 'khadija@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-05'),
('Ali Hassan', 'ali.hassan@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-05'),

-- Students (Group 5) [Id: 18-20]
('Maha Yasir', 'maha@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-06'),
('Taha Qureshi', 'taha@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-06'),
('Ammar Zafar', 'ammar@fyp.com', '$2a$11$Vd4wNMXLHdxWMlqntj9R7.GxXaJwPutbrJsHm2ELUgkC1gEBnNoJi', 'Student', NULL, '2024-01-06');


-- ------------------------------------------------------------------------------
-- 2. GROUPS (5 Entries)
-- ------------------------------------------------------------------------------
INSERT INTO Groups (GroupName, Description, CreatedAt) VALUES
('Group 24 - FYP Sys', 'Developers of the Smart FYP Management System.', '2024-01-10'),
('Alpha Innovators', 'Focusing on IoT-based Smart Agriculture monitoring and hardware metrics.', '2024-01-11'),
('Cyber Sentinels', 'Developing blockchain protocols for decentralized credential verification.', '2024-01-12'),
('Code Breakers', 'Building an automated penetration testing suite using Python.', '2024-01-13'),
('Visionary AI', 'Creating deep learning models for early-stage retinal disease detection.', '2024-01-14');


-- ------------------------------------------------------------------------------
-- 3. GROUP MEMBERS (15 Entries)
-- ------------------------------------------------------------------------------
INSERT INTO GroupMembers (GroupId, StudentId, JoinedAt) VALUES
(1, 6, '2024-01-10'), (1, 7, '2024-01-10'), (1, 8, '2024-01-10'),     -- Group 1 (Mehaal's)
(2, 9, '2024-01-11'), (2, 10, '2024-01-11'), (2, 11, '2024-01-11'),   -- Group 2
(3, 12, '2024-01-12'), (3, 13, '2024-01-12'), (3, 14, '2024-01-12'),  -- Group 3
(4, 15, '2024-01-13'), (4, 16, '2024-01-13'), (4, 17, '2024-01-13'),  -- Group 4
(5, 18, '2024-01-14'), (5, 19, '2024-01-14'), (5, 20, '2024-01-14');  -- Group 5


-- ------------------------------------------------------------------------------
-- 4. PROJECTS (5 Entries)
-- ------------------------------------------------------------------------------
INSERT INTO Projects (Title, Description, TechnologyStack, GroupId, SupervisorId, Status, CreatedAt) VALUES
('Smart FYP Management System', 'A comprehensive web application to manage final year project workflows natively.', 'ASP.NET Core, React, SQL Server', 1, 2, 'Active', '2024-01-15'),
('IoT Smart Agriculture Monitor', 'A system using soil moisture and temperature sensors to automate irrigation using ML models.', 'Python, Flask, Arduino, React', 2, 3, 'Completed', '2024-01-16'),
('Decentralized Credential Verification', 'A blockchain-based application that issues university degrees to prevent credential forgery.', 'Ethereum, Solidity, Next.js', 3, 4, 'Active', '2024-01-17'),
('Automated Penetration Testing Suite', 'A Python-based framework to simulate and analyze common web application vulnerabilities autonomously.', 'Python, Kali Linux, Bash', 4, 4, 'Rejected', '2024-01-18'),
('Deep Learning for Retinal Scans', 'Applying Convolutional Neural Networks (CNNs) to detect diabetic retinopathy from medical scans.', 'PyTorch, Python, Django', 5, 2, 'Active', '2024-01-19');


-- ------------------------------------------------------------------------------
-- 5. PROPOSALS (5 Entries)
-- ------------------------------------------------------------------------------
INSERT INTO Proposals (ProjectId, Status, Remarks, SubmittedAt) VALUES
(1, 'Approved', 'Approved. The scope thoroughly covers all necessary FYP management modules.', '2024-01-20'),
(2, 'Approved', 'Approved. Hardware sensors must be procured and tested by week 4.', '2024-01-21'),
(3, 'Approved', 'Excellent application of blockchain. Ensure the gas fees are optimized on the testnet.', '2024-01-22'),
(4, 'Rejected', 'The scope is too broad and potentially violates ethical computing guidelines without a sandbox.', '2024-01-23'),
(5, 'Approved', 'Medical dataset ethics approval must be acquired before training the model.', '2024-01-24');


-- ------------------------------------------------------------------------------
-- 6. PROGRESS REPORTS (10 Entries)
-- ------------------------------------------------------------------------------
INSERT INTO Progress (ProjectId, Report, DateSubmitted) VALUES
(1, 'Database schema designed via Entity Framework Core. Auth implemented.', '2024-02-15'),
(1, 'React dashboard created. Complex SQL queries successfully integrated.', '2024-03-20'),
(2, 'Sensor nodes assembled and communicating with the local Raspberry Pi gateway over MQTT.', '2024-02-28'),
(2, 'Machine learning inference model deployed to edge devices. Hardware testing passed.', '2024-04-10'),
(3, 'Smart contracts written and deployed to the Sepolia testnet.', '2024-03-05'),
(3, 'Frontend Web3 integration complete; Metamask logins successfully tested.', '2024-04-05'),
(4, 'Proposal revised and scaled down to focus purely on local XSS vulnerabilities.', '2024-02-10'),
(5, 'Secured 10,000 anonymized retinal scans from the local hospital database.', '2024-02-22'),
(5, 'Initial CNN model trained with an accuracy of 82%. Tuning hyperparameters now.', '2024-03-25'),
(5, 'Model accuracy reached 94%. Developing the web dashboard for doctors.', '2024-04-20');


-- ------------------------------------------------------------------------------
-- 7. DELIVERABLES (15 Entries)
-- ------------------------------------------------------------------------------
INSERT INTO Deliverables (ProjectId, Title, Type, Status, DueDate, Description, SubmittedAt, CreatedAt) VALUES
(1, 'Project Proposal Document', 'Milestone', 'Completed', '2024-02-01', 'Initial proposal detailing scope and objectives.', '2024-01-30', '2024-01-25'),
(1, 'Mid-Evaluation Presentation', 'Presentation', 'Completed', '2024-04-15', 'Slides covering backend architecture and database ERD.', '2024-04-12', '2024-03-01'),
(1, 'Final Project Report', 'Final Report', 'In Progress', '2024-05-25', 'Complete documentation of the SDLC.', NULL, '2024-04-20'),
(2, 'Hardware Procurement List', 'Milestone', 'Completed', '2024-02-10', 'List of IoT sensors and microcontrollers required.', '2024-02-08', '2024-01-25'),
(2, 'Mid-Term Demo', 'Presentation', 'Completed', '2024-04-10', 'Live demonstration of hardware capturing data.', '2024-04-09', '2024-03-01'),
(2, 'Final Report', 'Final Report', 'Submitted', '2024-05-20', 'Draft of the final hardware analysis report.', '2024-05-18', '2024-04-15'),
(3, 'Smart Contract Architecture', 'Milestone', 'Completed', '2024-02-20', 'UML diagram of the blockchain architecture.', '2024-02-19', '2024-01-28'),
(3, 'Smart Contract Audit Report', 'Milestone', 'Completed', '2024-03-10', 'Security audit of the Solidity contracts to prevent re-entrancy.', '2024-03-08', '2024-02-05'),
(3, 'Final Presentation', 'Presentation', 'Completed', '2024-05-10', 'Final viva presentation slides.', '2024-05-09', '2024-04-15'),
(4, 'Revised Proposal', 'Milestone', 'Submitted', '2024-02-15', 'Updated scope based on supervisor feedback.', '2024-02-14', '2024-01-25'),
(5, 'Data Ethics Clearance', 'Milestone', 'Completed', '2024-02-05', 'Signed clearance from the hospital data board.', '2024-02-04', '2024-01-25'),
(5, 'Model Training Logs', 'Milestone', 'Completed', '2024-04-01', 'Loss and Accuracy graphs over 50 epochs.', '2024-03-29', '2024-03-01'),
(5, 'Final Report', 'Final Report', 'In Progress', '2024-05-25', 'Complete breakdown of ML performance metrics.', NULL, '2024-04-20');


-- ------------------------------------------------------------------------------
-- 8. EVALUATIONS (5 Entries)
-- ------------------------------------------------------------------------------
INSERT INTO Evaluations (ProjectId, Marks, Feedback, EvaluatedAt) VALUES
(1, 88, 'Strong database-backed system with complete CRUD flow. Good use of React.', '2024-04-18'),
(2, 95, 'Excellent integration of hardware and software. The live dashboard was highly responsive.', '2024-05-10'),
(3, 91, 'Solid execution of Web3 fundamentals. The smart contracts are secure and robust.', '2024-05-12'),
(4, 0, 'Project pending re-evaluation of the revised proposal.', '2024-02-20'),
(5, 89, 'The CNN accuracy is commendable. Ensure the UI for doctors is intuitive.', '2024-04-22');
GO

PRINT 'Successfully inserted over 50 professional records into FYPDB!';
