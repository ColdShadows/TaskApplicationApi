using TaskApplicationApi.Models;
using TaskApplicationApi.Repositories.Interfaces;
using TaskApplicationApi.Services.Interfaces;

namespace TaskApplicationApi.Services
{
    public class UserTasksService : IUserTasksService
    {
        private readonly IUserTasksRepository _userTasksRepository;

        public UserTasksService(IUserTasksRepository userTasksRepository) 
        {
            _userTasksRepository = userTasksRepository ?? throw new ArgumentNullException(nameof(userTasksRepository));
        }

        public async Task<UserTask> Create(string userId, UserTask userTask)
        {
            return await _userTasksRepository.Create(userId, userTask);
        }

        public async Task Delete(string userId, string id)
        {
            await _userTasksRepository.Delete(userId, id);
        }

        public async Task<UserTask> GetById(string userId, string id)
        {
            return await _userTasksRepository.GetById(userId, id);
        }

        public async Task<IList<UserTask>> GetListForUser(string userId)
        {
            return await _userTasksRepository.GetListForUser(userId);
        }

        public async Task<UserTask> Update(string userTaskId, string userId, UserTask updatedUserTask)
        {
            return await _userTasksRepository.Update(userTaskId, userId, updatedUserTask);
        }
    }
}
