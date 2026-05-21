using System.ComponentModel.DataAnnotations;

namespace FYPManagementSystem.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<GroupMember>? Members { get; set; }
        public ICollection<Project>?     Projects { get; set; }
    }
}