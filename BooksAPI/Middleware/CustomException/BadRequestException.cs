namespace BooksAPI.Middleware.CustomException
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
