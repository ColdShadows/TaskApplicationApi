using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApplicationApi.Models;
using TaskApplicationApi.Services.Interfaces;

namespace TaskApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserPreferencesController : ControllerBase
    {
        private readonly IUserPreferencesService _userPreferencesService;

        public UserPreferencesController(IUserPreferencesService userPreferencesService)
        {
            _userPreferencesService = userPreferencesService ?? throw new ArgumentNullException(nameof(userPreferencesService));
        }

        [HttpGet]
        public async Task<ActionResult<UserPreferences>> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userPreferencesForUser = await _userPreferencesService.GetForUser(userId);

            return base.Ok(userPreferencesForUser);
        }

        [HttpPost]
        public async Task<ActionResult<UserPreferences>> Post([FromBody] UserPreferences userPreferences)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var createdUserPreferences = await _userPreferencesService.Create(userId, userPreferences);

            return base.CreatedAtAction(nameof(Get), null, createdUserPreferences);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserPreferences>> Put(string id, [FromBody] UserPreferences userPreferences)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var updatedUserPreferences = await _userPreferencesService.Update(id, userId, userPreferences);

            return base.Ok(updatedUserPreferences);
        }
    }
}
