namespace TaskManagement.Application.DTOs.Label
{
    public class LabelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int ProjectId { get; set; }
    }
}
