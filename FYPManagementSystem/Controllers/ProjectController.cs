using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYPManagementSystem.Data;
using FYPManagementSystem.Models;
using FYPManagementSystem.Models.DTOs;

namespace FYPManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        // ── GET api/project ───────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _context.Projects
                .Include(p => p.Group)
                    .ThenInclude(g => g!.Members)
                        .ThenInclude(m => m.Student)
                .Include(p => p.Supervisor)
                .Select(p => new ProjectResponseDto
                {
                    Id              = p.Id,
                    Title           = p.Title,
                    Description     = p.Description,
                    TechnologyStack = p.TechnologyStack,
                    Status          = p.Status,
                    GroupId         = p.GroupId,
                    GroupName       = p.Group != null ? p.Group.GroupName : null,
                    SupervisorId    = p.SupervisorId,
                    SupervisorName  = p.Supervisor != null ? p.Supervisor.Name : null,
                    MemberNames     = p.Group != null && p.Group.Members != null
                        ? string.Join(", ", p.Group.Members
                            .Where(m => m.Student != null)
                            .Select(m => m.Student!.Name))
                        : null,
                    CreatedAt       = p.CreatedAt
                })
                .ToListAsync();

            return Ok(projects);
        }

        // ── GET api/project/{id} ──────────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Group)
                    .ThenInclude(g => g!.Members)
                        .ThenInclude(m => m.Student)
                .Include(p => p.Supervisor)
                .Include(p => p.Proposals)
                .Include(p => p.ProgressReports)
                .Include(p => p.Evaluations)
                .Include(p => p.Deliverables)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound(new { message = "Project not found." });

            return Ok(project);
        }

        // ── GET api/project/group/{groupId} ──────────────────────────────────
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetProjectByGroup(int groupId)
        {
            var project = await _context.Projects
                .Include(p => p.Group)
                    .ThenInclude(g => g!.Members)
                        .ThenInclude(m => m.Student)
                .Include(p => p.Supervisor)
                .Where(p => p.GroupId == groupId)
                .Select(p => new ProjectResponseDto
                {
                    Id              = p.Id,
                    Title           = p.Title,
                    Description     = p.Description,
                    TechnologyStack = p.TechnologyStack,
                    Status          = p.Status,
                    GroupId         = p.GroupId,
                    GroupName       = p.Group != null ? p.Group.GroupName : null,
                    SupervisorId    = p.SupervisorId,
                    SupervisorName  = p.Supervisor != null ? p.Supervisor.Name : "Not Assigned",
                    MemberNames     = p.Group != null && p.Group.Members != null
                        ? string.Join(", ", p.Group.Members
                            .Where(m => m.Student != null)
                            .Select(m => m.Student!.Name))
                        : null,
                    CreatedAt       = p.CreatedAt
                })
                .ToListAsync();

            return Ok(project);
        }

        // ── GET api/project/supervisor/{supervisorId} ─────────────────────────
        [HttpGet("supervisor/{supervisorId}")]
        public async Task<IActionResult> GetProjectsBySupervisor(int supervisorId)
        {
            var projects = await _context.Projects
                .Include(p => p.Group)
                    .ThenInclude(g => g!.Members)
                        .ThenInclude(m => m.Student)
                .Where(p => p.SupervisorId == supervisorId)
                .Select(p => new ProjectResponseDto
                {
                    Id              = p.Id,
                    Title           = p.Title,
                    Description     = p.Description,
                    TechnologyStack = p.TechnologyStack,
                    Status          = p.Status,
                    GroupId         = p.GroupId,
                    GroupName       = p.Group != null ? p.Group.GroupName : null,
                    SupervisorId    = p.SupervisorId,
                    MemberNames     = p.Group != null && p.Group.Members != null
                        ? string.Join(", ", p.Group.Members
                            .Where(m => m.Student != null)
                            .Select(m => m.Student!.Name))
                        : null,
                    CreatedAt       = p.CreatedAt
                })
                .ToListAsync();

            return Ok(projects);
        }

        // ── GET api/project/student/{studentId} ───────────────────────────────
        // Find the student's group, then return that group's project
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetProjectByStudent(int studentId)
        {
            var membership = await _context.GroupMembers
                .FirstOrDefaultAsync(gm => gm.StudentId == studentId);

            if (membership == null)
                return Ok(new List<ProjectResponseDto>());

            var projects = await _context.Projects
                .Include(p => p.Group)
                    .ThenInclude(g => g!.Members)
                        .ThenInclude(m => m.Student)
                .Include(p => p.Supervisor)
                .Where(p => p.GroupId == membership.GroupId)
                .Select(p => new ProjectResponseDto
                {
                    Id              = p.Id,
                    Title           = p.Title,
                    Description     = p.Description,
                    TechnologyStack = p.TechnologyStack,
                    Status          = p.Status,
                    GroupId         = p.GroupId,
                    GroupName       = p.Group != null ? p.Group.GroupName : null,
                    SupervisorId    = p.SupervisorId,
                    SupervisorName  = p.Supervisor != null ? p.Supervisor.Name : "Not Assigned",
                    MemberNames     = p.Group != null && p.Group.Members != null
                        ? string.Join(", ", p.Group.Members
                            .Where(m => m.Student != null)
                            .Select(m => m.Student!.Name))
                        : null,
                    CreatedAt       = p.CreatedAt
                })
                .ToListAsync();

            return Ok(projects);
        }

        // ── POST api/project ──────────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
        {
            var group = await _context.Groups.FindAsync(dto.GroupId);
            if (group == null)
                return BadRequest(new { message = "Invalid group ID." });

            // One project per group
            var exists = await _context.Projects.AnyAsync(p => p.GroupId == dto.GroupId);
            if (exists)
                return BadRequest(new { message = "This group already has a project." });

            var project = new Project
            {
                Title           = dto.Title,
                Description     = dto.Description,
                TechnologyStack = dto.TechnologyStack,
                GroupId         = dto.GroupId,
                Status          = "Pending",
                CreatedAt       = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Project created successfully!", projectId = project.Id });
        }

        // ── PUT api/project/{id} ──────────────────────────────────────────────
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] CreateProjectDto dto)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound(new { message = "Project not found." });

            project.Title           = dto.Title;
            project.Description     = dto.Description;
            project.TechnologyStack = dto.TechnologyStack;

            if (dto.GroupId != project.GroupId)
            {
                var group = await _context.Groups.FindAsync(dto.GroupId);
                if (group == null)
                    return BadRequest(new { message = "Invalid group ID." });

                var groupHasProject = await _context.Projects
                    .AnyAsync(p => p.GroupId == dto.GroupId && p.Id != id);
                if (groupHasProject)
                    return BadRequest(new { message = "This group already has a project." });

                project.GroupId = dto.GroupId;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Project updated successfully!" });
        }

        // ── DELETE api/project/{id} ───────────────────────────────────────────
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound(new { message = "Project not found." });

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Project deleted successfully!" });
        }

        // ── POST api/project/ai-assign/{projectId} ────────────────────────────
        [HttpPost("ai-assign/{projectId}")]
        public async Task<IActionResult> AssignSupervisorAI(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Group)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return NotFound(new { message = "Project not found." });

            var supervisors = await _context.Users
                .Where(u => u.Role == "Supervisor")
                .ToListAsync();

            if (!supervisors.Any())
                return BadRequest(new { message = "No supervisors available." });

            var allProjects = await _context.Projects.ToListAsync();

            var best = supervisors
                .Select(s => new
                {
                    Supervisor    = s,
                    WorkloadScore = 10 - allProjects.Count(p => p.SupervisorId == s.Id),
                    ExpertiseScore = (!string.IsNullOrEmpty(s.Expertise) &&
                                      !string.IsNullOrEmpty(project.TechnologyStack) &&
                                      project.TechnologyStack.Contains(
                                          s.Expertise, StringComparison.OrdinalIgnoreCase)) ? 5 : 0
                })
                .Select(x => new { x.Supervisor, TotalScore = x.WorkloadScore + x.ExpertiseScore })
                .OrderByDescending(x => x.TotalScore)
                .First();

            project.SupervisorId = best.Supervisor.Id;
            project.Status       = "Active";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message        = $"Supervisor '{best.Supervisor.Name}' assigned via AI scoring.",
                supervisorId   = best.Supervisor.Id,
                supervisorName = best.Supervisor.Name,
                score          = best.TotalScore
            });
        }

        // ── GET api/project/stats ─────────────────────────────────────────────
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new DashboardStatsDto
            {
                TotalProjects     = await _context.Projects.CountAsync(),
                TotalStudents     = await _context.Users.CountAsync(u => u.Role == "Student"),
                TotalSupervisors  = await _context.Users.CountAsync(u => u.Role == "Supervisor"),
                TotalGroups       = await _context.Groups.CountAsync(),
                ApprovedProposals = await _context.Proposals.CountAsync(p => p.Status == "Approved"),
                PendingProposals  = await _context.Proposals.CountAsync(p => p.Status == "Pending"),
                RejectedProposals = await _context.Proposals.CountAsync(p => p.Status == "Rejected"),
                CompletedProjects = await _context.Projects.CountAsync(p => p.Status == "Completed"),
                ActiveProjects    = await _context.Projects.CountAsync(p => p.Status == "Active")
            };

            return Ok(stats);
        }
    }
}
