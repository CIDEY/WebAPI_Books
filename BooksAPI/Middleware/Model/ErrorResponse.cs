namespace BooksAPI.Middleware.Model
{
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string TraceId { get; set; }
        public string DeveloperMessage { get; set; }
        public List<ValidationError> Errors { get; set; }
    }
}
