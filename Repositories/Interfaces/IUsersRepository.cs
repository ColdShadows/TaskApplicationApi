using TaskApplicationApi.Models;

namespace TaskApplicationApi.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<User> Create(string userId, User user);

        public Task<User> GetById(string userId);

        public Task<User> GetByUserSubject(string userSubject);

        public Task<User> Update(string userId, User updatedUser);
    }
}
