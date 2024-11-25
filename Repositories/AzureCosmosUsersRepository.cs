using Microsoft.Azure.Cosmos;
using TaskApplicationApi.Clients;
using TaskApplicationApi.Repositories.Interfaces;

using User = TaskApplicationApi.Models.User;

namespace TaskApplicationApi.Repositories
{
    public class AzureCosmosUsersRepository : IUsersRepository
    {
        Container _usersContainer;

        public AzureCosmosUsersRepository(IAzureCosmosDbClient azureCosmosDBClient)
        {
            if (azureCosmosDBClient is null)
            {
                throw new ArgumentNullException(nameof(azureCosmosDBClient));
            }

            _usersContainer = azureCosmosDBClient.GetContainer(AzureCosmosDbContainers.UsersContainer.Name);
        }

        public async Task<User> Create(string userSubject, User user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.UserSubject = userSubject;
            var userCreateResponse = await _usersContainer.CreateItemAsync(user, new PartitionKey(user.Id));

            return userCreateResponse.Resource;
        }

        public async Task<User> GetById(string userId)
        {
            var userTaskReadResponse = await _usersContainer.ReadItemAsync<User>(userId, new PartitionKey(userId));

            return userTaskReadResponse.Resource;
        }

        public async Task<User> GetByUserSubject(string userSubject)
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

            return usersByUserSubject.First();
        }

        public async Task<User> Update(string userId, User updatedUser)
        {
            var userTaskUpdatesResponse = await _usersContainer.UpsertItemAsync<User>(updatedUser, new PartitionKey(userId));

            return userTaskUpdatesResponse.Resource;
        }
    }
}
