using BooksAPI.Model;
using BooksAPI.Model.FilterSort;

namespace BooksAPI.Service.Interface
{
    public interface IGenreService
    {
        Task<PaginatedList<Genres>> GetAllGenresAsync(int pageNumber, int pageSize, GenreParameters parameters);
        Task<Genres> GetGenreForIdAsync(int id);
        Task<Genres> AddGenreAsync(Genres genre);
        Task UpdateGenreAsync(int id, Genres genre);
        Task DeleteGenreAsync(int id);
    }
}
