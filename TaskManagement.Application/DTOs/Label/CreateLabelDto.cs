namespace TaskManagement.Application.DTOs.Label
{
    public class CreateLabelDto
    {
        public string Name { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int ProjectId { get; set; }
    }
}
