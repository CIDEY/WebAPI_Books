using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksAPI.Model
{
    public class Books
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Author ID is required")]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Authors Author { get; set; }

        [Required(ErrorMessage = "Genre ID is required")]
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public Genres Genres { get; set; }

        [Range(1800, 2100, ErrorMessage = "Publication year must be between 1800 and 2100")]
        public int PublicationYear { get; set; }
        public double Rating { get; set; }
    }
}
