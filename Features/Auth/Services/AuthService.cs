using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ReactorTwinAPI.Features.Auth.Dtos;
using ReactorTwinAPI.Features.Users.Dtos;
using ReactorTwinAPI.Features.Users.Repositories;

namespace ReactorTwinAPI.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto req)
        {
            var any = await _userRepo.AnyUsersAsync();

            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                throw new ArgumentException("Username and password are required");

            var exists = await _userRepo.GetByUsernameAsync(req.Username);
            if (exists != null) throw new InvalidOperationException("Username already exists");

            var dto = new CreateUserDto { Username = req.Username };
            if (!any && req.RequestSuper)
            {
                dto.IsSuperUser = true;
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(req.Password);
            var created = await _userRepo.CreateAsync(dto, hash);
            return created;
        }

        public async Task<string?> LoginAsync(LoginDto req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                throw new ArgumentException("Username and password are required");

            var userEntity = await _userRepo.GetEntityByUsernameAsync(req.Username);
            if (userEntity == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(req.Password, userEntity.PasswordHash)) return null;

            return GenerateToken(userEntity.Id, userEntity.Username, userEntity.IsSuperUser, userEntity.CanCreateReactor);
        }

        private string GenerateToken(Guid userId, string username, bool isSuper, bool canCreate)
        {
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("Jwt:Key is not configured");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim("isSuper", isSuper ? "true" : "false"),
                new Claim("canCreate", canCreate ? "true" : "false")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
