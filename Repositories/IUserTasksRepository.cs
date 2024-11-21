using TaskApplicationApi.Models;

namespace TaskApplicationApi.Repositories
{
    public interface IUserTasksRepository
    {
        public Task<UserTask> Create(UserTask userTask);

        public Task<IList<UserTask>> GetListForUser(string userId);

        public Task<UserTask> GetById(string id);

        public Task<UserTask> Update(string id, UserTask updatedUserTask);

        public Task Delete(string id);
    }
}
