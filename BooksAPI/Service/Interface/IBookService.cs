using BooksAPI.Model;

namespace BooksAPI.Service.Interface
{
    public interface IBookService
    {
        Task<PaginatedList<Books>> GetAllBooksAsync(int pageNumber, int pageSize);
        Task<Books> GetBookForIdAsync(int id);
        Task<Books> AddBookAsync(Books genre);
        Task DeleteBookAsync(int id);
        Task UpdateBookAsync(int id, Books book);
    }
}
