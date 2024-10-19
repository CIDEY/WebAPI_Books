using BooksAPI.Middleware.Model;

namespace BooksAPI.Middleware
{
    public class Exceptions
    {
        public class BadRequestException : Exception
        {
            public BadRequestException(string message) : base(message) { }
        }

        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }

        public class ValidationException : Exception
        {
            public IEnumerable<ValidationError> Errors { get; }

            public ValidationException(string message, IEnumerable<ValidationError> errors) : base(message)
            {
                Errors = errors;
            }
        }
    }
}
