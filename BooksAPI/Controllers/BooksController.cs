using BooksAPI.DTO.Book;
using BooksAPI.Middleware.CustomException;
using BooksAPI.Model;
using BooksAPI.Model.FilterSort;
using BooksAPI.Service;
using BooksAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Returns a paginated list of books
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <returns>Paginated list of books</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBooks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? minYear = null,
            [FromQuery] int? maxYear = null,
            [FromQuery] double? minRating = null,
            [FromQuery] double? maxRating = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDescending = false)
        {
            if (pageNumber < 1)
                return BadRequest("Page number must be greater than 0.");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size must be between 1 and 100.");

            var parameters = new BookParameters
            {
                SearchTerm = searchTerm,
                MinYear = minYear,
                MaxYear = maxYear,
                MinRating = minRating,
                MaxRating = maxRating,
                SortBy = sortBy,
                SortDescending = sortDescending
            };

            var books = await _bookService.GetAllBooksAsync(pageNumber, pageSize, parameters);

            var booksDto = books.Items.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                AuthorName = b.Author.Name,
                GenreName = b.Genres.Name,
                PublicationYear = b.PublicationYear,
                Rating = b.Rating
            });

            return Ok(new
            {
                Books = booksDto,
                books.PageNumber,
                books.TotalPages,
                books.TotalCount,
                books.HasPreviousPage,
                books.HasNextPage,
                Status = true
            });
        }

        /// <summary>
        /// Returns the book by the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Book</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookForId(int id)
        {
            var book = await _bookService.GetBookForIdAsync(id);
            return Ok(new { Book = book, Status = true });
        }

        /// <summary>
        /// Adding a new book
        /// </summary>
        /// <param name="books"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.User))]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] CreateBookDto createBookDto)
        {
            var book = new Books
            {
                Title = createBookDto.Title,
                Description = createBookDto.Description,
                AuthorId = createBookDto.AuthorId,
                GenreId = createBookDto.GenreId,
                PublicationYear = createBookDto.PublicationYear,
                Rating = createBookDto.Rating
            };

            var addedBook = await _bookService.AddBookAsync(book);
            var bookDto = new BookDto
            {
                Id = addedBook.Id,
                Title = addedBook.Title,
                Description = addedBook.Description,
                AuthorName = addedBook.Author.Name,
                GenreName = addedBook.Genres.Name,
                PublicationYear = addedBook.PublicationYear,
                Rating = addedBook.Rating
            };

            return CreatedAtAction(nameof(GetBookForId), new { id = bookDto.Id }, new { Book = bookDto, Status = true });
        }

        /// <summary>
        /// Deleting an book by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Updating book data by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto updateBook)
        {
            if (id != updateBook.Id)
                return BadRequest("Book ID mismatch.");

            var book = new Books
            {
                Id = updateBook.Id,
                Title = updateBook.Title,
                Description = updateBook.Description,
                AuthorId = updateBook.AuthorId,
                GenreId = updateBook.GenreId,
                PublicationYear = updateBook.PublicationYear,
                Rating = updateBook.Rating
            };

            await _bookService.UpdateBookAsync(id, book);
            return NoContent();
        }
    }
}
