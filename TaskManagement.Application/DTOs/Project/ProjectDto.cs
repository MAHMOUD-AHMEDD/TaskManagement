namespace TaskManagement.Application.DTOs.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string OwnerId { get; set; } = null!;
        public string OwnerName { get; set; } = null!;
        public int TaskCount { get; set; }
        public int MemberCount { get; set; }
    }
}
