using System.ComponentModel.DataAnnotations;

namespace FYPManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Student"; // Student, Supervisor, Admin

        [MaxLength(200)]
        public string? Expertise { get; set; } // For supervisors

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Supervisor relationship is configured from Project.Supervisor.
    }
}
