using BooksAPI.Model;

namespace BooksAPI.Service.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<Books>> GetAllBooksAsync();
        Task<Books> GetBookForIdAsync(int id);
        Task AddBookAsync(Books genre);
        Task DeleteBookAsync(int id);
    }
}
