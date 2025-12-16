using ReactorTwinAPI.Features.Users.Dtos;

namespace ReactorTwinAPI.Features.Users.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<UserDto?> GetByUsernameAsync(string username);
        Task<Domain.Entities.User?> GetEntityByUsernameAsync(string username);
        Task<bool> AnyUsersAsync();
        Task<UserDto> CreateAsync(CreateUserDto dto, string passwordHash);
        Task<bool> UpdateAsync(Guid id, Action<Domain.Entities.User> applyChanges);
    }
}
