using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FYPManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Student"),
                    Expertise = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMembers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMembers_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TechnologyStack = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SupervisorId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Users_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deliverables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliverables", x => x.Id);
                    table.CheckConstraint("CK_Deliverables_Status", "[Status] IN ('Pending', 'In Progress', 'Submitted', 'Approved', 'Rejected', 'Completed')");
                    table.CheckConstraint("CK_Deliverables_Type", "[Type] IN ('Milestone', 'Final Report', 'Presentation')");
                    table.ForeignKey(
                        name: "FK_Deliverables_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Marks = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvaluatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluations", x => x.Id);
                    table.CheckConstraint("CK_Evaluations_Marks", "[Marks] BETWEEN 0 AND 100");
                    table.ForeignKey(
                        name: "FK_Evaluations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Progress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Report = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Progress_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proposals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.Id);
                    table.CheckConstraint("CK_Proposals_Status", "[Status] IN ('Pending', 'Approved', 'Rejected')");
                    table.ForeignKey(
                        name: "FK_Proposals_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "CreatedAt", "Description", "GroupName" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 1 — AI Innovators", "AI Innovators" },
                    { 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 2 — Code Masters", "Code Masters" },
                    { 3, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 3 — Data Wizards", "Data Wizards" },
                    { 4, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 4 — Cloud Pioneers", "Cloud Pioneers" },
                    { 5, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 5 — Cyber Guardians", "Cyber Guardians" },
                    { 6, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 6 — Mobile Mavens", "Mobile Mavens" },
                    { 7, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 7 — Web Architects", "Web Architects" },
                    { 8, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 8 — Blockchain Builders", "Blockchain Builders" },
                    { 9, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 9 — IoT Engineers", "IoT Engineers" },
                    { 10, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 10 — Vision Lab", "Vision Lab" },
                    { 11, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 11 — NLP Ninjas", "NLP Ninjas" },
                    { 12, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 12 — Robot Squad", "Robot Squad" },
                    { 13, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 13 — DB Experts", "DB Experts" },
                    { 14, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 14 — DevOps Force", "DevOps Force" },
                    { 15, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 15 — AR Creators", "AR Creators" },
                    { 16, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 16 — Big Data Bees", "Big Data Bees" },
                    { 17, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 17 — Game Changers", "Game Changers" },
                    { 18, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 18 — Embedded Elite", "Embedded Elite" },
                    { 19, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 19 — Smart Systems", "Smart Systems" },
                    { 20, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 20 — Deep Learners", "Deep Learners" },
                    { 21, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 21 — Security Sharks", "Security Sharks" },
                    { 22, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 22 — Full Stack Fusion", "Full Stack Fusion" },
                    { 23, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 23 — Quantum Coders", "Quantum Coders" },
                    { 24, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 24 — Green Tech", "Green Tech" },
                    { 25, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 25 — Agile Aces", "Agile Aces" },
                    { 26, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 26 — Network Ninjas", "Network Ninjas" },
                    { 27, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 27 — UX Pioneers", "UX Pioneers" },
                    { 28, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 28 — Open Source Squad", "Open Source Squad" },
                    { 29, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 29 — Tech Transformers", "Tech Transformers" },
                    { 30, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 30 — Algorithm Aces", "Algorithm Aces" },
                    { 31, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 31 — Neural Navigators", "Neural Navigators" },
                    { 32, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 32 — Smart Analytics", "Smart Analytics" },
                    { 33, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FYP Group 33 — Future Builders", "Future Builders" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Expertise", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@fyp.com", null, "Admin User", "$2a$11$ZKf29GtQy.LRFCt3Jt5Yp.cPKwAvBqt2CdpseIrS27PrN2iX.UWPm", "Admin" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "saleem@fyp.com", "Machine Learning", "Dr. Saleem Ahmed", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "faisal@fyp.com", "Web Development", "Dr. Faisal Khan", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "amna@fyp.com", "Artificial Intelligence", "Dr. Amna Riaz", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "usman@fyp.com", "Cybersecurity", "Dr. Usman Tariq", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hina@fyp.com", "Data Science", "Dr. Hina Shahid", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bilal@fyp.com", "Mobile Development", "Dr. Bilal Hassan", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sana@fyp.com", "Cloud Computing", "Dr. Sana Malik", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "raza@fyp.com", "Blockchain", "Dr. Raza Hussain", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nadia@fyp.com", "IoT", "Dr. Nadia Javed", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tariq@fyp.com", "Computer Vision", "Dr. Tariq Mehmood", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "fareeha@fyp.com", "Natural Language Processing", "Dr. Fareeha Akram", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kamran@fyp.com", "Robotics", "Dr. Kamran Iqbal", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ayesha@fyp.com", "Database Systems", "Dr. Ayesha Baig", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "zubair@fyp.com", "Software Engineering", "Dr. Zubair Ahmed", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 16, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rabia@fyp.com", "Human Computer Interaction", "Dr. Rabia Noor", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 17, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "imran@fyp.com", "Game Development", "Dr. Imran Saeed", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mehwish@fyp.com", "Augmented Reality", "Dr. Mehwish Ali", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 19, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owais@fyp.com", "Big Data", "Dr. Owais Raza", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "samina@fyp.com", "DevOps", "Dr. Samina Qadir", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 21, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "naveed@fyp.com", "Embedded Systems", "Dr. Naveed Shah", "$2a$11$vM/AFY79KCCAMSg5bC7gzeaCB8stHZ7kOirxm13l6BM1o1b.8CSl.", "Supervisor" },
                    { 22, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mehaal22@fyp.com", null, "Mehaal Khan", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 23, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "abdullah23@fyp.com", null, "Abdullah Kamran", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 24, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mustafa24@fyp.com", null, "Mustafa Naeem", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ali25@fyp.com", null, "Ali Hassan", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 26, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sara26@fyp.com", null, "Sara Ahmed", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 27, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "usman27@fyp.com", null, "Usman Tariq", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 28, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "fatima28@fyp.com", null, "Fatima Malik", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 29, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bilal29@fyp.com", null, "Bilal Shah", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hina30@fyp.com", null, "Hina Raza", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 31, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "omar31@fyp.com", null, "Omar Farooq", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 32, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "zainab32@fyp.com", null, "Zainab Iqbal", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hamza33@fyp.com", null, "Hamza Butt", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 34, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ayesha34@fyp.com", null, "Ayesha Siddiq", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "talha35@fyp.com", null, "Talha Mehmood", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 36, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "maham36@fyp.com", null, "Maham Qureshi", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 37, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "asad37@fyp.com", null, "Asad Javed", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 38, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nimra38@fyp.com", null, "Nimra Shahid", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 39, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "fawad39@fyp.com", null, "Fawad Hussain", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 40, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "saba40@fyp.com", null, "Saba Noor", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 41, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kashif41@fyp.com", null, "Kashif Rao", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 42, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rabia42@fyp.com", null, "Rabia Khan", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 43, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "daniyal43@fyp.com", null, "Daniyal Ahmed", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 44, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "eman44@fyp.com", null, "Eman Baig", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 45, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "waqas45@fyp.com", null, "Waqas Sohail", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 46, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "maryam46@fyp.com", null, "Maryam Tariq", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 47, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "usama47@fyp.com", null, "Usama Raza", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 48, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "aqsa48@fyp.com", null, "Aqsa Riaz", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 49, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "shahroz49@fyp.com", null, "Shahroz Ali", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 50, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "laraib50@fyp.com", null, "Laraib Saeed", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 51, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "umer51@fyp.com", null, "Umer Cheema", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 52, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "noor52@fyp.com", null, "Noor Fatima", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 53, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "arslan53@fyp.com", null, "Arslan Zafar", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 54, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sidra54@fyp.com", null, "Sidra Hussain", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 55, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kamran55@fyp.com", null, "Kamran Malik", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 56, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "zuha56@fyp.com", null, "Zuha Sheikh", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 57, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owais57@fyp.com", null, "Owais Nawaz", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 58, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bushra58@fyp.com", null, "Bushra Iqbal", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 59, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "talal59@fyp.com", null, "Talal Mirza", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 60, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hafsa60@fyp.com", null, "Hafsa Tariq", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 61, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rizwan61@fyp.com", null, "Rizwan Aslam", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 62, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "iqra62@fyp.com", null, "Iqra Anwar", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 63, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "faizan63@fyp.com", null, "Faizan Rauf", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 64, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tooba64@fyp.com", null, "Tooba Khan", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 65, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "shoaib65@fyp.com", null, "Shoaib Ahmad", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 66, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "misbah66@fyp.com", null, "Misbah Ali", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 67, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hassan67@fyp.com", null, "Hassan Raza", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 68, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "amna68@fyp.com", null, "Amna Qadri", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 69, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "shahzaib69@fyp.com", null, "Shahzaib Gill", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 70, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "lubna70@fyp.com", null, "Lubna Farooqi", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 71, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "adnan71@fyp.com", null, "Adnan Karim", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 72, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rida72@fyp.com", null, "Rida Zubair", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 73, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "muzammil73@fyp.com", null, "Muzammil Awan", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 74, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nida74@fyp.com", null, "Nida Sajid", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 75, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jahanzaib75@fyp.com", null, "Jahanzaib Baig", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 76, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kinza76@fyp.com", null, "Kinza Saleem", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 77, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "zain77@fyp.com", null, "Zain Shabbir", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 78, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "huma78@fyp.com", null, "Huma Naeem", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 79, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mohsin79@fyp.com", null, "Mohsin Latif", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 80, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sadia80@fyp.com", null, "Sadia Maqbool", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 81, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "umar81@fyp.com", null, "Umar Abbasi", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 82, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "aimen82@fyp.com", null, "Aimen Ashraf", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 83, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "saad83@fyp.com", null, "Saad Mehmood", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 84, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "zara84@fyp.com", null, "Zara Siddiq", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 85, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nabeel85@fyp.com", null, "Nabeel Akram", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 86, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sumbul86@fyp.com", null, "Sumbul Haider", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 87, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rehan87@fyp.com", null, "Rehan Qamar", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 88, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "isra88@fyp.com", null, "Isra Rehman", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 89, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sohail89@fyp.com", null, "Sohail Akhtar", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 90, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nawal90@fyp.com", null, "Nawal Shah", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 91, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahsan91@fyp.com", null, "Ahsan Nawaz", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 92, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mariam92@fyp.com", null, "Mariam Zia", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 93, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "farhan93@fyp.com", null, "Farhan Qureshi", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 94, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hira94@fyp.com", null, "Hira Manzoor", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 95, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "shahbaz95@fyp.com", null, "Shahbaz Malik", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 96, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "alina96@fyp.com", null, "Alina Ahmed", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 97, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "muneeb97@fyp.com", null, "Muneeb Raza", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 98, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sana98@fyp.com", null, "Sana Chaudhry", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 99, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "junaid99@fyp.com", null, "Junaid Iqbal", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 100, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "komal100@fyp.com", null, "Komal Arif", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 101, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bilal101@fyp.com", null, "Bilal Aziz", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 102, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "fariha102@fyp.com", null, "Fariha Zaman", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 103, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "arslan103@fyp.com", null, "Arslan Baig", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 104, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tayyaba104@fyp.com", null, "Tayyaba Malik", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 105, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hamid105@fyp.com", null, "Hamid Saeed", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 106, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "amara106@fyp.com", null, "Amara Khan", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 107, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "waqar107@fyp.com", null, "Waqar Hussain", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 108, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "saman108@fyp.com", null, "Saman Tariq", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 109, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rana109@fyp.com", null, "Rana Atif", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 110, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rida110@fyp.com", null, "Rida Hameed", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 111, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahsan111@fyp.com", null, "Ahsan Mirza", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 112, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kiran112@fyp.com", null, "Kiran Shahzad", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 113, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "taha113@fyp.com", null, "Taha Farooq", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 114, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "zara114@fyp.com", null, "Zara Butt", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 115, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "amir115@fyp.com", null, "Amir Sultan", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 116, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sumaira116@fyp.com", null, "Sumaira Rafiq", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 117, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "shayan117@fyp.com", null, "Shayan Ahmed", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 118, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "madiha118@fyp.com", null, "Madiha Iqbal", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 119, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "burhan119@fyp.com", null, "Burhan Ali", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 120, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nisha120@fyp.com", null, "Nisha Arshad", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" },
                    { 121, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "qasim121@fyp.com", null, "Qasim Raza", "$2a$11$7ku.FE/mjGLGEfbhHOjB1OYtRdInjLT7OIx1v4YfCJZyJnvQ64UAu", "Student" }
                });

            migrationBuilder.InsertData(
                table: "GroupMembers",
                columns: new[] { "Id", "GroupId", "JoinedAt", "StudentId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 2, 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 23 },
                    { 3, 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 4, 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 25 },
                    { 5, 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 26 },
                    { 6, 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 27 },
                    { 7, 3, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 28 },
                    { 8, 3, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 29 },
                    { 9, 3, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 30 },
                    { 10, 4, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 31 },
                    { 11, 4, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 32 },
                    { 12, 4, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 33 },
                    { 13, 5, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 34 },
                    { 14, 5, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 35 },
                    { 15, 5, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 36 },
                    { 16, 6, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 37 },
                    { 17, 6, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 38 },
                    { 18, 6, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 39 },
                    { 19, 7, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 40 },
                    { 20, 7, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 41 },
                    { 21, 7, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 42 },
                    { 22, 8, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 43 },
                    { 23, 8, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 44 },
                    { 24, 8, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 45 },
                    { 25, 9, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 46 },
                    { 26, 9, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 47 },
                    { 27, 9, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 48 },
                    { 28, 10, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 49 },
                    { 29, 10, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 50 },
                    { 30, 10, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 51 },
                    { 31, 11, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 52 },
                    { 32, 11, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 53 },
                    { 33, 11, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 54 },
                    { 34, 12, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 55 },
                    { 35, 12, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 56 },
                    { 36, 12, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 57 },
                    { 37, 13, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 58 },
                    { 38, 13, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 59 },
                    { 39, 13, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 60 },
                    { 40, 14, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 61 },
                    { 41, 14, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 62 },
                    { 42, 14, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 63 },
                    { 43, 15, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 64 },
                    { 44, 15, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 65 },
                    { 45, 15, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 66 },
                    { 46, 16, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 67 },
                    { 47, 16, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 68 },
                    { 48, 16, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 69 },
                    { 49, 17, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 70 },
                    { 50, 17, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 71 },
                    { 51, 17, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 72 },
                    { 52, 18, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 73 },
                    { 53, 18, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 74 },
                    { 54, 18, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 75 },
                    { 55, 19, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 76 },
                    { 56, 19, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 77 },
                    { 57, 19, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 78 },
                    { 58, 20, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 79 },
                    { 59, 20, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 80 },
                    { 60, 20, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 81 },
                    { 61, 21, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 82 },
                    { 62, 21, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 83 },
                    { 63, 21, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 84 },
                    { 64, 22, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 85 },
                    { 65, 22, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 86 },
                    { 66, 22, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 87 },
                    { 67, 23, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 88 },
                    { 68, 23, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 89 },
                    { 69, 23, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 90 },
                    { 70, 24, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 91 },
                    { 71, 24, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 92 },
                    { 72, 24, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 93 },
                    { 73, 25, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 94 },
                    { 74, 25, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 95 },
                    { 75, 25, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 96 },
                    { 76, 26, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 97 },
                    { 77, 26, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 98 },
                    { 78, 26, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 99 },
                    { 79, 27, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 80, 27, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 81, 27, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 82, 28, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 83, 28, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 84, 28, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 85, 29, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 86, 29, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 107 },
                    { 87, 29, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 108 },
                    { 88, 30, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 109 },
                    { 89, 30, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 110 },
                    { 90, 30, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 111 },
                    { 91, 31, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 112 },
                    { 92, 31, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 113 },
                    { 93, 31, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 114 },
                    { 94, 32, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 115 },
                    { 95, 32, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 116 },
                    { 96, 32, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 117 },
                    { 97, 33, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 118 },
                    { 98, 33, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 119 },
                    { 99, 33, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 120 }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedAt", "Description", "GroupId", "Status", "SupervisorId", "TechnologyStack", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Smart FYP Management System", 1, "Completed", 2, "ASP.NET Core, React, SQL Server", "Smart FYP Management System" },
                    { 2, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: AI Chatbot for University", 2, "Completed", 3, "Python, Machine Learning, React", "AI Chatbot for University" },
                    { 3, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Cloud-Based LMS", 3, "Completed", 4, "React, Node.js, MongoDB", "Cloud-Based LMS" },
                    { 4, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Blockchain Certificate Verification", 4, "Completed", 5, "Solidity, Ethereum, React", "Blockchain Certificate Verification" },
                    { 5, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: IoT Smart Campus", 5, "Completed", 6, "Arduino, MQTT, Node.js", "IoT Smart Campus" },
                    { 6, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Computer Vision Attendance", 6, "Active", 7, "Python, OpenCV, TensorFlow", "Computer Vision Attendance" },
                    { 7, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: NLP Research Assistant", 7, "Active", 8, "Python, NLTK, FastAPI", "NLP Research Assistant" },
                    { 8, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Autonomous Robot Navigation", 8, "Active", 9, "ROS, Python, C++", "Autonomous Robot Navigation" },
                    { 9, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Hospital DB Management", 9, "Active", 10, "PostgreSQL, ASP.NET Core, Angular", "Hospital DB Management" },
                    { 10, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: CI/CD Pipeline Automation", 10, "Active", 11, "Docker, Kubernetes, Jenkins", "CI/CD Pipeline Automation" },
                    { 11, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: AR Campus Tour", 11, "Active", 12, "Unity, ARCore, C#", "AR Campus Tour" },
                    { 12, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Big Data Analytics Dashboard", 12, "Active", 13, "Apache Spark, Python, Kafka", "Big Data Analytics Dashboard" },
                    { 13, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Multiplayer Online Game Engine", 13, "Active", 14, "Unity, C#, Photon", "Multiplayer Online Game Engine" },
                    { 14, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Smart Home Embedded System", 14, "Active", 15, "C, FreeRTOS, ARM", "Smart Home Embedded System" },
                    { 15, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Deep Learning Diagnosis", 15, "Active", 16, "Python, TensorFlow, Flask", "Deep Learning Diagnosis" },
                    { 16, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Network Intrusion Detection", 16, "Pending", 17, "Python, Scikit-learn, ELK Stack", "Network Intrusion Detection" },
                    { 17, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Full-Stack E-Commerce Platform", 17, "Pending", 18, "React, Node.js, PostgreSQL", "Full-Stack E-Commerce Platform" },
                    { 18, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Quantum Algorithm Simulator", 18, "Pending", 19, "Python, Qiskit, NumPy", "Quantum Algorithm Simulator" },
                    { 19, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Renewable Energy Monitor", 19, "Pending", 20, "React, Python, MQTT", "Renewable Energy Monitor" },
                    { 20, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Agile Project Tracker", 20, "Pending", 21, "React, Django, PostgreSQL", "Agile Project Tracker" },
                    { 21, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Peer-to-Peer File Sharing", 21, "Pending", 2, "Python, BitTorrent, Flask", "Peer-to-Peer File Sharing" },
                    { 22, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Accessibility UX Toolkit", 22, "Pending", 3, "React, Figma, TypeScript", "Accessibility UX Toolkit" },
                    { 23, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Open-Source Code Review Tool", 23, "Pending", 4, "GitHub API, React, Node.js", "Open-Source Code Review Tool" },
                    { 24, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Digital Twin Campus", 24, "Pending", 5, "Three.js, Unity, WebGL", "Digital Twin Campus" },
                    { 25, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: AI Code Generator", 25, "Pending", 6, "Python, OpenAI API, React", "AI Code Generator" },
                    { 26, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Student Mental Health App", 26, "Pending", 7, "React Native, Firebase, Node.js", "Student Mental Health App" },
                    { 27, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Neural Style Transfer App", 27, "Pending", 8, "Python, PyTorch, React", "Neural Style Transfer App" },
                    { 28, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Real-Time Analytics Platform", 28, "Pending", 9, "Apache Flink, React, InfluxDB", "Real-Time Analytics Platform" },
                    { 29, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Predictive Maintenance System", 29, "Pending", 10, "Python, Scikit-learn, IoT", "Predictive Maintenance System" },
                    { 30, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: E-Voting Blockchain", 30, "Pending", 11, "Solidity, React, Web3.js", "E-Voting Blockchain" },
                    { 31, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Smart Traffic Management", 31, "Pending", 12, "Python, SUMO, OpenCV", "Smart Traffic Management" },
                    { 32, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Personalized Learning Engine", 32, "Pending", 13, "Python, TensorFlow, Django", "Personalized Learning Engine" },
                    { 33, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "A university FYP project: Federated Learning Platform", 33, "Pending", 14, "Python, PySyft, Flask", "Federated Learning Platform" }
                });

            migrationBuilder.InsertData(
                table: "Deliverables",
                columns: new[] { "Id", "CreatedAt", "Description", "DueDate", "FilePath", "ProjectId", "Status", "SubmittedAt", "Title", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supervisor-approved project proposal.", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, "Completed", new DateTime(2024, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Proposal Approval", "Milestone" },
                    { 2, new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Final report draft uploaded for evaluation.", new DateTime(2024, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, "Submitted", new DateTime(2024, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Final Report", "Final Report" },
                    { 3, new DateTime(2024, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Presentation slides for committee review.", new DateTime(2024, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, "In Progress", null, "Final Presentation", "Presentation" }
                });

            migrationBuilder.InsertData(
                table: "Evaluations",
                columns: new[] { "Id", "EvaluatedAt", "Feedback", "Marks", "ProjectId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Excellent system architecture and clean code.", 85, 1 },
                    { 2, new DateTime(2024, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Outstanding AI implementation. A+ work.", 90, 2 },
                    { 3, new DateTime(2024, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good effort on cloud integration, minor UI issues.", 78, 3 },
                    { 4, new DateTime(2024, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Exceptional blockchain solution, well documented.", 92, 4 },
                    { 5, new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Great IoT integration with thorough testing.", 88, 5 }
                });

            migrationBuilder.InsertData(
                table: "Progress",
                columns: new[] { "Id", "DateSubmitted", "FilePath", "ProjectId", "Report" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, "Week 1 update: Core module implemented and tested." },
                    { 2, new DateTime(2024, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, "Week 2 update: Core module implemented and tested." },
                    { 3, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, "Week 3 update: Core module implemented and tested." },
                    { 4, new DateTime(2024, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, "Week 4 update: Core module implemented and tested." },
                    { 5, new DateTime(2024, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, "Week 5 update: Core module implemented and tested." },
                    { 6, new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6, "Week 6 update: Core module implemented and tested." },
                    { 7, new DateTime(2024, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7, "Week 7 update: Core module implemented and tested." },
                    { 8, new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8, "Week 8 update: Core module implemented and tested." },
                    { 9, new DateTime(2024, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9, "Week 9 update: Core module implemented and tested." },
                    { 10, new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 10, "Week 10 update: Core module implemented and tested." },
                    { 11, new DateTime(2024, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 11, "Week 11 update: Core module implemented and tested." },
                    { 12, new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12, "Week 12 update: Core module implemented and tested." },
                    { 13, new DateTime(2024, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13, "Week 13 update: Core module implemented and tested." },
                    { 14, new DateTime(2024, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14, "Week 14 update: Core module implemented and tested." },
                    { 15, new DateTime(2024, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15, "Week 15 update: Core module implemented and tested." }
                });

            migrationBuilder.InsertData(
                table: "Proposals",
                columns: new[] { "Id", "ProjectId", "Remarks", "Status", "SubmittedAt" },
                values: new object[,]
                {
                    { 1, 1, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 4, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 6, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 7, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 8, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 9, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 10, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 11, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 12, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 13, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 14, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 15, "Good proposal, approved for development.", "Approved", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 16, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 17, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 18, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 19, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 20, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 21, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 22, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 23, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 24, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 25, null, "Pending", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 26, null, "Rejected", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 27, null, "Rejected", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 28, null, "Rejected", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, 29, null, "Rejected", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, 30, null, "Rejected", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, 31, null, "Rejected", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, 32, null, "Rejected", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, 33, null, "Rejected", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliverables_ProjectId",
                table: "Deliverables",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_ProjectId",
                table: "Evaluations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupId_StudentId",
                table: "GroupMembers",
                columns: new[] { "GroupId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_StudentId",
                table: "GroupMembers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Progress_ProjectId",
                table: "Progress",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_GroupId",
                table: "Projects",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SupervisorId",
                table: "Projects",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_ProjectId",
                table: "Proposals",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deliverables");

            migrationBuilder.DropTable(
                name: "Evaluations");

            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.DropTable(
                name: "Progress");

            migrationBuilder.DropTable(
                name: "Proposals");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
