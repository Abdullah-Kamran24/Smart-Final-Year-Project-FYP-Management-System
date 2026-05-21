using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYPManagementSystem.Data;
using FYPManagementSystem.Models;
using FYPManagementSystem.Models.DTOs;

namespace FYPManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliverableController : ControllerBase
    {
        private static readonly string[] AllowedTypes = { "Milestone", "Final Report", "Presentation" };
        private static readonly string[] AllowedStatuses = { "Pending", "In Progress", "Submitted", "Approved", "Rejected", "Completed" };

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DeliverableController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET api/deliverable
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deliverables = await _context.Deliverables
                .Include(d => d.Project)
                .OrderBy(d => d.DueDate ?? DateTime.MaxValue)
                .ToListAsync();

            return Ok(deliverables.Select(ToResponse));
        }

        // GET api/deliverable/project/{projectId}
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var deliverables = await _context.Deliverables
                .Include(d => d.Project)
                .Where(d => d.ProjectId == projectId)
                .OrderBy(d => d.DueDate ?? DateTime.MaxValue)
                .ToListAsync();

            return Ok(deliverables.Select(ToResponse));
        }

        // POST api/deliverable
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeliverableDto dto)
        {
            var validation = await ValidateDto(dto);
            if (validation != null) return validation;

            var deliverable = new Deliverable
            {
                ProjectId = dto.ProjectId,
                Title = dto.Title.Trim(),
                Type = dto.Type,
                Status = dto.Status,
                DueDate = dto.DueDate,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                SubmittedAt = dto.Status == "Submitted" ? DateTime.UtcNow : null
            };

            _context.Deliverables.Add(deliverable);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deliverable created successfully!", deliverableId = deliverable.Id });
        }

        // PUT api/deliverable/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateDeliverableDto dto)
        {
            var deliverable = await _context.Deliverables.FindAsync(id);
            if (deliverable == null)
                return NotFound(new { message = "Deliverable not found." });

            var validation = await ValidateDto(dto);
            if (validation != null) return validation;

            deliverable.ProjectId = dto.ProjectId;
            deliverable.Title = dto.Title.Trim();
            deliverable.Type = dto.Type;
            deliverable.Status = dto.Status;
            deliverable.DueDate = dto.DueDate;
            deliverable.Description = dto.Description;
            if (dto.Status == "Submitted" && deliverable.SubmittedAt == null)
                deliverable.SubmittedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Deliverable updated successfully!" });
        }

        // POST api/deliverable/{id}/upload
        [HttpPost("{id}/upload")]
        public async Task<IActionResult> Upload(int id, [FromForm] IFormFile file)
        {
            var deliverable = await _context.Deliverables.FindAsync(id);
            if (deliverable == null)
                return NotFound(new { message = "Deliverable not found." });

            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file provided." });

            var uploadsDir = Path.Combine(_env.ContentRootPath, "Uploads");
            Directory.CreateDirectory(uploadsDir);

            var safeName = Path.GetFileName(file.FileName);
            var fileName = $"{Guid.NewGuid()}_{safeName}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            deliverable.FilePath = $"/uploads/{fileName}";
            deliverable.Status = "Submitted";
            deliverable.SubmittedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Deliverable file uploaded successfully!", filePath = deliverable.FilePath });
        }

        // DELETE api/deliverable/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deliverable = await _context.Deliverables.FindAsync(id);
            if (deliverable == null)
                return NotFound(new { message = "Deliverable not found." });

            _context.Deliverables.Remove(deliverable);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deliverable deleted." });
        }

        private async Task<IActionResult?> ValidateDto(CreateDeliverableDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest(new { message = "Title is required." });

            if (!AllowedTypes.Contains(dto.Type))
                return BadRequest(new { message = "Invalid deliverable type." });

            if (!AllowedStatuses.Contains(dto.Status))
                return BadRequest(new { message = "Invalid deliverable status." });

            var projectExists = await _context.Projects.AnyAsync(p => p.Id == dto.ProjectId);
            if (!projectExists)
                return BadRequest(new { message = "Invalid project ID." });

            return null;
        }

        private static DeliverableResponseDto ToResponse(Deliverable d)
        {
            return new DeliverableResponseDto
            {
                Id = d.Id,
                ProjectId = d.ProjectId,
                ProjectTitle = d.Project != null ? d.Project.Title : null,
                Title = d.Title,
                Type = d.Type,
                Status = d.Status,
                DueDate = d.DueDate,
                Description = d.Description,
                FilePath = d.FilePath,
                SubmittedAt = d.SubmittedAt,
                CreatedAt = d.CreatedAt
            };
        }
    }
}
