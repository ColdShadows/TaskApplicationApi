using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TaskApplicationApi.Models;
using TaskApplicationApi.Services.Interfaces;

namespace TaskApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userById = await _usersService.GetById(id);

            return base.Ok(userById);
        }

        [HttpGet()]
        public async Task<ActionResult<User>> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userPreferencesById = await _usersService.GetByUserSubject(userId);

            return base.Ok(userPreferencesById);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User userPreferences)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var createdUserPreferences = await _usersService.Create(userId, userPreferences);

            return base.CreatedAtAction(nameof(Get), new { id = createdUserPreferences.Id }, createdUserPreferences);
        }

        [HttpPut()]
        public async Task<ActionResult<User>> Put([FromBody] User userPreferences)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var updatedUserPreferences = await _usersService.Update(userId, userPreferences);

            return base.Ok(updatedUserPreferences);
        }
    }
}
