using Microsoft.Azure.Cosmos;
using TaskApplicationApi.Clients;
using TaskApplicationApi.Exceptions;
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
            var existingUser = await GetByUserSubjectOrDefault(userSubject);
            if(existingUser is not null)
            {
                throw new ResourceAlreadyExistsException(nameof(User));
            }

            user.Id = Guid.NewGuid().ToString();
            user.UserSubject = userSubject;
            var userCreateResponse = await _usersContainer.CreateItemAsync(user, new PartitionKey(user.Id));

            return userCreateResponse.Resource;
        }

        public async Task<User> GetByUserSubject(string userSubject)
        {
            using FeedIterator<User> usersByUserSubjectFeed = _usersContainer.GetItemQueryIterator<User>(
                queryText: $"SELECT * FROM Users u WHERE u.usersubject = '{userSubject}'"
            );

            List<User> usersByUserSubject = new List<User>();
            while (usersByUserSubjectFeed.HasMoreResults)
            {
                var response = await usersByUserSubjectFeed.ReadNextAsync();
                usersByUserSubject.AddRange(response.ToList());
            }

            if (usersByUserSubject.Count < 1)
            {
                throw new ResourceNotFoundException(nameof(User));
            }

            if (usersByUserSubject.Count > 1)
            {
                throw new InvalidOperationException($"Expected to find 1 user with subject {userSubject}, but found {usersByUserSubject.Count}");
            }

            return usersByUserSubject.First();
        }

        public async Task<User> Update(string userSubject, User updatedUser)
        {
            var user = await GetByUserSubject(userSubject);
            //TODO: consider having a separate input object without these Ids, and then map properties instead
            updatedUser.Id = user.Id;
            updatedUser.UserSubject = user.UserSubject;
            var userTaskUpdatesResponse = await _usersContainer.UpsertItemAsync<User>(updatedUser, new PartitionKey(user.Id));


            return userTaskUpdatesResponse.Resource;
        }

        private async Task<User> GetByUserSubjectOrDefault(string userSubject)
        {
            using FeedIterator<User> usersByUserSubjectFeed = _usersContainer.GetItemQueryIterator<User>(
                queryText: $"SELECT * FROM Users u WHERE u.usersubject = '{userSubject}'"
            );

            List<User> usersByUserSubject = new List<User>();
            while (usersByUserSubjectFeed.HasMoreResults)
            {
                var response = await usersByUserSubjectFeed.ReadNextAsync();
                usersByUserSubject.AddRange(response.ToList());
            }

            return usersByUserSubject.FirstOrDefault();
        }        
    }
}
