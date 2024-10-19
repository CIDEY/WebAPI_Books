using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Model
{
    public class Genres
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Genre name is required")]
        [StringLength(100, ErrorMessage = "Genre name cannot be longer than 100 characters")]
        public string Name { get; set; }
    }
}
