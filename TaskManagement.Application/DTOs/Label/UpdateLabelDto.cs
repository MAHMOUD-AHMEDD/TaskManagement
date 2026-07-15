namespace TaskManagement.Application.DTOs.Label
{
    public class UpdateLabelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Color { get; set; } = null!;
    }
}
