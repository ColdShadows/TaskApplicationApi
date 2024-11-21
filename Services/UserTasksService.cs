using TaskApplicationApi.Models;
using TaskApplicationApi.Repositories;

namespace TaskApplicationApi.Services
{
    public class UserTasksService : IUserTasksService
    {
        private readonly IUserTasksRepository _userTasksRepository;

        public UserTasksService(IUserTasksRepository userTasksRepository) 
        {
            _userTasksRepository = userTasksRepository ?? throw new ArgumentNullException(nameof(userTasksRepository));
        }

        public async Task<UserTask> Create(UserTask userTask)
        {
            return await _userTasksRepository.Create(userTask);
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
