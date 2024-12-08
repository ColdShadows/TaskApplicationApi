using TaskApplicationApi.Models;

namespace TaskApplicationApi.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<User> Create(string userSubject, User user);

        public Task<User> GetByUserSubject(string userSubject);

        public Task<User> Update(string userSubject, User updatedUser);
    }
}
