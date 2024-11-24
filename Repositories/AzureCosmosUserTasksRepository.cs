using TaskApplicationApi.Models;

using Microsoft.Azure.Cosmos;
using TaskApplicationApi.Clients;

namespace TaskApplicationApi.Repositories
{
    public class AzureCosmosUserTasksRepository : IUserTasksRepository
    {
        Container _userTasksContainer;

        public AzureCosmosUserTasksRepository(IAzureCosmosDbClient azureCosmosDBClient) 
        {
            if(azureCosmosDBClient is null)
            {
                throw new ArgumentNullException(nameof(azureCosmosDBClient));
            }

            _userTasksContainer = azureCosmosDBClient.GetContainer(AzureCosmosDbContainers.UserTasksContainer.Name);
        }

        public async Task<UserTask> Create(string userId, UserTask userTask)
        {
            userTask.Id = Guid.NewGuid().ToString();
            userTask.UserId = userId;
            var userTaskCreateResponse = await _userTasksContainer.CreateItemAsync(userTask, new PartitionKey(userId));

            return userTaskCreateResponse.Resource;
        }

        public async Task Delete(string userId, string id)
        {
            var userTaskReadResponse = await _userTasksContainer.ReadItemAsync<UserTask>(id, new PartitionKey(userId));
            userTaskReadResponse.Resource.IsDeleted = true;

            await _userTasksContainer.UpsertItemAsync<UserTask>(userTaskReadResponse.Resource, new PartitionKey(userId));
        }

        public async Task<UserTask> GetById(string userId, string id)
        {
            var userTaskReadResponse = await _userTasksContainer.ReadItemAsync<UserTask>(id, new PartitionKey(userId));

            return userTaskReadResponse.Resource;
        }

        public async Task<IList<UserTask>> GetListForUser(string userId)
        {
            using FeedIterator<UserTask> userTasksByUserIdFeed = _userTasksContainer.GetItemQueryIterator<UserTask>(
                queryText: $"SELECT * FROM UserTasks u WHERE u.IsDeleted = false", requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId) }
            );

            List<UserTask> userTasksByUserId = new List<UserTask>();
            while (userTasksByUserIdFeed.HasMoreResults)
            {
                var response = await userTasksByUserIdFeed.ReadNextAsync();
                userTasksByUserId.AddRange(response.ToList());
            }

            return userTasksByUserId;
        }

        public async Task<UserTask> Update(string userId, UserTask updatedUserTask)
        {
            updatedUserTask.UserId = userId;
            var userTaskUpdatesResponse = await _userTasksContainer.UpsertItemAsync<UserTask>(updatedUserTask, new PartitionKey(userId));

            return userTaskUpdatesResponse.Resource;
        }
    }
}
