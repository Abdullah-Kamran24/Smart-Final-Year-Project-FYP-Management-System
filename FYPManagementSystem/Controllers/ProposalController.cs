using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYPManagementSystem.Data;
using FYPManagementSystem.Models;
using FYPManagementSystem.Models.DTOs;

namespace FYPManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProposalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProposalController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/proposal
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var proposals = await _context.Proposals
                .Include(p => p.Project)
                    .ThenInclude(pr => pr!.Group)
                        .ThenInclude(g => g!.Members)
                            .ThenInclude(m => m.Student)
                .Select(p => new {
                    p.Id,
                    p.Status,
                    p.Remarks,
                    p.SubmittedAt,
                    ProjectTitle = p.Project != null ? p.Project.Title : null,
                    StudentName  = p.Project != null && p.Project.Group != null
                                     ? p.Project.Group.Members
                                         .Select(m => m.Student != null ? m.Student.Name : null)
                                         .FirstOrDefault()
                                     : null,
                    p.ProjectId
                })
                .ToListAsync();

            return Ok(proposals);
        }

        // GET api/proposal/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var proposal = await _context.Proposals
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proposal == null)
                return NotFound(new { message = "Proposal not found." });

            return Ok(proposal);
        }

        // POST api/proposal
        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] CreateProposalDto dto)
        {
            var project = await _context.Projects.FindAsync(dto.ProjectId);
            if (project == null)
                return NotFound(new { message = "Project not found." });

            var existing = await _context.Proposals.AnyAsync(p => p.ProjectId == dto.ProjectId);
            if (existing)
                return BadRequest(new { message = "A proposal already exists for this project." });

            var proposal = new Proposal
            {
                ProjectId   = dto.ProjectId,
                Status      = "Pending",
                SubmittedAt = DateTime.UtcNow
            };

            _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Proposal submitted successfully!", proposalId = proposal.Id });
        }

        // PUT api/proposal/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateProposalDto dto)
        {
            if (dto.Status != "Approved" && dto.Status != "Rejected" && dto.Status != "Pending")
                return BadRequest(new { message = "Status must be Pending, Approved, or Rejected." });

            var proposal = await _context.Proposals
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proposal == null)
                return NotFound(new { message = "Proposal not found." });

            proposal.Status  = dto.Status;
            proposal.Remarks = dto.Remarks;

            if (proposal.Project != null)
            {
                if (dto.Status == "Approved")
                    proposal.Project.Status = "Active";
                else if (dto.Status == "Rejected")
                    proposal.Project.Status = "Rejected";
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Proposal {dto.Status} successfully!" });
        }

        // DELETE api/proposal/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal == null)
                return NotFound(new { message = "Proposal not found." });

            _context.Proposals.Remove(proposal);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Proposal deleted." });
        }
    }
}
