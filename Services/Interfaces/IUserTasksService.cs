using TaskApplicationApi.Models;

namespace TaskApplicationApi.Services.Interfaces
{
    public interface IUserTasksService
    {
        public Task<UserTask> Create(string userId, UserTask userTask);

        public Task<IList<UserTask>> GetListForUser(string userId);

        public Task<UserTask> GetById(string userId, string id);

        public Task<UserTask> Update(string userTaskId, string userId, UserTask updatedUserTask);

        public Task Delete(string userId, string id);
    }
}
