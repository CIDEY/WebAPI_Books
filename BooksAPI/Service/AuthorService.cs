﻿using Microsoft.EntityFrameworkCore;
using BooksAPI.DBContextAPI;
using BooksAPI.Middleware.CustomException;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using BooksAPI.Model.FilterSort;
using Microsoft.AspNetCore.Server.HttpSys;

namespace BooksAPI.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly BooksApiDb _context;
        private readonly ICacheService _cacheService;
        public AuthorService(BooksApiDb context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<PaginatedList<Authors>> GetAllAuthorsAsync(int pageNumber, int pageSize, AuthorParameters parameters)
        {
            string cacheKey = $"author_page_{pageNumber}_size_{pageSize}_params_{parameters.SearchTerm}_{parameters.SortBy}_{parameters.SortDescending}";

            return await _cacheService.GetOrCreate(cacheKey, async () =>
            {
                var query = _context.Authors.AsNoTracking();

                // Применяем фильтр
                if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
                    query = query.Where(a => a.Name.Contains(parameters.SearchTerm));

                // Применяем сортировку
                if (!string.IsNullOrWhiteSpace(parameters.SortBy))
                {
                    switch (parameters.SortBy.ToLower())
                    {
                        case "name":
                            query = parameters.SortDescending
                                ? query.OrderByDescending(a => a.Name)
                                : query.OrderBy(a => a.Name);
                            break;
                        default:
                            query = query.OrderBy(a => a.Id);
                            break;
                    }
                }
                else
                    query = query.OrderBy(a => a.Id);

                var count = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return new PaginatedList<Authors>(items, count, pageNumber, pageSize);
            }, TimeSpan.FromMinutes(10));
        }

        public async Task<Authors> GetAuthorForIdAsync(int id)
        {
            return await _cacheService.GetOrCreate($"author_{id}", async () =>
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                    throw new NotFoundException($"Author with ID {id} not found.");
                return author;
            }, TimeSpan.FromHours(1));
        }

        public async Task<Authors> AddAuthorAsync(Authors author)
        {
            if (author == null)
                throw new BadRequestException("Author data is null.");

            if (string.IsNullOrWhiteSpace(author.Name))
                throw new BadRequestException("Author name cannot be empty.");

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            _cacheService.Remove("books_");

            return author;
        }

        public async Task UpdateAuthorAsync(int id, Authors updatedAuthor)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                throw new NotFoundException($"Author with ID {id} not found.");

            if (string.IsNullOrWhiteSpace(updatedAuthor.Name))
                throw new BadRequestException("Author name cannot be empty.");

            author.Name = updatedAuthor.Name;

            await _context.SaveChangesAsync();

            _cacheService.Remove($"book_{id}");
            _cacheService.Remove("books_");
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                throw new NotFoundException($"Author with ID {id} not found.");

            if (await _context.Books.AnyAsync(b => b.AuthorId == id))
                throw new BadRequestException($"Cannot delete author with ID {id} because they have associated books.");

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            _cacheService.Remove($"book_{id}");
            _cacheService.Remove("book_");
        }
    }
}
