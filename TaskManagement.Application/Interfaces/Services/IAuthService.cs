namespace TaskManagement.Application.Interfaces.Services;

public interface IAuthService
{
    Task RegisterUserAsync(string fullName, string email, string username, string password);
    Task<string> LoginUserAsync(string usernameOrEmail, string password);
}