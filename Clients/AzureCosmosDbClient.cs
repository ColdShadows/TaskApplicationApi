using Microsoft.Azure.Cosmos;

namespace TaskApplicationApi.Clients
{
    public class AzureCosmosDbClient : IAzureCosmosDbClient
    {
        private readonly IConfiguration _config;
        private readonly CosmosClient _cosmosClient;
        private Database _cosmosDb;
        private readonly Dictionary<string, Container> _containers = new Dictionary<string, Container>();

        public AzureCosmosDbClient(CosmosClient cosmosClient, IConfiguration configuration)
        {
            _config = configuration;
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
            _cosmosDb = _cosmosClient.CreateDatabaseIfNotExistsAsync(_config.GetValue<string>("CosmosDB:Database")).GetAwaiter().GetResult();

            InitializeContainers();
        }

        private void InitializeContainers()
        {
            Container userTasksContainer = _cosmosDb.CreateContainerIfNotExistsAsync(
                AzureCosmosDbContainers.UserTasksContainer.Name,
                AzureCosmosDbContainers.UserTasksContainer.PartitionKeyPath)
                .GetAwaiter()
                .GetResult();

            Container usersContainer = _cosmosDb.CreateContainerIfNotExistsAsync(
                AzureCosmosDbContainers.UsersContainer.Name,
                AzureCosmosDbContainers.UsersContainer.PartitionKeyPath)
                .GetAwaiter()
                .GetResult();

            Container userPreferencesContainer = _cosmosDb.CreateContainerIfNotExistsAsync(
                AzureCosmosDbContainers.UsersPreferencesContainer.Name,
                AzureCosmosDbContainers.UsersPreferencesContainer.PartitionKeyPath)
                .GetAwaiter()
                .GetResult();

            _containers.Add(AzureCosmosDbContainers.UserTasksContainer.Name, userTasksContainer);
            _containers.Add(AzureCosmosDbContainers.UsersContainer.Name, usersContainer);
            _containers.Add(AzureCosmosDbContainers.UsersPreferencesContainer.Name, userPreferencesContainer);
        }

        public Container GetContainer(string containerName)
        {
            return _containers.TryGetValue(containerName, out var container)
                ? container
                : throw new InvalidOperationException($"Container with name {containerName} not found");
        }
    }
}
