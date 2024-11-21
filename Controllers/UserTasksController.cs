using Microsoft.AspNetCore.Mvc;
using TaskApplicationApi.Models;
using TaskApplicationApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<ActionResult<UserTask>> Post([FromBody] UserTask userTask)
        {
            var createdUserTask = await _userTaskService.Create(userTask);

            return base.CreatedAtAction(nameof(Get), new { id = createdUserTask.Id }, createdUserTask);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
