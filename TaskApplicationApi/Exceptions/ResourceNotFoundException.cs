namespace TaskApplicationApi.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string resourceName)
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; private set; }
    }
}
