<div align="center">

# 🎓 Smart FYP Management System
### *Digitizing the Final Year Project Experience at FAST University*

[![ASP.NET Core](https://img.shields.io/badge/Backend-ASP.NET%20Core%208-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/Frontend-React%2018-61DAFB?style=for-the-badge&logo=react)](https://reactjs.org/)
[![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![JWT](https://img.shields.io/badge/Auth-JWT-000000?style=for-the-badge&logo=jsonwebtokens)](https://jwt.io/)

</div>

---

## 📖 About the Project

In today's rapidly growing technological era, students at FAST University still face difficulties managing their Final Year Projects the old-fashioned way. Physically approaching supervisors to check availability, submitting progress on paper, and waiting for feedback in person — it's slow, inefficient, and frustrating for everyone involved.

**Smart FYP Management System** changes all of that.

This platform automates and simplifies the entire FYP lifecycle — from group formation and proposal submission, all the way to supervisor evaluation and final grading. Everything is online, organized, and accessible in one place.

> *Less paperwork. More progress.*

---

## 👥 Group Information

| Field | Details |
|---|---|
| **Group Number** | Group 24 |
| **Project Title** | Smart Final Year Project (FYP) Management System |
| **GitHub Repository** | [Smart-Final-Year-Project-FYP-Management-System](https://github.com/Abdullah-Kamran24/Smart-Final-Year-Project-FYP-Management-System) |

### Group Members

| Name | Roll Number |
|---|---|
| Mehaal Khan | 23P-0544 |
| Abdullah Kamran | 23P-0612 |
| Mustafa Naeem | 23P-0501 |

---

## ✨ What Can It Do?

The system covers the complete FYP journey from day one to final submission:

**For Students**
- 📁 Create and manage project groups
- 📝 Submit project proposals for supervisor review
- 📊 Upload weekly progress reports and files
- 📦 Submit deliverables milestones, final reports, and presentations
- 🔔 Stay updated on approvals, feedback, and evaluations

**For Supervisors**
- ✅ Review and approve or reject proposals
- 💬 Provide detailed feedback and evaluations
- 📈 Monitor student progress across all assigned projects
- 🎯 Assign marks and grades with a built in grade calculator

**For Admins**
- 👤 Manage all users students, supervisors, and admins
- 🗂️ Oversee all groups, projects, and proposals system-wide
- 📊 View dashboards with live statistics and charts

---

## 🛠️ Technologies Used

| Layer | Technology |
|---|---|
| **Backend** | ASP.NET Core 8 Web API (C#) |
| **Database** | Microsoft SQL Server (RDBMS) |
| **ORM / Data Access** | Entity Framework Core (Code-First) + ADO.NET |
| **Frontend** | React 18 + Vite |
| **Styling** | Custom CSS, HTML5 |
| **Authentication** | JWT (JSON Web Tokens) + BCrypt password hashing |
| **Charts** | Chart.js with react-chartjs-2 |

---

## 🚀 Getting Started

Follow these steps to run the project on your local machine.

### Prerequisites

Make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js v18+](https://nodejs.org/)
- [Microsoft SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

---

### Step 1 — Database Setup

1. Open **SQL Server Management Studio (SSMS)**
2. Connect to your server: `.\SQLEXPRESS`
3. Open the file `FYPManagementSystem/database_setup.sql`
4. Click **Execute (F5)**

This will automatically create the `FYPManagementDB` database and seed it with all tables, groups, projects, and default users.

> 💡 **Shortcut:** Double-click `RESET_DATABASE.bat` in the project root to run the SQL script automatically.

---

### Step 2 — Run the Backend

Open a terminal and run:

```bash
cd FYPManagementSystem
dotnet run
```

The backend API will start at **http://localhost:5000**

---

### Step 3 — Run the Frontend

Open a **new terminal** and run:

```bash
cd fyp-frontend
npm install
npm run dev -- --port 3001
```

The frontend will be live at **http://localhost:3001**

---

## 🔑 Demo Credentials

Use these accounts to explore the system right away:

| Role | Email | Password |
|---|---|---|
| 👑 **Admin** | `admin@fyp.com` | `admin123` |
| 🎓 **Supervisor** | `saleem@fyp.com` | `supervisor123` |
| 📚 **Student** | `mehaal22@fyp.com` | `student123` |

---

## 📁 Project Structure

```
Smart-FYP-Management-System/
│
├── FYPManagementSystem/          ← ASP.NET Core Backend
│   ├── Controllers/              ← API Endpoints
│   ├── Models/                   ← Data Models & DTOs
│   ├── Data/                     ← EF Core DbContext & Seeding
│   ├── Uploads/                  ← Uploaded student files
│   ├── Program.cs                ← App configuration
│   ├── appsettings.json          ← Connection string & JWT config
│   └── database_setup.sql        ← Full SQL setup script
│
└── fyp-frontend/                 ← React Frontend
    └── src/
        ├── pages/                ← Dashboard, Projects, Proposals...
        ├── components/           ← Shared UI components
        ├── context/              ← Auth context (JWT state)
        └── api.js                ← Axios instance
```

---

## 🗄️ Database Schema

The system manages **7 core tables**:

```
Users → Groups → GroupMembers
                    ↓
                Projects → Proposals
                         → Progress
                         → Evaluations
```

| Table | Description |
|---|---|
| `Users` | All system users with roles (Admin / Supervisor / Student) |
| `Groups` | Student project groups |
| `GroupMembers` | Maps students to their groups |
| `Projects` | FYP projects linked to groups and supervisors |
| `Proposals` | Submitted proposals with approval status |
| `Progress` | Weekly progress reports and uploaded files |
| `Evaluations` | Final marks, feedback, and grades |

---

## 🔒 Security

- All passwords are hashed using **BCrypt** — plain text passwords are never stored
- Every API route is protected using **JWT Bearer tokens**
- Role-based access control ensures Students, Supervisors, and Admins only see what they're meant to

---

<div align="center">

Made by:
*Mehaal Khan(23P-0544) • Abdullah Kamran(23P-0612) • Mustafa Naeem*(23P-0501)

</div>
