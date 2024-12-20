﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.DBContextAPI;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using BooksAPI.Service;
using BooksAPI.Model.FilterSort;
using Microsoft.AspNetCore.Authorization;
using BooksAPI.DTO.Genre;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        /// <summary>
        /// Returns a paginated list of genres
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <returns>Paginated list of genres</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllGenres(
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

            var parameters = new GenreParameters
            {
                SearchTerm = searchTerm,
                SortBy = sortBy,
                SortDescending = sortDescending
            };

            var genres = await _genreService.GetAllGenresAsync(pageNumber, pageSize, parameters);
            var genresDto = genres.Items.Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name
            });

            return Ok(new
            {
                Genres = genresDto,
                genres.PageNumber,
                genres.TotalPages,
                genres.TotalCount,
                genres.HasPreviousPage,
                genres.HasNextPage,
                Status = true
            });
        }

        /// <summary>
        /// Returns the Genre by the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Genre</returns>
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

        /// <summary>
        /// Adding a new genre
        /// </summary>
        /// <param name="genres"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddGenre(Genres genres)
        {
            await _genreService.AddGenreAsync(genres);

            return CreatedAtAction(nameof(GetGenreForId), new { id = genres.Id }, genres);
        }

        /// <summary>
        /// Deleting an genre by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreService.GetGenreForIdAsync(id);

            await _genreService.DeleteGenreAsync(id);
            return NoContent(); // Возвращаем статус 204 No Content
        }
        /// <summary>
        /// Updating genre data by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="genre"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Genres genre)
        {
            await _genreService.UpdateGenreAsync(id, genre);
            return NoContent();
        }
    }
}
