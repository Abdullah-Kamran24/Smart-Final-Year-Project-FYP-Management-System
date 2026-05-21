# Smart Final Year Project (FYP) Management System

## Group Information
- **Group Number:** Group 24
- **Project Title:** Smart Final Year Project (FYP) Management System
- **GitHub Repository URL:** https://github.com/Abdullah-Kamran24/Smart-Final-Year-Project-FYP-Management-System

### Group Members
| Name | Roll Number |
| --- | --- |
| Mehaal Khan | 23P-0544 |
| Abdullah Kamran | 23P-0612 |
| Mustafa Naeem | 23P-0501 |

## Short Description
The Smart FYP Management System handles the complete lifecycle of final year projects in a university. It covers student group creation, project proposal submission and approval, supervisor assignment, progress tracking, deliverables management (milestones, final reports, presentations), and project evaluation with marks and grading. The application supports Students, Supervisors, and Admin roles.

## List of Technologies Used
- **Backend:** ASP.NET Core 8 Web API (C#)
- **Database:** Microsoft SQL Server (RDBMS)
- **Database Access:** Entity Framework Core (Code-First) & ADO.NET (for raw SQL execution)
- **Frontend:** React 18, Vite
- **Styling:** Custom CSS, HTML5
- **Authentication:** JSON Web Tokens (JWT), BCrypt for password hashing
- **Charts:** Chart.js with react-chartjs-2

## Installation and Run Instructions

### Prerequisites
1. **.NET 8 SDK**
2. **Node.js** (v18 or later)
3. **Microsoft SQL Server Express** (or another SQL Server instance)

### Step 1: Database Setup
1. Open SQL Server Management Studio (SSMS).
2. Connect to `.\SQLEXPRESS`.
3. Open and execute the `database_setup.sql` script provided in the `FYPManagementSystem` folder. This will automatically create the `FYPDB` database and seed it with all required tables and default users.
*(Alternatively, simply double-click the `RESET_DATABASE.bat` file in the project root to run this automatically).*

### Step 2: Running the Backend
1. Open a terminal and navigate to the backend folder:
   ```bash
   cd FYPManagementSystem
   ```
2. Start the ASP.NET Core server:
   ```bash
   dotnet run
   ```
   *The backend will run on `http://localhost:5000`.*

### Step 3: Running the Frontend
1. Open a **new** terminal and navigate to the frontend folder:
   ```bash
   cd fyp-frontend
   ```
2. Install the Node.js dependencies:
   ```bash
   npm install
   ```
3. Start the Vite development server:
   ```bash
   npm run dev -- --port 3001
   ```
   *The frontend will run on `http://localhost:3001`.*

### Demo Credentials
- **Admin:** admin@fyp.com / admin123
- **Supervisor:** saleem@fyp.com / supervisor123
- **Student:** mehaal22@fyp.com / student123
