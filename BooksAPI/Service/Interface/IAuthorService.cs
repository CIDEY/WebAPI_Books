using BooksAPI.Model;

namespace BooksAPI.Service.Interface
{
    public interface IAuthorService
    {
        Task<IEnumerable<Authors>> GetAllAuthorsAsync();
        Task<Authors> GetAuthorForIdAsync(int id);
        Task<Authors> AddAuthorAsync(Authors author);
        Task UpdateAuthorAsync(int id, Authors author);
        Task DeleteAuthorAsync(int id);
    }
}
