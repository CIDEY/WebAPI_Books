﻿using Microsoft.EntityFrameworkCore;
using BooksAPI.DBContextAPI;
using BooksAPI.Middleware.CustomException;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using BooksAPI.Model.FilterSort;

namespace BooksAPI.Service
{
    public class GenreService : IGenreService
    {
        private readonly BooksApiDb _context;
        private readonly ICacheService _cacheService;
        public GenreService(BooksApiDb context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }
        public async Task<PaginatedList<Genres>> GetAllGenresAsync(int pageNumber,
            int pageSize,
            GenreParameters parameters)
        {
            string cacheKey = $"genres_page_{pageNumber}_size_{pageSize}_params_{parameters.SearchTerm}_{parameters.SortBy}_{parameters.SortDescending}";

            return await _cacheService.GetOrCreate(cacheKey, async () =>
            {
                var query = _context.Genres.AsNoTracking();

                // Применяем фильтр
                if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
                    query = query.Where(g => g.Name.Contains(parameters.SearchTerm));

                // Применяем сортировку
                if (!string.IsNullOrWhiteSpace(parameters.SortBy))
                {
                    switch (parameters.SortBy.ToLower())
                    {
                        case "name":
                            query = parameters.SortDescending
                                ? query.OrderByDescending(g => g.Name)
                                : query.OrderBy(g => g.Name);
                            break;
                        default:
                            query = query.OrderBy(g => g.Id);
                            break;
                    }
                }
                else
                    query = query.OrderBy(g => g.Id);

                var count = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return new PaginatedList<Genres>(items, count, pageNumber, pageSize);
            }, TimeSpan.FromMinutes(10));
        }

        public async Task<Genres> GetGenreForIdAsync(int id)
        {
            return await _cacheService.GetOrCreate($"genre_{id}", async () =>
            {
                var genre = await _context.Genres.FindAsync(id);
                if (genre == null)
                    throw new NotFoundException($"Genre with ID {id} not found.");
                return genre;
            }, TimeSpan.FromHours(1));
        }

        public async Task<Genres> AddGenreAsync(Genres genre)
        {
            if (genre == null)
                throw new BadRequestException("Genre data is null.");

            if (string.IsNullOrWhiteSpace(genre.Name))
                throw new BadRequestException("Genre name cannot be empty.");

            if (await _context.Genres.AnyAsync(g => g.Name == genre.Name))
                throw new BadRequestException($"Genre with name '{genre.Name}' already exists.");

            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();

            _cacheService.Remove("genres_");

            return genre;
        }

        public async Task UpdateGenreAsync(int id, Genres updatedGenre)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
                throw new NotFoundException($"Genre with ID {id} not found.");

            if (string.IsNullOrWhiteSpace(updatedGenre.Name))
                throw new BadRequestException("Genre name cannot be empty.");

            if (await _context.Genres.AnyAsync(g => g.Name == updatedGenre.Name && g.Id != id))
                throw new BadRequestException($"Genre with name '{updatedGenre.Name}' already exists.");

            genre.Name = updatedGenre.Name;

            await _context.SaveChangesAsync();

            _cacheService.Remove($"genre_{id}");
            _cacheService.Remove("genres_");
        }

        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
                throw new NotFoundException($"Genre with ID {id} not found.");

            if (await _context.Books.AnyAsync(b => b.GenreId == id))
                throw new BadRequestException($"Cannot delete genre with ID {id} because it is associated with one or more books.");

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            // Инвалидируем кэш для этого жанра и списка жанров
            _cacheService.Remove($"genre_{id}");
            _cacheService.Remove("genres_");
        }
    }
}