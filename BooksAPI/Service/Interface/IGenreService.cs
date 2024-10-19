using BooksAPI.Model;

namespace BooksAPI.Service.Interface
{
    public interface IGenreService
    {
        Task<IEnumerable<Genres>> GetAllGenresAsync();
        Task<Genres> GetGenreForIdAsync(int id);
        Task AddGenreAsync(Genres genre);
        Task DeleteGenreAsync(int id);
    }
}
