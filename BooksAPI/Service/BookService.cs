using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using BooksAPI.DBContextAPI;
using BooksAPI.Middleware.CustomException;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Service
{

    public class BookService : IBookService
    {
        private readonly BooksApiDb _context;
        public BookService(BooksApiDb context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Books>> GetAllBooksAsync(int pageNumber, int pageSize)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .AsNoTracking();

            var count = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<Books>(items, count, pageNumber, pageSize);
        }

        public async Task<Books> GetBookForIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found.");

            return book;
        }

        public async Task<Books> AddBookAsync(Books book)
        {
            if (book == null)
                throw new BadRequestException("Book data is null.");

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new BadRequestException("Book title cannot be empty.");

            if (!await _context.Authors.AnyAsync(a => a.Id == book.AuthorId))
                throw new BadRequestException($"Author with ID {book.AuthorId} does not exist.");

            if (!await _context.Genres.AnyAsync(g => g.Id == book.GenreId))
                throw new BadRequestException($"Genre with ID {book.GenreId} does not exist.");

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task UpdateBookAsync(int id, Books updatedBook)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found.");

            if (string.IsNullOrWhiteSpace(updatedBook.Title))
                throw new BadRequestException("Book title cannot be empty.");

            if (!await _context.Authors.AnyAsync(a => a.Id == updatedBook.AuthorId))
                throw new BadRequestException($"Author with ID {updatedBook.AuthorId} does not exist.");

            if (!await _context.Genres.AnyAsync(g => g.Id == updatedBook.GenreId))
                throw new BadRequestException($"Genre with ID {updatedBook.GenreId} does not exist.");

            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.AuthorId = updatedBook.AuthorId;
            book.GenreId = updatedBook.GenreId;
            book.PublicationYear = updatedBook.PublicationYear;
            book.Rating = updatedBook.Rating;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found.");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

    }
}
