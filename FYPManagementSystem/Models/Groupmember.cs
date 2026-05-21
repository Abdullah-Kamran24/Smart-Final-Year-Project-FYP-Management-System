using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPManagementSystem.Models
{
    public class GroupMember
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public Group? Group { get; set; }

        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public User? Student { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}