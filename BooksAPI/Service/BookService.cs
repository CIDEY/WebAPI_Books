using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using BooksAPI.DBContextAPI;
using BooksAPI.Middleware.CustomException;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using System.ComponentModel.DataAnnotations;
using BooksAPI.Model.FilterSort;

namespace BooksAPI.Service
{

    public class BookService : IBookService
    {
        private readonly BooksApiDb _context;
        private readonly ICacheService _cacheService;
        public BookService(BooksApiDb context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<PaginatedList<Books>> GetAllBooksAsync(int pageNumber, int pageSize, BookParameters parameters)
        {
            string cacheKey = $"books_page_{pageNumber}_size_{pageSize}_params_{parameters.SearchTerm}_{parameters.MinYear}_{parameters.MaxYear}_{parameters.MinRating}_{parameters.MaxRating}_{parameters.SortBy}_{parameters.SortDescending}";

            return await _cacheService.GetOrCreate(cacheKey, async () =>
            {
                var query = _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genres)
                    .AsNoTracking();

                // Применяем фильтры
                if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
                {
                    query = query.Where(b => b.Title.Contains(parameters.SearchTerm) ||
                                             b.Author.Name.Contains(parameters.SearchTerm) ||
                                             b.Genres.Name.Contains(parameters.SearchTerm));
                }

                if (parameters.MinYear.HasValue)
                    query = query.Where(b => b.PublicationYear >= parameters.MinYear.Value);

                if (parameters.MaxYear.HasValue)
                    query = query.Where(b => b.PublicationYear <= parameters.MaxYear.Value);

                if (parameters.MinRating.HasValue)
                    query = query.Where(b => b.Rating >= parameters.MinRating.Value);

                if (parameters.MaxRating.HasValue)
                    query = query.Where(b => b.Rating <= parameters.MaxRating.Value);

                // Применяем сортировку
                if (!string.IsNullOrWhiteSpace(parameters.SortBy))
                {
                    switch (parameters.SortBy.ToLower())
                    {
                        case "title":
                            query = parameters.SortDescending
                                ? query.OrderByDescending(b => b.Title)
                                : query.OrderBy(b => b.Title);
                            break;
                        case "year":
                            query = parameters.SortDescending
                                ? query.OrderByDescending(b => b.PublicationYear)
                                : query.OrderBy(b => b.PublicationYear);
                            break;
                        case "rating":
                            query = parameters.SortDescending
                                ? query.OrderByDescending(b => b.Rating)
                                : query.OrderBy(b => b.Rating);
                            break;
                        case "author":
                            query = parameters.SortDescending
                                ? query.OrderByDescending(b => b.Author.Name)
                                : query.OrderBy(b => b.Author.Name);
                            break;
                        default:
                            query = query.OrderBy(b => b.Id);
                            break;
                    }
                }
                else
                    query = query.OrderBy(b => b.Id);

                var count = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return new PaginatedList<Books>(items, count, pageNumber, pageSize);
            }, TimeSpan.FromMinutes(10));
        }

        public async Task<Books> GetBookForIdAsync(int id)
        {
            return await _cacheService.GetOrCreate($"book_{id}", async () =>
            {
                var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                    throw new NotFoundException($"Book with ID {id} not found.");

                return book;
            }, TimeSpan.FromHours(1));
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

            _cacheService.Remove("books_");

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

            _cacheService.Remove($"book_{id}");
            _cacheService.Remove("books_");
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found.");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            _cacheService.Remove($"book_{id}");
            _cacheService.Remove("books_");
        }
    }
}
