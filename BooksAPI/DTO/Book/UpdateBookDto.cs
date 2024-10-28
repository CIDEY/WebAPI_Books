namespace BooksAPI.DTO.Book
{
    public class UpdateBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public int PublicationYear { get; set; }
        public double Rating { get; set; }
    }
}
