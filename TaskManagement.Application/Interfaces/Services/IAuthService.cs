namespace TaskManagement.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterUserAsync(string email, string username, string password);
        Task LoginUserAsync(string username, string password);
    }
}
