using Microsoft.EntityFrameworkCore;
using FYPManagementSystem.Models;

namespace FYPManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User>        Users        { get; set; }
        public DbSet<Group>       Groups       { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Project>     Projects     { get; set; }
        public DbSet<Proposal>    Proposals    { get; set; }
        public DbSet<Progress>    Progress     { get; set; }
        public DbSet<Evaluation>  Evaluations  { get; set; }
        public DbSet<Deliverable> Deliverables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── User ──────────────────────────────────────────────────────────
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Role).HasDefaultValue("Student");
            });

            // ── GroupMember → Group ───────────────────────────────────────────
            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── GroupMember → Student ─────────────────────────────────────────
            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Student)
                .WithMany()
                .HasForeignKey(gm => gm.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique: one student can only appear once per group
            modelBuilder.Entity<GroupMember>()
                .HasIndex(gm => new { gm.GroupId, gm.StudentId })
                .IsUnique();

            // ── Project → Group ───────────────────────────────────────────────
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Projects)
                .HasForeignKey(p => p.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Project → Supervisor ──────────────────────────────────────────
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Supervisor)
                .WithMany()
                .HasForeignKey(p => p.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // ── Check Constraints ─────────────────────────────────────────────
            modelBuilder.Entity<Proposal>()
                .HasCheckConstraint("CK_Proposals_Status",
                    "[Status] IN ('Pending', 'Approved', 'Rejected')");

            modelBuilder.Entity<Evaluation>()
                .HasCheckConstraint("CK_Evaluations_Marks",
                    "[Marks] BETWEEN 0 AND 100");

            // ── Deliverable → Project ────────────────────────────────────────
            modelBuilder.Entity<Deliverable>()
                .HasOne(d => d.Project)
                .WithMany(p => p.Deliverables)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Deliverable>()
                .HasCheckConstraint("CK_Deliverables_Type",
                    "[Type] IN ('Milestone', 'Final Report', 'Presentation')");

            modelBuilder.Entity<Deliverable>()
                .HasCheckConstraint("CK_Deliverables_Status",
                    "[Status] IN ('Pending', 'In Progress', 'Submitted', 'Approved', 'Rejected', 'Completed')");

            // ══════════════════════════════════════════════════════════════════
            //  SEED DATA
            // ══════════════════════════════════════════════════════════════════

            var adminHash = BCrypt.Net.BCrypt.HashPassword("admin123");
            var supHash   = BCrypt.Net.BCrypt.HashPassword("supervisor123");
            var stuHash   = BCrypt.Net.BCrypt.HashPassword("student123");

            // ── 1 Admin ───────────────────────────────────────────────────────
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Admin User",         Email = "admin@fyp.com",
                            Password = adminHash, Role = "Admin",
                            Expertise = null, CreatedAt = new DateTime(2024, 1, 1) }
            );

            // ── 20 Supervisors ────────────────────────────────────────────────
            var supervisors = new[]
            {
                new { Id=2,  Name="Dr. Saleem Ahmed",  Email="saleem@fyp.com",    Expertise="Machine Learning" },
                new { Id=3,  Name="Dr. Faisal Khan",   Email="faisal@fyp.com",    Expertise="Web Development" },
                new { Id=4,  Name="Dr. Amna Riaz",     Email="amna@fyp.com",      Expertise="Artificial Intelligence" },
                new { Id=5,  Name="Dr. Usman Tariq",   Email="usman@fyp.com",     Expertise="Cybersecurity" },
                new { Id=6,  Name="Dr. Hina Shahid",   Email="hina@fyp.com",      Expertise="Data Science" },
                new { Id=7,  Name="Dr. Bilal Hassan",  Email="bilal@fyp.com",     Expertise="Mobile Development" },
                new { Id=8,  Name="Dr. Sana Malik",    Email="sana@fyp.com",      Expertise="Cloud Computing" },
                new { Id=9,  Name="Dr. Raza Hussain",  Email="raza@fyp.com",      Expertise="Blockchain" },
                new { Id=10, Name="Dr. Nadia Javed",   Email="nadia@fyp.com",     Expertise="IoT" },
                new { Id=11, Name="Dr. Tariq Mehmood", Email="tariq@fyp.com",     Expertise="Computer Vision" },
                new { Id=12, Name="Dr. Fareeha Akram", Email="fareeha@fyp.com",   Expertise="Natural Language Processing" },
                new { Id=13, Name="Dr. Kamran Iqbal",  Email="kamran@fyp.com",    Expertise="Robotics" },
                new { Id=14, Name="Dr. Ayesha Baig",   Email="ayesha@fyp.com",    Expertise="Database Systems" },
                new { Id=15, Name="Dr. Zubair Ahmed",  Email="zubair@fyp.com",    Expertise="Software Engineering" },
                new { Id=16, Name="Dr. Rabia Noor",    Email="rabia@fyp.com",     Expertise="Human Computer Interaction" },
                new { Id=17, Name="Dr. Imran Saeed",   Email="imran@fyp.com",     Expertise="Game Development" },
                new { Id=18, Name="Dr. Mehwish Ali",   Email="mehwish@fyp.com",   Expertise="Augmented Reality" },
                new { Id=19, Name="Dr. Owais Raza",    Email="owais@fyp.com",     Expertise="Big Data" },
                new { Id=20, Name="Dr. Samina Qadir",  Email="samina@fyp.com",    Expertise="DevOps" },
                new { Id=21, Name="Dr. Naveed Shah",   Email="naveed@fyp.com",    Expertise="Embedded Systems" },
            };

            foreach (var s in supervisors)
            {
                modelBuilder.Entity<User>().HasData(new User
                {
                    Id        = s.Id,
                    Name      = s.Name,
                    Email     = s.Email,
                    Password  = supHash,
                    Role      = "Supervisor",
                    Expertise = s.Expertise,
                    CreatedAt = new DateTime(2024, 1, 1)
                });
            }

            // ── 100 Students (IDs 22 – 121) ───────────────────────────────────
            var studentNames = new[]
            {
                "Mehaal Khan","Abdullah Kamran","Mustafa Naeem","Ali Hassan","Sara Ahmed",
                "Usman Tariq","Fatima Malik","Bilal Shah","Hina Raza","Omar Farooq",
                "Zainab Iqbal","Hamza Butt","Ayesha Siddiq","Talha Mehmood","Maham Qureshi",
                "Asad Javed","Nimra Shahid","Fawad Hussain","Saba Noor","Kashif Rao",
                "Rabia Khan","Daniyal Ahmed","Eman Baig","Waqas Sohail","Maryam Tariq",
                "Usama Raza","Aqsa Riaz","Shahroz Ali","Laraib Saeed","Umer Cheema",
                "Noor Fatima","Arslan Zafar","Sidra Hussain","Kamran Malik","Zuha Sheikh",
                "Owais Nawaz","Bushra Iqbal","Talal Mirza","Hafsa Tariq","Rizwan Aslam",
                "Iqra Anwar","Faizan Rauf","Tooba Khan","Shoaib Ahmad","Misbah Ali",
                "Hassan Raza","Amna Qadri","Shahzaib Gill","Lubna Farooqi","Adnan Karim",
                "Rida Zubair","Muzammil Awan","Nida Sajid","Jahanzaib Baig","Kinza Saleem",
                "Zain Shabbir","Huma Naeem","Mohsin Latif","Sadia Maqbool","Umar Abbasi",
                "Aimen Ashraf","Saad Mehmood","Zara Siddiq","Nabeel Akram","Sumbul Haider",
                "Rehan Qamar","Isra Rehman","Sohail Akhtar","Nawal Shah","Ahsan Nawaz",
                "Mariam Zia","Farhan Qureshi","Hira Manzoor","Shahbaz Malik","Alina Ahmed",
                "Muneeb Raza","Sana Chaudhry","Junaid Iqbal","Komal Arif","Bilal Aziz",
                "Fariha Zaman","Arslan Baig","Tayyaba Malik","Hamid Saeed","Amara Khan",
                "Waqar Hussain","Saman Tariq","Rana Atif","Rida Hameed","Ahsan Mirza",
                "Kiran Shahzad","Taha Farooq","Zara Butt","Amir Sultan","Sumaira Rafiq",
                "Shayan Ahmed","Madiha Iqbal","Burhan Ali","Nisha Arshad","Qasim Raza"
            };

            for (int i = 0; i < 100; i++)
            {
                int    uid   = 22 + i;
                // simpler: first name + id
                string first = studentNames[i].Split(' ')[0].ToLower();
                modelBuilder.Entity<User>().HasData(new User
                {
                    Id        = uid,
                    Name      = studentNames[i],
                    Email     = $"{first}{uid}@fyp.com",
                    Password  = stuHash,
                    Role      = "Student",
                    Expertise = null,
                    CreatedAt = new DateTime(2024, 1, 1)
                });
            }

            // ── 33 Groups ─────────────────────────────────────────────────────
            var groupNames = new[]
            {
                "AI Innovators","Code Masters","Data Wizards","Cloud Pioneers","Cyber Guardians",
                "Mobile Mavens","Web Architects","Blockchain Builders","IoT Engineers","Vision Lab",
                "NLP Ninjas","Robot Squad","DB Experts","DevOps Force","AR Creators",
                "Big Data Bees","Game Changers","Embedded Elite","Smart Systems","Deep Learners",
                "Security Sharks","Full Stack Fusion","Quantum Coders","Green Tech","Agile Aces",
                "Network Ninjas","UX Pioneers","Open Source Squad","Tech Transformers","Algorithm Aces",
                "Neural Navigators","Smart Analytics","Future Builders"
            };

            for (int g = 0; g < 33; g++)
            {
                modelBuilder.Entity<Group>().HasData(new Group
                {
                    Id          = g + 1,
                    GroupName   = groupNames[g],
                    Description = $"FYP Group {g + 1} — {groupNames[g]}",
                    CreatedAt   = new DateTime(2024, 1, 15)
                });
            }

            // ── 33 × 3 GroupMembers (students 22–120, group 33 gets student 120 + 121) ──
            int gmId = 1;
            for (int g = 0; g < 33; g++)
            {
                int groupId = g + 1;
                for (int m = 0; m < 3; m++)
                {
                    int studentId = 22 + (g * 3) + m;
                    if (studentId > 121) break;          // safety cap
                    modelBuilder.Entity<GroupMember>().HasData(new GroupMember
                    {
                        Id        = gmId++,
                        GroupId   = groupId,
                        StudentId = studentId,
                        JoinedAt  = new DateTime(2024, 1, 15)
                    });
                }
            }

            // ── Project titles (33) ───────────────────────────────────────────
            var projectTitles = new[]
            {
                "Smart FYP Management System","AI Chatbot for University","Cloud-Based LMS",
                "Blockchain Certificate Verification","IoT Smart Campus","Computer Vision Attendance",
                "NLP Research Assistant","Autonomous Robot Navigation","Hospital DB Management",
                "CI/CD Pipeline Automation","AR Campus Tour","Big Data Analytics Dashboard",
                "Multiplayer Online Game Engine","Smart Home Embedded System","Deep Learning Diagnosis",
                "Network Intrusion Detection","Full-Stack E-Commerce Platform","Quantum Algorithm Simulator",
                "Renewable Energy Monitor","Agile Project Tracker","Peer-to-Peer File Sharing",
                "Accessibility UX Toolkit","Open-Source Code Review Tool","Digital Twin Campus",
                "AI Code Generator","Student Mental Health App","Neural Style Transfer App",
                "Real-Time Analytics Platform","Predictive Maintenance System","E-Voting Blockchain",
                "Smart Traffic Management","Personalized Learning Engine","Federated Learning Platform"
            };

            // Supervisor expertise → relevant tech stacks
            var techStacks = new[]
            {
                "ASP.NET Core, React, SQL Server","Python, Machine Learning, React","React, Node.js, MongoDB",
                "Solidity, Ethereum, React","Arduino, MQTT, Node.js","Python, OpenCV, TensorFlow",
                "Python, NLTK, FastAPI","ROS, Python, C++","PostgreSQL, ASP.NET Core, Angular",
                "Docker, Kubernetes, Jenkins","Unity, ARCore, C#","Apache Spark, Python, Kafka",
                "Unity, C#, Photon","C, FreeRTOS, ARM","Python, TensorFlow, Flask",
                "Python, Scikit-learn, ELK Stack","React, Node.js, PostgreSQL","Python, Qiskit, NumPy",
                "React, Python, MQTT","React, Django, PostgreSQL","Python, BitTorrent, Flask",
                "React, Figma, TypeScript","GitHub API, React, Node.js","Three.js, Unity, WebGL",
                "Python, OpenAI API, React","React Native, Firebase, Node.js","Python, PyTorch, React",
                "Apache Flink, React, InfluxDB","Python, Scikit-learn, IoT","Solidity, React, Web3.js",
                "Python, SUMO, OpenCV","Python, TensorFlow, Django","Python, PySyft, Flask"
            };

            // Distribute supervisors round-robin across 33 projects (IDs 2–21 = 20 supervisors)
            for (int p = 0; p < 33; p++)
            {
                int supId = 2 + (p % 20);   // cycles through supervisors 2-21
                modelBuilder.Entity<Project>().HasData(new Project
                {
                    Id              = p + 1,
                    Title           = projectTitles[p],
                    Description     = $"A university FYP project: {projectTitles[p]}",
                    TechnologyStack = techStacks[p],
                    GroupId         = p + 1,
                    SupervisorId    = supId,
                    Status          = p < 5 ? "Completed" : p < 15 ? "Active" : "Pending",
                    CreatedAt       = new DateTime(2024, 1, 20)
                });
            }

            // ── Proposals (one per project) ───────────────────────────────────
            for (int p = 0; p < 33; p++)
            {
                modelBuilder.Entity<Proposal>().HasData(new Proposal
                {
                    Id          = p + 1,
                    ProjectId   = p + 1,
                    Status      = p < 5  ? "Approved" :
                                  p < 15 ? "Approved" :
                                  p < 25 ? "Pending"  : "Rejected",
                    Remarks     = p < 15 ? "Good proposal, approved for development." : null,
                    SubmittedAt = new DateTime(2024, 1, 22)
                });
            }

            // ── Progress Reports (first 15 active/completed projects) ─────────
            for (int p = 0; p < 15; p++)
            {
                modelBuilder.Entity<Progress>().HasData(new Progress
                {
                    Id            = p + 1,
                    ProjectId     = p + 1,
                    Report        = $"Week {p + 1} update: Core module implemented and tested.",
                    FilePath      = null,
                    DateSubmitted = new DateTime(2024, 2, 1).AddDays(p * 7)
                });
            }

            // ── Evaluations (first 5 completed projects) ──────────────────────
            var marks    = new[] { 85, 90, 78, 92, 88 };
            var feedback = new[]
            {
                "Excellent system architecture and clean code.",
                "Outstanding AI implementation. A+ work.",
                "Good effort on cloud integration, minor UI issues.",
                "Exceptional blockchain solution, well documented.",
                "Great IoT integration with thorough testing."
            };

            for (int e = 0; e < 5; e++)
            {
                modelBuilder.Entity<Evaluation>().HasData(new Evaluation
                {
                    Id          = e + 1,
                    ProjectId   = e + 1,
                    Marks       = marks[e],
                    Feedback    = feedback[e],
                    EvaluatedAt = new DateTime(2024, 5, 1).AddDays(e)
                });
            }

            // ── Deliverables for viva demo ───────────────────────────────────
            modelBuilder.Entity<Deliverable>().HasData(
                new Deliverable
                {
                    Id = 1,
                    ProjectId = 1,
                    Title = "Proposal Approval",
                    Type = "Milestone",
                    Status = "Completed",
                    DueDate = new DateTime(2024, 2, 15),
                    Description = "Supervisor-approved project proposal.",
                    FilePath = null,
                    SubmittedAt = new DateTime(2024, 2, 12),
                    CreatedAt = new DateTime(2024, 2, 1)
                },
                new Deliverable
                {
                    Id = 2,
                    ProjectId = 1,
                    Title = "Final Report",
                    Type = "Final Report",
                    Status = "Submitted",
                    DueDate = new DateTime(2024, 5, 10),
                    Description = "Final report draft uploaded for evaluation.",
                    FilePath = null,
                    SubmittedAt = new DateTime(2024, 5, 8),
                    CreatedAt = new DateTime(2024, 4, 20)
                },
                new Deliverable
                {
                    Id = 3,
                    ProjectId = 2,
                    Title = "Final Presentation",
                    Type = "Presentation",
                    Status = "In Progress",
                    DueDate = new DateTime(2024, 5, 18),
                    Description = "Presentation slides for committee review.",
                    FilePath = null,
                    SubmittedAt = null,
                    CreatedAt = new DateTime(2024, 4, 25)
                }
            );
        }
    }
}
