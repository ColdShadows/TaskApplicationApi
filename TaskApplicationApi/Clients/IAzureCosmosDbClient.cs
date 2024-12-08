using Microsoft.Azure.Cosmos;

namespace TaskApplicationApi.Clients
{
    public interface IAzureCosmosDbClient
    {
        public Container GetContainer(string containerName);
    }
}