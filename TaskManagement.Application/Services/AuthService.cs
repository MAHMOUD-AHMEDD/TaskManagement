using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.Settings;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(UserManager<User> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async System.Threading.Tasks.Task RegisterUserAsync(string fullName, string email, string username, string password)
        {
            var user = new User
            {
                FullName = fullName,
                Email = email,
                UserName = username
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Registration failed: {errors}");
            }
        }

        public async System.Threading.Tasks.Task<string> LoginUserAsync(string usernameOrEmail, string password)
        {
            var user = await _userManager.FindByNameAsync(usernameOrEmail) ?? await _userManager.FindByEmailAsync(usernameOrEmail);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new Exception("Invalid username/email or password.");
            }
            return GenerateJwtToken(user);
        }

        public string GenerateJwtToken(User user)
        {

            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
