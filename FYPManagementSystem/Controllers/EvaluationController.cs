using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYPManagementSystem.Data;
using FYPManagementSystem.Models;
using FYPManagementSystem.Models.DTOs;

namespace FYPManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EvaluationController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/evaluation
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var evals = await _context.Evaluations
                .Include(e => e.Project)
                    .ThenInclude(p => p!.Group)
                        .ThenInclude(g => g!.Members)
                            .ThenInclude(m => m.Student)
                .Select(e => new {
                    e.Id,
                    e.Marks,
                    e.Feedback,
                    e.EvaluatedAt,
                    ProjectTitle = e.Project != null ? e.Project.Title : null,
                    StudentName  = e.Project != null && e.Project.Group != null
                                     ? e.Project.Group.Members
                                         .Select(m => m.Student != null ? m.Student.Name : null)
                                         .FirstOrDefault()
                                     : null,
                    e.ProjectId,
                    Grade = e.Marks >= 90 ? "A+" :
                            e.Marks >= 80 ? "A"  :
                            e.Marks >= 70 ? "B"  :
                            e.Marks >= 60 ? "C"  :
                            e.Marks >= 50 ? "D"  : "F"
                })
                .ToListAsync();

            return Ok(evals);
        }

        // GET api/evaluation/project/{projectId}
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var evals = await _context.Evaluations
                .Where(e => e.ProjectId == projectId)
                .ToListAsync();

            return Ok(evals);
        }

        // POST api/evaluation
        [HttpPost]
        public async Task<IActionResult> Evaluate([FromBody] CreateEvaluationDto dto)
        {
            if (dto.Marks < 0 || dto.Marks > 100)
                return BadRequest(new { message = "Marks must be between 0 and 100." });

            var project = await _context.Projects.FindAsync(dto.ProjectId);
            if (project == null)
                return NotFound(new { message = "Project not found." });

            var evaluation = new Evaluation
            {
                ProjectId   = dto.ProjectId,
                Marks       = dto.Marks,
                Feedback    = dto.Feedback,
                EvaluatedAt = DateTime.UtcNow
            };

            _context.Evaluations.Add(evaluation);
            project.Status = "Completed";
            await _context.SaveChangesAsync();

            var grade = dto.Marks >= 90 ? "A+" :
                        dto.Marks >= 80 ? "A"  :
                        dto.Marks >= 70 ? "B"  :
                        dto.Marks >= 60 ? "C"  :
                        dto.Marks >= 50 ? "D"  : "F";

            return Ok(new { message = "Evaluation submitted!", evaluationId = evaluation.Id, grade, marks = dto.Marks });
        }

        // PUT api/evaluation/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateEvaluationDto dto)
        {
            if (dto.Marks < 0 || dto.Marks > 100)
                return BadRequest(new { message = "Marks must be between 0 and 100." });

            var eval = await _context.Evaluations.FindAsync(id);
            if (eval == null)
                return NotFound(new { message = "Evaluation not found." });

            eval.Marks    = dto.Marks;
            eval.Feedback = dto.Feedback;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Evaluation updated!" });
        }

        // DELETE api/evaluation/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eval = await _context.Evaluations.FindAsync(id);
            if (eval == null)
                return NotFound(new { message = "Evaluation not found." });

            _context.Evaluations.Remove(eval);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Evaluation deleted." });
        }
    }
}
