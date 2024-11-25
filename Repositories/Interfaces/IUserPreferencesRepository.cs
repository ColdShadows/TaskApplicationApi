using TaskApplicationApi.Models;

namespace TaskApplicationApi.Repositories.Interfaces
{
    public interface IUserPreferencesRepository
    {
        public Task<UserPreferences> Create(string userSubject, UserPreferences UserPreferences);

        public Task<UserPreferences> GetById(string userSubject, string id);

        public Task<UserPreferences> GetForUser(string userSubject);

        public Task<UserPreferences> Update(string userSubject, UserPreferences UserPreferences);
    }
}