using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactorTwinAPI.Application.Services;
using ReactorTwinAPI.Features.Admin.Dtos;
using ReactorTwinAPI.Features.Users.Repositories;

namespace ReactorTwinAPI.Features.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ICurrentUserService _currentUser;

        public AdminController(IUserRepository userRepo, ICurrentUserService currentUser)
        {
            _userRepo = userRepo;
            _currentUser = currentUser;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!_currentUser.IsSuperUser) return Forbid();

            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPatch("{id}/permissions")]
        [Authorize]
        public async Task<IActionResult> UpdatePermissions(Guid id, [FromBody] UpdatePermissionsDto req)
        {
            if (!_currentUser.IsSuperUser) return Forbid();

            var success = await _userRepo.UpdateAsync(id, user =>
            {
                if (req.CanCreate.HasValue) user.CanCreateReactor = req.CanCreate.Value;
                if (req.IsSuper.HasValue) user.IsSuperUser = req.IsSuper.Value;
            });

            if (!success) return NotFound();
            return NoContent();
        }
    }
}
