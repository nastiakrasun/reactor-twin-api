using System.Security.Claims;

namespace ReactorTwinAPI.Application.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        bool IsSuperUser { get; }
        bool CanCreateReactor { get; }
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var sub = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(sub, out var id)) return id;
                return null;
            }
        }

        public bool IsSuperUser => _httpContextAccessor.HttpContext?.User?.FindFirst("isSuper")?.Value == "true";
        public bool CanCreateReactor => _httpContextAccessor.HttpContext?.User?.FindFirst("canCreate")?.Value == "true";
    }
}
