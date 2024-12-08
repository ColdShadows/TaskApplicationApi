using TaskApplicationApi.Models;

namespace TaskApplicationApi.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<User> Create(string userSubject, User user);

        public Task<User> GetByUserSubject(string userSubject);

        public Task<User> Update(string userSubject, User updatedUser);
    }
}
