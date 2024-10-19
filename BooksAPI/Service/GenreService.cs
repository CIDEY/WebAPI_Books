using Microsoft.EntityFrameworkCore;
using BooksAPI.DBContextAPI;
using BooksAPI.Middleware.CustomException;
using BooksAPI.Model;
using BooksAPI.Service.Interface;

namespace BooksAPI.Service
{
    public class GenreService : IGenreService
    {
        private readonly BooksApiDb _context;

        public GenreService(BooksApiDb context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Genres>> GetAllGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }
        public async Task AddGenreAsync(Genres genre)
        {
            if (string.IsNullOrWhiteSpace(genre.Name))
                throw new BadRequestException("Genres name cannot be empty.");

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }

        public async Task<Genres> GetGenreForIdAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
                throw new NotFoundException($"Genre with ID {id} not found.");
            return genre;
        }
        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
                throw new NotFoundException($"Genre with ID {id} not found.");

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
    }
}