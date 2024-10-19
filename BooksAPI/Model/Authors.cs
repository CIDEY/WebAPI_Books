using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Model
{
    public class Authors
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters")]
        public string Name { get; set; }
    }
}
