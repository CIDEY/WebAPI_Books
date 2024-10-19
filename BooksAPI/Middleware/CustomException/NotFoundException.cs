namespace BooksAPI.Middleware.CustomException
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
