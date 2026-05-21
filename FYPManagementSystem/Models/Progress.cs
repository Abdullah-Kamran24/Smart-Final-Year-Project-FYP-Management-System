using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPManagementSystem.Models
{
    public class Progress
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string? Report { get; set; }

        [MaxLength(300)]
        public string? FilePath { get; set; }

        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
