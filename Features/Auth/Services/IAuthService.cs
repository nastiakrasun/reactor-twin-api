using ReactorTwinAPI.Features.Auth.Dtos;
using ReactorTwinAPI.Features.Users.Dtos;

namespace ReactorTwinAPI.Features.Auth.Services
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterDto req);
        Task<string?> LoginAsync(LoginDto req);
    }
}
