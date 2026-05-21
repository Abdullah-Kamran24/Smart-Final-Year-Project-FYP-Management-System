# FYP Management System — Backend API

**Group:** Mehaal Khan (23P-0544), Abdullah Kamran (23P-0612), Mustafa Naeem (23P-0501)

## Tech Stack
- ASP.NET Core 8.0 Web API
- Entity Framework Core + SQL Server
- JWT Authentication
- BCrypt password hashing
- Swagger UI
- Deliverables module for milestones, final report, and presentation tracking

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Steps

1. **Restore packages**
   ```bash
   dotnet restore
   ```

2. **Update connection string** in `appsettings.json`
   ```json
   "DefaultConnection": "Server=localhost;Database=FYPDB;Trusted_Connection=True;TrustServerCertificate=True;"
   ```

3. **Apply migrations** (runs automatically on startup, or manually):
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Run the API**
   ```bash
   dotnet run
   ```

5. **Open Swagger UI**
   ```
   http://localhost:5000/swagger
   ```

## Seed Credentials

| Role       | Email              | Password       |
|------------|--------------------|----------------|
| Admin      | admin@fyp.com      | admin123       |
| Supervisor | saleem@fyp.com     | supervisor123  |
| Supervisor | faisal@fyp.com     | supervisor123  |
| Student    | mehaal22@fyp.com   | student123     |
| Student    | abdullah23@fyp.com | student123     |
| Student    | mustafa24@fyp.com  | student123     |

## API Endpoints

### Auth
| Method | Endpoint              | Description       |
|--------|-----------------------|-------------------|
| POST   | /api/auth/register    | Register user     |
| POST   | /api/auth/login       | Login & get JWT   |
| GET    | /api/auth/users       | Get all users     |

### Projects
| Method | Endpoint                          | Description              |
|--------|-----------------------------------|--------------------------|
| GET    | /api/project                      | All projects             |
| GET    | /api/project/{id}                 | Project by ID            |
| GET    | /api/project/student/{id}         | Projects by student      |
| GET    | /api/project/supervisor/{id}      | Projects by supervisor   |
| GET    | /api/project/stats                | Dashboard statistics     |
| POST   | /api/project                      | Create project           |
| PUT    | /api/project/{id}                 | Update project           |
| DELETE | /api/project/{id}                 | Delete project           |
| POST   | /api/project/ai-assign/{id}       | AI supervisor assignment |

### Proposals
| Method | Endpoint            | Description         |
|--------|---------------------|---------------------|
| GET    | /api/proposal       | All proposals       |
| GET    | /api/proposal/{id}  | Proposal by ID      |
| POST   | /api/proposal       | Submit proposal     |
| PUT    | /api/proposal/{id}  | Approve/Reject      |
| DELETE | /api/proposal/{id}  | Delete proposal     |

### Progress
| Method | Endpoint                       | Description           |
|--------|--------------------------------|-----------------------|
| GET    | /api/progress                  | All reports           |
| GET    | /api/progress/project/{id}     | Reports by project    |
| POST   | /api/progress                  | Submit text report    |
| POST   | /api/progress/upload           | Upload file report    |
| PUT    | /api/progress/{id}             | Update report         |
| DELETE | /api/progress/{id}             | Delete report         |

### Deliverables
| Method | Endpoint                             | Description                         |
|--------|--------------------------------------|-------------------------------------|
| GET    | /api/deliverable                     | All deliverables                    |
| GET    | /api/deliverable/project/{id}        | Deliverables by project             |
| POST   | /api/deliverable                     | Create milestone/report/presentation |
| PUT    | /api/deliverable/{id}                | Update deliverable                  |
| POST   | /api/deliverable/{id}/upload         | Upload deliverable file             |
| DELETE | /api/deliverable/{id}                | Delete deliverable                  |

### Evaluations
| Method | Endpoint                        | Description          |
|--------|---------------------------------|----------------------|
| GET    | /api/evaluation                 | All evaluations      |
| GET    | /api/evaluation/project/{id}    | Evals by project     |
| POST   | /api/evaluation                 | Submit evaluation    |
| PUT    | /api/evaluation/{id}            | Update evaluation    |
| DELETE | /api/evaluation/{id}            | Delete evaluation    |

### Supervisors
| Method | Endpoint                  | Description          |
|--------|---------------------------|----------------------|
| GET    | /api/supervisor           | All supervisors      |
| GET    | /api/supervisor/{id}      | Supervisor by ID     |
| GET    | /api/supervisor/workload  | Workload stats       |
