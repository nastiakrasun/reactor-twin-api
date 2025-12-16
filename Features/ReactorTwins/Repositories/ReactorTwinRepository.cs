using Microsoft.EntityFrameworkCore;
using ReactorTwinAPI.Domain.Entities;
using ReactorTwinAPI.Features.ReactorTwins.Dtos;
using ReactorTwinAPI.Infrastructure.Persistence;

namespace ReactorTwinAPI.Features.ReactorTwins.Repositories
{
    public class ReactorTwinRepository : IReactorTwinRepository
    {
        private readonly AppDbContext _db;
        private readonly AutoMapper.IMapper _mapper;

        public ReactorTwinRepository(AppDbContext db, AutoMapper.IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ReactorTwinDto> CreateAsync(CreateReactorTwinDto dto, Guid ownerId)
        {
            var reactor = _mapper.Map<ReactorTwin>(dto);
            reactor.CreatedAt = DateTime.UtcNow;
            reactor.OwnerId = ownerId;

            _db.ReactorTwins.Add(reactor);
            await _db.SaveChangesAsync();

            return _mapper.Map<ReactorTwinDto>(reactor);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var e = await _db.ReactorTwins.FindAsync(id);
            if (e == null) return false;

            _db.ReactorTwins.Remove(e);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ReactorTwinDto>> GetAllAsync()
        {
            var list = await _db.ReactorTwins.ToListAsync();
            return list.Select(MapToDto);
        }

        public async Task<ReactorTwinDto?> GetByIdAsync(Guid id)
        {
            var e = await _db.ReactorTwins.FirstOrDefaultAsync(x => x.Id == id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateReactorTwinDto dto)
        {
            var reactor = await _db.ReactorTwins.FindAsync(id);
            if (reactor == null) return false;

            _mapper.Map(dto, reactor);

            reactor.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return true;
        }


        private ReactorTwinDto MapToDto(ReactorTwin reactor)
        {
            return _mapper.Map<ReactorTwinDto>(reactor);
        }
    }
}
