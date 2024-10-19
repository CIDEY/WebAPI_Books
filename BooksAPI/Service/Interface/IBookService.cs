using BooksAPI.Model;

namespace BooksAPI.Service.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<Books>> GetAllBooksAsync();
        Task<Books> GetBookForIdAsync(int id);
        Task<Books> AddBookAsync(Books genre);
        Task DeleteBookAsync(int id);
        Task UpdateBookAsync(int id, Books book);
    }
}
