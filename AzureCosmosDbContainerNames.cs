namespace TaskApplicationApi
{
    public static class AzureCosmosDbContainers
    {
        public static readonly (string Name, string ParticionKeyPath) UserTasksContainer = ("UserTasks", "/usertasks");
    }
}
