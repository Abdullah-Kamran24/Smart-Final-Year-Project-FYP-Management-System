using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPManagementSystem.Models
{
    public class Proposal
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public string? Remarks { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
