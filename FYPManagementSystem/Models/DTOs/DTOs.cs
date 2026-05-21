namespace FYPManagementSystem.Models.DTOs
{
    // ── Auth DTOs ────────────────────────────────────────────────────────────
    public class LoginDto
    {
        public string Email    { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        public string  Name      { get; set; } = string.Empty;
        public string  Email     { get; set; } = string.Empty;
        public string  Password  { get; set; } = string.Empty;
        public string  Role      { get; set; } = "Student";
        public string? Expertise { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token  { get; set; } = string.Empty;
        public string Name   { get; set; } = string.Empty;
        public string Email  { get; set; } = string.Empty;
        public string Role   { get; set; } = string.Empty;
        public int    UserId { get; set; }
    }

    // ── Group DTOs ───────────────────────────────────────────────────────────
    public class CreateGroupDto
    {
        public string   GroupName   { get; set; } = string.Empty;
        public string?  Description { get; set; }
        public List<int> StudentIds { get; set; } = new();   // 2–3 student IDs
    }

    public class GroupMemberDto
    {
        public int    StudentId   { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Email       { get; set; } = string.Empty;
    }

    public class GroupResponseDto
    {
        public int               Id             { get; set; }
        public string            GroupName      { get; set; } = string.Empty;
        public string?           Description    { get; set; }
        public List<GroupMemberDto> Members     { get; set; } = new();
        public string?           ProjectTitle   { get; set; }
        public int?              ProjectId      { get; set; }
        public string?           ProjectStatus  { get; set; }
        public string?           SupervisorName { get; set; }
        public int?              SupervisorId   { get; set; }
        public DateTime          CreatedAt      { get; set; }
    }

    public class AddMemberDto
    {
        public int StudentId { get; set; }
    }

    // ── Project DTOs ─────────────────────────────────────────────────────────
    public class CreateProjectDto
    {
        public string  Title           { get; set; } = string.Empty;
        public string? Description     { get; set; }
        public string? TechnologyStack { get; set; }
        public int     GroupId         { get; set; }
    }

    public class ProjectResponseDto
    {
        public int      Id              { get; set; }
        public string   Title           { get; set; } = string.Empty;
        public string?  Description     { get; set; }
        public string?  TechnologyStack { get; set; }
        public string   Status          { get; set; } = string.Empty;
        public string?  GroupName       { get; set; }
        public int      GroupId         { get; set; }
        public string?  SupervisorName  { get; set; }
        public int?     SupervisorId    { get; set; }
        public DateTime CreatedAt       { get; set; }
        // convenience: comma-separated member names
        public string?  MemberNames     { get; set; }
    }

    // ── Proposal DTOs ────────────────────────────────────────────────────────
    public class CreateProposalDto
    {
        public int ProjectId { get; set; }
    }

    public class UpdateProposalDto
    {
        public string  Status  { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }

    // ── Progress DTOs ────────────────────────────────────────────────────────
    public class CreateProgressDto
    {
        public int     ProjectId { get; set; }
        public string? Report    { get; set; }
    }

    public class UpdateProgressDto
    {
        public string? Report { get; set; }
    }

    // ── Evaluation DTOs ──────────────────────────────────────────────────────
    public class CreateEvaluationDto
    {
        public int     ProjectId { get; set; }
        public int     Marks     { get; set; }
        public string? Feedback  { get; set; }
    }

    // ── Deliverable DTOs ────────────────────────────────────────────────────
    public class CreateDeliverableDto
    {
        public int       ProjectId    { get; set; }
        public string    Title        { get; set; } = string.Empty;
        public string    Type         { get; set; } = "Milestone";
        public string    Status       { get; set; } = "Pending";
        public DateTime? DueDate      { get; set; }
        public string?   Description  { get; set; }
    }

    public class DeliverableResponseDto
    {
        public int       Id           { get; set; }
        public int       ProjectId    { get; set; }
        public string?   ProjectTitle { get; set; }
        public string    Title        { get; set; } = string.Empty;
        public string    Type         { get; set; } = string.Empty;
        public string    Status       { get; set; } = string.Empty;
        public DateTime? DueDate      { get; set; }
        public string?   Description  { get; set; }
        public string?   FilePath     { get; set; }
        public DateTime? SubmittedAt  { get; set; }
        public DateTime  CreatedAt    { get; set; }
    }

    // ── Dashboard DTO ────────────────────────────────────────────────────────
    public class DashboardStatsDto
    {
        public int TotalProjects     { get; set; }
        public int TotalStudents     { get; set; }
        public int TotalSupervisors  { get; set; }
        public int TotalGroups       { get; set; }
        public int ApprovedProposals { get; set; }
        public int PendingProposals  { get; set; }
        public int RejectedProposals { get; set; }
        public int CompletedProjects { get; set; }
        public int ActiveProjects    { get; set; }
    }
}
