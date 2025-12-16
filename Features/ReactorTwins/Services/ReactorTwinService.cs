using ReactorTwinAPI.Features.ReactorTwins.Dtos;
using ReactorTwinAPI.Features.ReactorTwins.Repositories;

namespace ReactorTwinAPI.Features.ReactorTwins.Services
{
    public class ReactorTwinService : IReactorTwinService
    {
        private readonly IReactorTwinRepository _repository;
        private readonly ReactorTwinAPI.Application.Services.ICurrentUserService _currentUser;

        public ReactorTwinService(IReactorTwinRepository repository, ReactorTwinAPI.Application.Services.ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<ReactorTwinDto> CreateAsync(CreateReactorTwinDto dto)
        {
            var userId = _currentUser.UserId;
            if (userId == null) throw new UnauthorizedAccessException();
            if (!(_currentUser.IsSuperUser || _currentUser.CanCreateReactor))
                throw new UnauthorizedAccessException("User is not allowed to create reactors");

            return await _repository.CreateAsync(dto, userId.Value);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var userId = _currentUser.UserId;
            if (userId == null) throw new UnauthorizedAccessException();

            var reactor = await _repository.GetByIdAsync(id);
            if (reactor == null) return false;

            if (_currentUser.IsSuperUser || reactor.OwnerId == userId.Value)
            {
                return await _repository.DeleteAsync(id);
            }

            throw new UnauthorizedAccessException("Not allowed to delete this reactor");
        }

        public async Task<IEnumerable<ReactorTwinDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ReactorTwinDto?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateReactorTwinDto dto)
        {
            var userId = _currentUser.UserId;
            if (userId == null) throw new UnauthorizedAccessException();

            var reactor = await _repository.GetByIdAsync(id);
            if (reactor == null) return false;

            if (_currentUser.IsSuperUser || reactor.OwnerId == userId.Value)
            {
                return await _repository.UpdateAsync(id, dto);
            }

            throw new UnauthorizedAccessException("Not allowed to update this reactor");
        }
    }
}
