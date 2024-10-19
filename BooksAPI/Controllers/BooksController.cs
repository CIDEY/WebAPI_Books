using BooksAPI.Middleware.CustomException;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
            try
            {
                var book = await _bookService.GetBookForIdAsync(id);
                return Ok(new { Book = book, Status = true });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message, Status = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Books books)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)),
                    Status = false
                });
            }

            try
            {
                await _bookService.AddBookAsync(books);
                return CreatedAtAction(nameof(GetBookForId), new { id = books.Id }, new { Book = books, Status = true });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message, Status = false });
            }
            catch (Exception ex)
            {
                // Логирование исключения
                return StatusCode(500, new { Message = "An error occurred while processing your request", Status = false });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message, Status = false });
            }
        }
    }
}
