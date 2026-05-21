using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPManagementSystem.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(200)]
        public string? TechnologyStack { get; set; }

        // Group-based (replaces StudentId)
        public int GroupId { get; set; }

        public int? SupervisorId { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Active, Completed, Rejected

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("GroupId")]
        public Group? Group { get; set; }

        [ForeignKey("SupervisorId")]
        public User? Supervisor { get; set; }

        public ICollection<Proposal>?   Proposals { get; set; }
        public ICollection<Progress>?   ProgressReports { get; set; }
        public ICollection<Evaluation>? Evaluations { get; set; }
        public ICollection<Deliverable>? Deliverables { get; set; }
    }
}
