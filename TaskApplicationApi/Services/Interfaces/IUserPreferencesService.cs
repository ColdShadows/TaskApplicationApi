using TaskApplicationApi.Models;

namespace TaskApplicationApi.Services.Interfaces
{
    public interface IUserPreferencesService
    {
        public Task<UserPreferences> Create(string userSubject, UserPreferences UserPreferences);

        public Task<UserPreferences> GetForUser(string userSubject);

        public Task<UserPreferences> Update(string id, string userSubject, UserPreferences UserPreferences);
    }
}
