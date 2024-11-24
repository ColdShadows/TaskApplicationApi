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
            Container userTasksContainer = _cosmosDb.CreateContainerIfNotExistsAsync(AzureCosmosDbContainers.UserTasksContainer.Name, AzureCosmosDbContainers.UserTasksContainer.PartitionKeyPath).GetAwaiter().GetResult();

            _containers.Add(AzureCosmosDbContainers.UserTasksContainer.Name, userTasksContainer);
        }

        public Container GetContainer(string containerName)
        {
            return _containers.TryGetValue(containerName, out var container)
                ? container
                : throw new InvalidOperationException($"Container with name {containerName} not found");
        }
    }
}
