using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.DBContextAPI;
using BooksAPI.Model;
using BooksAPI.Service;
using BooksAPI.Service.Interface;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();

            return Ok(new
            {
                Authors = authors,
                Status = true
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorsForId(int id)
        {
            var authors = await _authorService.GetAuthorForIdAsync(id);

            return Ok(new
            {
                Authors = authors,
                Status = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor(Authors authors)
        {
            await _authorService.AddAuthorAsync(authors);

            return CreatedAtAction(nameof(GetAuthorsForId), new { id = authors.Id }, authors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorService.GetAuthorForIdAsync(id);

            await _authorService.DeleteAuthorAsync(id);
            return NoContent(); // Возвращаем статус 204 No Content
        }
    }
}
