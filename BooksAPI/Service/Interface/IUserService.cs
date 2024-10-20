using BooksAPI.Model;

namespace BooksAPI.Service.Interface
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> GetById(int id);
        Task<User> Create(User user, string password);
    }
}
