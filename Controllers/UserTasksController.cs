using Microsoft.AspNetCore.Mvc;
using TaskApplicationApi.Models;
using TaskApplicationApi.Services;

namespace TaskApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            //TODO: Retrieve and use user id from token
            var userTasksForUser = await _userTaskService.GetListForUser("userId");

            return base.Ok(userTasksForUser);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserTask>> Get(string id)
        {
            //TODO: Retrieve and use user id from token
            //TODO: Verify resource belongs to user
            var userTaskById = await _userTaskService.GetById("userId", id);

            return base.Ok(userTaskById);
        }

        [HttpPost]
        public async Task<ActionResult<UserTask>> Post([FromBody] UserTask userTask)
        {
            //TODO: Retrieve and use user id from token
            var createdUserTask = await _userTaskService.Create("userId", userTask);

            return base.CreatedAtAction(nameof(Get), new { id = createdUserTask.Id }, createdUserTask);
        }

        [HttpPut()]
        public async Task<ActionResult<UserTask>> Put([FromBody] UserTask userTask)
        {
            //TODO: Retrieve and use user id from token
            //TODO: Verify resource belongs to user
            var userTasksForUser = await _userTaskService.Update("userId", userTask);

            return base.Ok(userTasksForUser);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            //TODO: Retrieve and use user id from token
            //TODO: Verify resource belongs to user
            await _userTaskService.Delete("userId", id);
        }
    }
}
