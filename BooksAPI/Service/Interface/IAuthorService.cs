using BooksAPI.Model;

namespace BooksAPI.Service.Interface
{
    public interface IAuthorService
    {
        Task<IEnumerable<Authors>> GetAllAuthorsAsync();
        Task<Authors> GetAuthorForIdAsync(int id);
        Task AddAuthorAsync(Authors authors);
        Task DeleteAuthorAsync(int id);
    }
}
