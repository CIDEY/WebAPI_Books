using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.DBContextAPI;
using BooksAPI.Model;
using BooksAPI.Service;
using BooksAPI.Service.Interface;
using BooksAPI.Model.FilterSort;

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

        /// <summary>
        /// Returns a paginated list of authors
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <returns>Paginated list of authors</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDescending = false)
        {
            if (pageNumber < 1)
                return BadRequest("Page number must be greater than 0.");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size must be between 1 and 100.");

            var parameters = new AuthorParameters
            {
                SearchTerm = searchTerm,
                SortBy = sortBy,
                SortDescending = sortDescending
            };

            var authors = await _authorService.GetAllAuthorsAsync(pageNumber, pageSize, parameters);

            return Ok(new
            {
                Authors = authors.Items,
                authors.PageNumber,
                authors.TotalPages,
                authors.TotalCount,
                authors.HasPreviousPage,
                authors.HasNextPage,
                Status = true
            });
        }

        /// <summary>
        /// Returns the author by the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Author</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var author = await _authorService.GetAuthorForIdAsync(id);
            return Ok(author);
        }

        /// <summary>
        /// Adding a new author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] Authors author)
        {
            var newAuthor = await _authorService.AddAuthorAsync(author);
            return CreatedAtAction(nameof(GetAuthor), new { id = newAuthor.Id }, newAuthor);
        }

        /// <summary>
        /// Updating author data by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Authors author)
        {
            await _authorService.UpdateAuthorAsync(id, author);
            return NoContent();
        }

        /// <summary>
        /// Deleting an author by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorService.DeleteAuthorAsync(id);
            return NoContent();
        }
    }
}