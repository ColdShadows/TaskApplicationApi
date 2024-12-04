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
    public class UserTasksController : ControllerBase
    {
        private readonly IUserTasksService _userTaskService;

        public UserTasksController(IUserTasksService userTaskService)
        {
            _userTaskService = userTaskService ?? throw new ArgumentNullException(nameof(userTaskService));
        }

        [HttpGet]
        public async Task<ActionResult<IList<UserTask>>> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userTasksForUser = await _userTaskService.GetListForUser(userId);

            return base.Ok(userTasksForUser);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserTask>> Get(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userTaskById = await _userTaskService.GetById(userId, id);

            return base.Ok(userTaskById);
        }

        [HttpPost]
        public async Task<ActionResult<UserTask>> Post([FromBody] UserTask userTask)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var createdUserTask = await _userTaskService.Create(userId, userTask);

            return base.CreatedAtAction(nameof(Get), new { id = createdUserTask.Id }, createdUserTask);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserTask>> Put(string id, [FromBody] UserTask userTask)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var updatedUserTask = await _userTaskService.Update(id, userId, userTask);

            return base.Ok(updatedUserTask);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _userTaskService.Delete(userId, id);
        }
    }
}
