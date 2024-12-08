namespace TaskApplicationApi.Exceptions
{
    public class ResourceAlreadyExistsException : Exception
    {
        public ResourceAlreadyExistsException(string resourceName) 
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; private set; }
    }
}
