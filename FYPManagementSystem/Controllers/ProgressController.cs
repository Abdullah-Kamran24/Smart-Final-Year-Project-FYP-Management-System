using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYPManagementSystem.Data;
using FYPManagementSystem.Models;
using FYPManagementSystem.Models.DTOs;

namespace FYPManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProgressController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env     = env;
        }

        // GET api/progress
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _context.Progress
                .Include(p => p.Project)
                    .ThenInclude(pr => pr!.Group)
                        .ThenInclude(g => g!.Members)
                            .ThenInclude(m => m.Student)
                .Select(p => new {
                    p.Id,
                    p.Report,
                    p.FilePath,
                    p.DateSubmitted,
                    ProjectTitle = p.Project != null ? p.Project.Title : null,
                    StudentName  = p.Project != null && p.Project.Group != null
                                     ? p.Project.Group.Members
                                         .Select(m => m.Student != null ? m.Student.Name : null)
                                         .FirstOrDefault()
                                     : null,
                    p.ProjectId
                })
                .OrderByDescending(p => p.DateSubmitted)
                .ToListAsync();

            return Ok(reports);
        }

        // GET api/progress/project/{projectId}
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var reports = await _context.Progress
                .Where(p => p.ProjectId == projectId)
                .OrderByDescending(p => p.DateSubmitted)
                .ToListAsync();

            return Ok(reports);
        }

        // POST api/progress
        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] CreateProgressDto dto)
        {
            var project = await _context.Projects.FindAsync(dto.ProjectId);
            if (project == null)
                return NotFound(new { message = "Project not found." });

            var progress = new Progress
            {
                ProjectId     = dto.ProjectId,
                Report        = dto.Report,
                DateSubmitted = DateTime.UtcNow
            };

            _context.Progress.Add(progress);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Progress report submitted!", progressId = progress.Id });
        }

        // POST api/progress/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] int projectId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file provided." });

            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                return NotFound(new { message = "Project not found." });

            var uploadsDir = Path.Combine(_env.ContentRootPath, "Uploads");
            Directory.CreateDirectory(uploadsDir);

            var safeName = Path.GetFileName(file.FileName);
            var fileName = $"{Guid.NewGuid()}_{safeName}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var progress = new Progress
            {
                ProjectId     = projectId,
                Report        = $"File uploaded: {safeName}",
                FilePath      = $"/uploads/{fileName}",
                DateSubmitted = DateTime.UtcNow
            };

            _context.Progress.Add(progress);
            await _context.SaveChangesAsync();

            return Ok(new { message = "File uploaded successfully!", filePath = progress.FilePath, progressId = progress.Id });
        }

        // PUT api/progress/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProgressDto dto)
        {
            var progress = await _context.Progress.FindAsync(id);
            if (progress == null)
                return NotFound(new { message = "Progress report not found." });

            if (string.IsNullOrWhiteSpace(dto.Report))
                return BadRequest(new { message = "Progress report cannot be empty." });

            progress.Report = dto.Report;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Progress report updated." });
        }

        // DELETE api/progress/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var progress = await _context.Progress.FindAsync(id);
            if (progress == null)
                return NotFound(new { message = "Progress report not found." });

            _context.Progress.Remove(progress);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Progress report deleted." });
        }
    }
}
