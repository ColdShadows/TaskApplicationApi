namespace TaskApplicationApi
{
    public static class AzureCosmosDbContainers
    {
        public static readonly (string Name, string PartitionKeyPath) UserTasksContainer = ("UserTasks", "/userid");

        public static readonly (string Name, string PartitionKeyPath) UsersPreferencesContainer = ("UserPreferences", "/userid");

        public static readonly (string Name, string PartitionKeyPath) UsersContainer = ("Users", "/username");
    }
}
