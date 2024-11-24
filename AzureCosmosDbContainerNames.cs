namespace TaskApplicationApi
{
    public static class AzureCosmosDbContainers
    {
        public static readonly (string Name, string PartitionKeyPath) UserTasksContainer = ("UserTasks", "/userid");
    }
}
