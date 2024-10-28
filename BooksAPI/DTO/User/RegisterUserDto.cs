using System.ComponentModel.DataAnnotations;

namespace BooksAPI.DTO.User
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage ="Username is required.")]
        [StringLength(50, ErrorMessage ="Username cannot be longer than 50 char's.")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
    }
}
