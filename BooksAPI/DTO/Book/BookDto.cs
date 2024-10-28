namespace BooksAPI.DTO.Book
{
    /// <summary>
    /// To return book information
    /// </summary>
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }
        public int PublicationYear { get; set; }
        public double Rating { get; set; }
    }
}
