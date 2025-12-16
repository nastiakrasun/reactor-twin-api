using ReactorTwinAPI.Features.ReactorTwins.Dtos;

namespace ReactorTwinAPI.Features.ReactorTwins.Services
{
    public interface IReactorTwinService
    {
        Task<ReactorTwinDto> CreateAsync(CreateReactorTwinDto dto);
        Task<ReactorTwinDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReactorTwinDto>> GetAllAsync();
        Task<bool> UpdateAsync(Guid id, UpdateReactorTwinDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
