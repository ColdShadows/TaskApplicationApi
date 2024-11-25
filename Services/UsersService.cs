using TaskApplicationApi.Models;
using TaskApplicationApi.Repositories.Interfaces;
using TaskApplicationApi.Services.Interfaces;

namespace TaskApplicationApi.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public async Task<User> Create(string userId, User user)
        {
            return await _usersRepository.Create(userId, user);
        }

        public async Task<User> GetById(string id)
        {
            return await _usersRepository.GetById(id);
        }

        public async Task<User> Update(string userId, User updatedUser)
        {
            return await _usersRepository.Update(userId, updatedUser);
        }
    }
}
