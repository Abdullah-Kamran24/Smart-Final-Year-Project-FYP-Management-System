using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYPManagementSystem.Data;
using FYPManagementSystem.Models;
using FYPManagementSystem.Models.DTOs;

namespace FYPManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GroupController(AppDbContext context)
        {
            _context = context;
        }

        // ── GET api/group ─────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _context.Groups
                .Include(g => g.Members)
                    .ThenInclude(m => m.Student)
                .Include(g => g.Projects)
                    .ThenInclude(p => p.Supervisor)
                .Select(g => new GroupResponseDto
                {
                    Id          = g.Id,
                    GroupName   = g.GroupName,
                    Description = g.Description,
                    CreatedAt   = g.CreatedAt,
                    Members     = g.Members != null
                        ? g.Members.Select(m => new GroupMemberDto
                        {
                            StudentId   = m.StudentId,
                            StudentName = m.Student != null ? m.Student.Name  : "",
                            Email       = m.Student != null ? m.Student.Email : ""
                        }).ToList()
                        : new List<GroupMemberDto>(),
                    ProjectTitle   = g.Projects != null && g.Projects.Any()
                                       ? g.Projects.First().Title : null,
                    ProjectId      = g.Projects != null && g.Projects.Any()
                                       ? g.Projects.First().Id : null,
                    ProjectStatus  = g.Projects != null && g.Projects.Any()
                                       ? g.Projects.First().Status : null,
                    SupervisorName = g.Projects != null && g.Projects.Any() && g.Projects.First().Supervisor != null
                                       ? g.Projects.First().Supervisor!.Name : null,
                    SupervisorId   = g.Projects != null && g.Projects.Any()
                                       ? g.Projects.First().SupervisorId : null
                })
                .ToListAsync();

            return Ok(groups);
        }

        // ── GET api/group/{id} ────────────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                    .ThenInclude(m => m.Student)
                .Include(g => g.Projects)
                    .ThenInclude(p => p.Supervisor)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                return NotFound(new { message = "Group not found." });

            var dto = new GroupResponseDto
            {
                Id          = group.Id,
                GroupName   = group.GroupName,
                Description = group.Description,
                CreatedAt   = group.CreatedAt,
                Members     = group.Members?.Select(m => new GroupMemberDto
                {
                    StudentId   = m.StudentId,
                    StudentName = m.Student?.Name  ?? "",
                    Email       = m.Student?.Email ?? ""
                }).ToList() ?? new(),
                ProjectTitle   = group.Projects?.FirstOrDefault()?.Title,
                ProjectId      = group.Projects?.FirstOrDefault()?.Id,
                ProjectStatus  = group.Projects?.FirstOrDefault()?.Status,
                SupervisorName = group.Projects?.FirstOrDefault()?.Supervisor?.Name,
                SupervisorId   = group.Projects?.FirstOrDefault()?.SupervisorId
            };

            return Ok(dto);
        }

        // ── GET api/group/student/{studentId} ─────────────────────────────────
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var membership = await _context.GroupMembers
                .Include(gm => gm.Group)
                    .ThenInclude(g => g!.Members)
                        .ThenInclude(m => m.Student)
                .Include(gm => gm.Group)
                    .ThenInclude(g => g!.Projects)
                        .ThenInclude(p => p.Supervisor)
                .FirstOrDefaultAsync(gm => gm.StudentId == studentId);

            if (membership == null)
                return Ok(null);  // student not yet in a group

            var g = membership.Group!;
            return Ok(new GroupResponseDto
            {
                Id          = g.Id,
                GroupName   = g.GroupName,
                Description = g.Description,
                CreatedAt   = g.CreatedAt,
                Members     = g.Members?.Select(m => new GroupMemberDto
                {
                    StudentId   = m.StudentId,
                    StudentName = m.Student?.Name  ?? "",
                    Email       = m.Student?.Email ?? ""
                }).ToList() ?? new(),
                ProjectTitle   = g.Projects?.FirstOrDefault()?.Title,
                ProjectId      = g.Projects?.FirstOrDefault()?.Id,
                ProjectStatus  = g.Projects?.FirstOrDefault()?.Status,
                SupervisorName = g.Projects?.FirstOrDefault()?.Supervisor?.Name,
                SupervisorId   = g.Projects?.FirstOrDefault()?.SupervisorId
            });
        }

        // ── GET api/group/supervisor/{supervisorId} ───────────────────────────
        [HttpGet("supervisor/{supervisorId}")]
        public async Task<IActionResult> GetBySupervisor(int supervisorId)
        {
            var projects = await _context.Projects
                .Where(p => p.SupervisorId == supervisorId)
                .Include(p => p.Group)
                    .ThenInclude(g => g!.Members)
                        .ThenInclude(m => m.Student)
                .ToListAsync();

            var result = projects.Select(p => new GroupResponseDto
            {
                Id          = p.Group!.Id,
                GroupName   = p.Group.GroupName,
                Description = p.Group.Description,
                CreatedAt   = p.Group.CreatedAt,
                Members     = p.Group.Members?.Select(m => new GroupMemberDto
                {
                    StudentId   = m.StudentId,
                    StudentName = m.Student?.Name  ?? "",
                    Email       = m.Student?.Email ?? ""
                }).ToList() ?? new(),
                ProjectTitle   = p.Title,
                ProjectId      = p.Id,
                ProjectStatus  = p.Status,
                SupervisorName = null,
                SupervisorId   = supervisorId
            }).ToList();

            return Ok(result);
        }

        // ── POST api/group ────────────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupDto dto)
        {
            // Validate 2–3 students
            if (dto.StudentIds == null || dto.StudentIds.Count < 2 || dto.StudentIds.Count > 3)
                return BadRequest(new { message = "A group must have 2 or 3 students." });

            // Check all student IDs are valid
            foreach (var sid in dto.StudentIds)
            {
                var student = await _context.Users.FindAsync(sid);
                if (student == null || student.Role != "Student")
                    return BadRequest(new { message = $"Invalid student ID: {sid}" });

                // Check student is not already in a group
                var existing = await _context.GroupMembers.AnyAsync(gm => gm.StudentId == sid);
                if (existing)
                    return BadRequest(new { message = $"Student ID {sid} is already in a group." });
            }

            var group = new Group
            {
                GroupName   = dto.GroupName,
                Description = dto.Description,
                CreatedAt   = DateTime.UtcNow
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            foreach (var sid in dto.StudentIds)
            {
                _context.GroupMembers.Add(new GroupMember
                {
                    GroupId   = group.Id,
                    StudentId = sid,
                    JoinedAt  = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Group created successfully!", groupId = group.Id });
        }

        // ── POST api/group/{id}/members ───────────────────────────────────────
        [HttpPost("{id}/members")]
        public async Task<IActionResult> AddMember(int id, [FromBody] AddMemberDto dto)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                return NotFound(new { message = "Group not found." });

            if (group.Members?.Count >= 3)
                return BadRequest(new { message = "Group already has 3 members (maximum)." });

            var student = await _context.Users.FindAsync(dto.StudentId);
            if (student == null || student.Role != "Student")
                return BadRequest(new { message = "Invalid student ID." });

            var alreadyIn = await _context.GroupMembers.AnyAsync(gm => gm.StudentId == dto.StudentId);
            if (alreadyIn)
                return BadRequest(new { message = "Student is already in a group." });

            _context.GroupMembers.Add(new GroupMember
            {
                GroupId   = id,
                StudentId = dto.StudentId,
                JoinedAt  = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return Ok(new { message = "Member added successfully!" });
        }

        // ── DELETE api/group/{id}/members/{studentId} ─────────────────────────
        [HttpDelete("{id}/members/{studentId}")]
        public async Task<IActionResult> RemoveMember(int id, int studentId)
        {
            var member = await _context.GroupMembers
                .FirstOrDefaultAsync(gm => gm.GroupId == id && gm.StudentId == studentId);

            if (member == null)
                return NotFound(new { message = "Member not found in group." });

            _context.GroupMembers.Remove(member);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Member removed." });
        }

        // ── PUT api/group/{id} ────────────────────────────────────────────────
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateGroupDto dto)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound(new { message = "Group not found." });

            group.GroupName   = dto.GroupName;
            group.Description = dto.Description;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Group updated successfully!" });
        }

        // ── DELETE api/group/{id} ─────────────────────────────────────────────
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound(new { message = "Group not found." });

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Group deleted." });
        }

        // ── POST api/group/ai-assign/{groupId} ────────────────────────────────
        // Auto-assign best supervisor to the group's project
        [HttpPost("ai-assign/{groupId}")]
        public async Task<IActionResult> AiAssignSupervisor(int groupId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.GroupId == groupId);

            if (project == null)
                return NotFound(new { message = "No project found for this group." });

            var supervisors = await _context.Users
                .Where(u => u.Role == "Supervisor")
                .ToListAsync();

            if (!supervisors.Any())
                return BadRequest(new { message = "No supervisors available." });

            var allProjects = await _context.Projects.ToListAsync();

            // Score: workload (10 - assigned count) + expertise match (+5)
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
            if (project.Status == "Pending") project.Status = "Active";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message        = $"Supervisor '{best.Supervisor.Name}' assigned via AI scoring.",
                supervisorId   = best.Supervisor.Id,
                supervisorName = best.Supervisor.Name,
                score          = best.TotalScore
            });
        }
    }
}