using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.DBContextAPI;
using BooksAPI.Model;
using BooksAPI.Service.Interface;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAllGenresAsync();

            return Ok(new
            {
                Genres = genres,
                Status = true
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreForId(int id)
        {
            var genre = await _genreService.GetGenreForIdAsync(id);

            return Ok(new
            {
                Genre = genre,
                Status = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre(Genres genres)
        {
            await _genreService.AddGenreAsync(genres);

            return CreatedAtAction(nameof(GetGenreForId), new { id = genres.Id }, genres);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreService.GetGenreForIdAsync(id);

            await _genreService.DeleteGenreAsync(id);
            return NoContent(); // Возвращаем статус 204 No Content
        }
    }
}
