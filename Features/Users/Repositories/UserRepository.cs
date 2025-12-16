using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReactorTwinAPI.Features.Users.Dtos;
using ReactorTwinAPI.Infrastructure.Persistence;

namespace ReactorTwinAPI.Features.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var u = await _db.Set<Domain.Entities.User>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return u == null ? null : _mapper.Map<UserDto>(u);
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var u = await _db.Set<Domain.Entities.User>().AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);
            return u == null ? null : _mapper.Map<UserDto>(u);
        }

        public async Task<Domain.Entities.User?> GetEntityByUsernameAsync(string username)
        {
            return await _db.Set<Domain.Entities.User>().FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<bool> AnyUsersAsync()
        {
            return await _db.Set<Domain.Entities.User>().AnyAsync();
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto, string passwordHash)
        {
            var user = new Domain.Entities.User
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                IsSuperUser = dto.IsSuperUser,
                CanCreateReactor = dto.CanCreateReactor
            };

            _db.Set<Domain.Entities.User>().Add(user);
            await _db.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateAsync(Guid id, Action<Domain.Entities.User> applyChanges)
        {
            var user = await _db.Set<Domain.Entities.User>().FindAsync(id);
            if (user == null) return false;
            applyChanges(user);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
