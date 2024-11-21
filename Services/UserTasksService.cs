using TaskApplicationApi.Models;

namespace TaskApplicationApi.Services
{
    public class UserTasksService : IUserTasksService
    {
        public async Task<UserTask> Create(UserTask userTask)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserTask> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<UserTask>> GetListForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserTask> Update(string id, UserTask updatedUserTask)
        {
            throw new NotImplementedException();
        }
    }
}
