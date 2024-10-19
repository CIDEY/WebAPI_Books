using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksAPI.Model
{
    public class Books
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Authors Author { get; set; }
        [Required]
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public Genres Genres { get; set; }
        public int PublicationYear { get; set; }
        public double Rating { get; set; }
    }
}
