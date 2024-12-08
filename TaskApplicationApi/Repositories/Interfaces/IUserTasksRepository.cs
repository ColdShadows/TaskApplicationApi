using TaskApplicationApi.Models;

namespace TaskApplicationApi.Repositories.Interfaces
{
    public interface IUserTasksRepository
    {
        public Task<UserTask> Create(string userSubject, UserTask userTask);

        public Task<IList<UserTask>> GetListForUser(string userSubject);

        public Task<UserTask> GetById(string userSubject, string id);

        public Task<UserTask> Update(string userTaskId, string userSubject, UserTask updatedUserTask);

        public Task Delete(string userSubject, string id);
    }
}
