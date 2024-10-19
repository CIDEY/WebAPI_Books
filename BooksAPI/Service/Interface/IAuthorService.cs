using BooksAPI.Model;

namespace BooksAPI.Service.Interface
{
    public interface IAuthorService
    {
        Task<PaginatedList<Authors>> GetAllAuthorsAsync(int pageNumber, int pageSize);
        Task<Authors> GetAuthorForIdAsync(int id);
        Task<Authors> AddAuthorAsync(Authors author);
        Task UpdateAuthorAsync(int id, Authors author);
        Task DeleteAuthorAsync(int id);
    }
}
