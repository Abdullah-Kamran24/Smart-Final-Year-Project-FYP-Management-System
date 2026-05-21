using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPManagementSystem.Models
{
    public class Evaluation
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [Range(0, 100)]
        public int Marks { get; set; }

        public string? Feedback { get; set; }

        public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
