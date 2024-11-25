using Microsoft.Azure.Cosmos;
using TaskApplicationApi.Clients;
using TaskApplicationApi.Models;
using TaskApplicationApi.Repositories.Interfaces;

using User = TaskApplicationApi.Models.User;

namespace TaskApplicationApi.Repositories
{
    public class AzureCosmosUserPreferencesRepository : IUserPreferencesRepository
    {
        Container _userPreferencesContainer;
        Container _usersContainer;

        public AzureCosmosUserPreferencesRepository(IAzureCosmosDbClient azureCosmosDBClient)
        {
            if (azureCosmosDBClient is null)
            {
                throw new ArgumentNullException(nameof(azureCosmosDBClient));
            }

            _userPreferencesContainer = azureCosmosDBClient.GetContainer(AzureCosmosDbContainers.UsersPreferencesContainer.Name);
            _usersContainer = azureCosmosDBClient.GetContainer(AzureCosmosDbContainers.UsersContainer.Name);
        }

        public async Task<UserPreferences> Create(string userSubject, UserPreferences userPreference)
        {
            var userId = await GetUserIdByAuthentication(userSubject);
            userPreference.UserId = userSubject;
            userPreference.Id = Guid.NewGuid().ToString();
            var userPreferencesCreateResponse = await _userPreferencesContainer.CreateItemAsync(userPreference, new PartitionKey(userId));

            return userPreferencesCreateResponse.Resource;
        }

        public async Task<UserPreferences> GetById(string userSubject, string id)
        {
            var userId = await GetUserIdByAuthentication(userSubject);
            var userPreferencesReadResponse = await _userPreferencesContainer.ReadItemAsync<UserPreferences>(id, new PartitionKey(userId));

            return userPreferencesReadResponse.Resource;
        }

        public async Task<UserPreferences> Update(string userSubject, UserPreferences updateduserPreferences)
        {
            var userId = await GetUserIdByAuthentication(userSubject);
            updateduserPreferences.UserId = userId;
            var userPreferencesUpdatesResponse = await _userPreferencesContainer.UpsertItemAsync<UserPreferences>(updateduserPreferences, new PartitionKey(userId));

            return userPreferencesUpdatesResponse.Resource;
        }

        public async Task<UserPreferences> GetForUser(string userSubject)
        {
            var userId = await GetUserIdByAuthentication(userSubject);

            using FeedIterator<UserPreferences> userPreferencessByUserIdFeed = _userPreferencesContainer.GetItemQueryIterator<UserPreferences>(
                queryText: $"SELECT * FROM userPreferencess u WHERE u.UserId = {userId}", requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId) }
            );

            List<UserPreferences> userPreferencessByUserId = new List<UserPreferences>();
            while (userPreferencessByUserIdFeed.HasMoreResults)
            {
                var response = await userPreferencessByUserIdFeed.ReadNextAsync();
                userPreferencessByUserId.AddRange(response.ToList());
            }

            if (userPreferencessByUserId.Count > 1)
            {
                throw new InvalidOperationException($"Expected to find 1 user with subject {userSubject}, but found {userPreferencessByUserId.Count}");
            }

            return userPreferencessByUserId.FirstOrDefault();
        }


        //TODO: Move this to a better shared spot
        private async Task<string> GetUserIdByAuthentication(string userSubject)
        {
            using FeedIterator<User> usersByUserSubjectFeed = _usersContainer.GetItemQueryIterator<User>(
                queryText: $"SELECT * FROM Users u WHERE u.username = {userSubject}", requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userSubject) }
            );

            List<User> usersByUserSubject = new List<User>();
            while (usersByUserSubjectFeed.HasMoreResults)
            {
                var response = await usersByUserSubjectFeed.ReadNextAsync();
                usersByUserSubject.AddRange(response.ToList());
            }

            if (usersByUserSubject.Count != 1)
            {
                throw new InvalidOperationException($"Expected to find 1 user with subject {userSubject}, but found {usersByUserSubject.Count}");
            }

            return usersByUserSubject.First().Id;
        }
    }
}
