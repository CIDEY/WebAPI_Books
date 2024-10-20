using BooksAPI.Model;

namespace BooksAPI.Service.Interface
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
