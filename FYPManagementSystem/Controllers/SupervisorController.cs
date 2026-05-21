using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYPManagementSystem.Data;

namespace FYPManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupervisorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SupervisorController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/supervisor
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var supervisors = await _context.Users
                .Where(u => u.Role == "Supervisor")
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Expertise,
                    ProjectCount = _context.Projects.Count(p => p.SupervisorId == u.Id)
                })
                .ToListAsync();

            return Ok(supervisors);
        }

        // GET api/supervisor/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supervisor = await _context.Users
                .Where(u => u.Id == id && u.Role == "Supervisor")
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Expertise,
                    Projects = _context.Projects
                        .Where(p => p.SupervisorId == u.Id)
                        .Select(p => new { p.Id, p.Title, p.Status })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (supervisor == null)
                return NotFound(new { message = "Supervisor not found." });

            return Ok(supervisor);
        }

        // GET api/supervisor/workload
        [HttpGet("workload")]
        public async Task<IActionResult> GetWorkload()
        {
            var workload = await _context.Users
                .Where(u => u.Role == "Supervisor")
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Expertise,
                    ActiveProjects    = _context.Projects.Count(p => p.SupervisorId == u.Id && p.Status == "Active"),
                    CompletedProjects = _context.Projects.Count(p => p.SupervisorId == u.Id && p.Status == "Completed"),
                    TotalProjects     = _context.Projects.Count(p => p.SupervisorId == u.Id)
                })
                .ToListAsync();

            return Ok(workload);
        }
    }
}
