namespace BuildingBlocks.Exceptions
{
    public class CustomException(string message, int? internalCode = 500) : Exception(message)
    {
        public int? InternalCode { get; set; } = internalCode;
    }

    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message, 404) { }
        public NotFoundException(string entity, string key) : base($"Entity <{entity}> ({key}) was not found", 404) { }
    }
}