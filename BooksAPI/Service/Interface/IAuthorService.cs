using BooksAPI.Model;
using BooksAPI.Model.FilterSort;

namespace BooksAPI.Service.Interface
{
    public interface IAuthorService
    {
        Task<PaginatedList<Authors>> GetAllAuthorsAsync(int pageNumber, int pageSize, AuthorParameters parameters);
        Task<Authors> GetAuthorForIdAsync(int id);
        Task<Authors> AddAuthorAsync(Authors author);
        Task UpdateAuthorAsync(int id, Authors author);
        Task DeleteAuthorAsync(int id);
    }
}
