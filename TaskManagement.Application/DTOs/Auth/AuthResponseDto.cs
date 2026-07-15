namespace TaskManagement.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Id { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
