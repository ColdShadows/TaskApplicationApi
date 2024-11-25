using TaskApplicationApi.Models;
using Microsoft.Azure.Cosmos;
using TaskApplicationApi.Clients;
using TaskApplicationApi.Repositories.Interfaces;

using User = TaskApplicationApi.Models.User;

namespace TaskApplicationApi.Repositories
{
    public class AzureCosmosUserTasksRepository : IUserTasksRepository
    {
        Container _userTasksContainer;
        Container _usersContainer;

        public AzureCosmosUserTasksRepository(IAzureCosmosDbClient azureCosmosDBClient) 
        {
            if(azureCosmosDBClient is null)
            {
                throw new ArgumentNullException(nameof(azureCosmosDBClient));
            }

            _userTasksContainer = azureCosmosDBClient.GetContainer(AzureCosmosDbContainers.UserTasksContainer.Name);
            _usersContainer = azureCosmosDBClient.GetContainer(AzureCosmosDbContainers.UsersContainer.Name);
        }

        public async Task<UserTask> Create(string userSubject, UserTask userTask)
        {
            var userId = await GetUserIdByAuthentication(userSubject);
            userTask.UserId = userSubject;
            userTask.Id = Guid.NewGuid().ToString();
            var userTaskCreateResponse = await _userTasksContainer.CreateItemAsync(userTask, new PartitionKey(userId));

            return userTaskCreateResponse.Resource;
        }

        public async Task Delete(string userSubject, string id)
        {
            var userId = await GetUserIdByAuthentication(userSubject);
            var userTaskReadResponse = await _userTasksContainer.ReadItemAsync<UserTask>(id, new PartitionKey(userId));
            userTaskReadResponse.Resource.IsDeleted = true;

            await _userTasksContainer.UpsertItemAsync<UserTask>(userTaskReadResponse.Resource, new PartitionKey(userId));
        }

        public async Task<UserTask> GetById(string userSubject, string id)
        {
            var userId = await GetUserIdByAuthentication(userSubject);
            var userTaskReadResponse = await _userTasksContainer.ReadItemAsync<UserTask>(id, new PartitionKey(userId));

            return userTaskReadResponse.Resource;
        }

        public async Task<IList<UserTask>> GetListForUser(string userSubject)
        {
            var userId = await GetUserIdByAuthentication(userSubject);

            using FeedIterator<UserTask> userTasksByUserIdFeed = _userTasksContainer.GetItemQueryIterator<UserTask>(
                queryText: "SELECT * FROM UserTasks u WHERE u.IsDeleted = false", requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId) }
            );

            List<UserTask> userTasksByUserId = new List<UserTask>();
            while (userTasksByUserIdFeed.HasMoreResults)
            {
                var response = await userTasksByUserIdFeed.ReadNextAsync();
                userTasksByUserId.AddRange(response.ToList());
            }

            return userTasksByUserId;
        }

        public async Task<UserTask> Update(string userSubject, UserTask updatedUserTask)
        {
            var userId = await GetUserIdByAuthentication(userSubject);
            updatedUserTask.UserId = userId;
            var userTaskUpdatesResponse = await _userTasksContainer.UpsertItemAsync<UserTask>(updatedUserTask, new PartitionKey(userId));

            return userTaskUpdatesResponse.Resource;
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
