using TaskApplicationApi.Models;
using TaskApplicationApi.Repositories.Interfaces;
using TaskApplicationApi.Services.Interfaces;

namespace TaskApplicationApi.Services
{
    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly IUserPreferencesRepository _userPreferencesRepository;

        public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository)
        {
            _userPreferencesRepository = userPreferencesRepository ?? throw new ArgumentNullException(nameof(userPreferencesRepository));
        }

        public async Task<UserPreferences> Create(string userSubject, UserPreferences UserPreferences)
        {
            return await _userPreferencesRepository.Create(userSubject, UserPreferences);
        }

        public async Task<UserPreferences> GetById(string userSubject, string id)
        {
            return await _userPreferencesRepository.GetById(userSubject, id);
        }

        public async Task<UserPreferences> GetForUser(string userSubject)
        {
            return await _userPreferencesRepository.GetForUser(userSubject);
        }

        public async Task<UserPreferences> Update(string userSubject, UserPreferences UserPreferences)
        {
            return await _userPreferencesRepository.Update(userSubject, UserPreferences);
        }
    }
}
