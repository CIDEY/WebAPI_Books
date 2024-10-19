using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using BooksAPI.DBContextAPI;
using BooksAPI.Middleware.CustomException;
using BooksAPI.Model;
using BooksAPI.Service.Interface;

namespace BooksAPI.Service
{

    public class BookService : IBookService
    {
        private readonly BooksApiDb _context;
        public BookService(BooksApiDb context) {
            _context = context; 
        }

        public async Task AddBookAsync(Books books)
        {
            if (string.IsNullOrWhiteSpace(books.Title))
                throw new BadRequestException("Title name cannot be empty.");

            _context.Books.Add(books);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Books>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found.");
  
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Books> GetBookForIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found.");
            return book;
        }
    }
}
