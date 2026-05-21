using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPManagementSystem.Models
{
    public class Deliverable
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Type { get; set; } = "Milestone"; // Milestone, Final Report, Presentation

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Pending"; // Pending, In Progress, Submitted, Approved, Rejected, Completed

        public DateTime? DueDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(300)]
        public string? FilePath { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
