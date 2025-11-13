namespace Catalog.API.Exceptions
{
    public class CustomException(string message) : Exception(message)
    {
        public static CustomException NotFound(string msg) => new(msg);
    }

}