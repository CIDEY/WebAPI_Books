using System.ComponentModel.DataAnnotations;

namespace BooksAPI.DTO.Book
{
    /// <summary>
    /// to create a new book
    /// </summary>
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int GenreId { get; set; }
        public int PublicationYear { get; set; }
        public double Rating { get; set; }
    }
}
