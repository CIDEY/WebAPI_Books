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
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();

            return Ok(new 
            { 
                Books = books,
                Status = true
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookForId(int id)
        {
            var books = await _bookService.GetBookForIdAsync(id);

            return Ok(new
            {
                Books = books,
                Status = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Books books)
        {
            await _bookService.AddBookAsync(books);

            return CreatedAtAction(nameof(GetBookForId), new { id = books.Id }, books);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var books = await _bookService.GetBookForIdAsync(id);

            await _bookService.DeleteBookAsync(id);
            return NoContent(); // Возвращаем статус 204 No Content
        }
    }
}
