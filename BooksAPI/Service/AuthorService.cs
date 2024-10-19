using Microsoft.EntityFrameworkCore;
using BooksAPI.DBContextAPI;
using BooksAPI.Middleware.CustomException;
using BooksAPI.Model;
using BooksAPI.Service.Interface;

namespace BooksAPI.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly BooksApiDb _context;
        public AuthorService(BooksApiDb context)
        {
            _context = context;
        }

        public async Task AddAuthorAsync(Authors authors)
        {
            if (string.IsNullOrWhiteSpace(authors.Name))
                throw new BadRequestException("Author name cannot be empty.");

            _context.Authors.Add(authors);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Authors>> GetAllAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }
        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                throw new NotFoundException($"Author with ID {id} not found.");

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<Authors> GetAuthorForIdAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                throw new NotFoundException($"Author with ID {id} not found.");
            return author;
        }
    }
}
