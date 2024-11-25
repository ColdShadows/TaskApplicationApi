using TaskApplicationApi.Models;

namespace TaskApplicationApi.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<User> Create(string userId, User user);

        public Task<User> GetById(string userId);

        public Task<User> GetByUserSubject(string userSubject);

        public Task<User> Update(string userId, User updatedUser);
    }
}
