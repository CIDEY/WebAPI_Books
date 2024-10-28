using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BooksAPI.DTO.User
{
    public class LoginDto
    {
        [Required]
        [FromForm(Name = "username")]
        public string Username { get; set; }
        [Required]
        [FromForm(Name = "password")]
        public string Password { get; set; }
    }
}
