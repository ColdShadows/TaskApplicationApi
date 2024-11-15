using TaskApplicationApi.Models;

namespace TaskApplicationApi.Services
{
    public interface IUserTasksService
    {
        public UserTask Create(UserTask userTask);

        public IList<UserTask> GetListForUser(string userId);

        public UserTask GetById(string id);

        public UserTask Update(string id, UserTask updatedUserTask);

        public void Delete(string id);
    }
}
