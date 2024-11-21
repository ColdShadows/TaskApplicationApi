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

        public async Task<UserTask> Create(UserTask userTask)
        {
            userTask.Id = Guid.NewGuid().ToString();
            var userTaskCreateResponse = await _userTasksContainer.CreateItemAsync(userTask);

            return userTaskCreateResponse.Resource;
        }

        public async Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserTask> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<UserTask>> GetListForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserTask> Update(string id, UserTask updatedUserTask)
        {
            throw new NotImplementedException();
        }
    }
}
