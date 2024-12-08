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
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        [HttpGet()]
        public async Task<ActionResult<User>> Get()
        {
            var userSubject = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userPreferencesById = await _usersService.GetByUserSubject(userSubject);

            return base.Ok(userPreferencesById);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            var userSubject = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var createdUserPreferences = await _usersService.Create(userSubject, user);

            return base.CreatedAtAction(nameof(Get), null, createdUserPreferences);
        }

        [HttpPut()]
        public async Task<ActionResult<User>> Put([FromBody] User user)
        {
            var userSubject = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var updatedUserPreferences = await _usersService.Update(userSubject, user);

            return base.Ok(updatedUserPreferences);
        }
    }
}
