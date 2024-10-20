using BooksAPI.DBContextAPI;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BooksAPI.Service
{
    public class UserService : IUserService
    {
        private readonly BooksApiDb _context;
        public UserService(BooksApiDb context)
        {
            _context = context;
        }
        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash))
                return null;

            return user;
        }

        private static bool VerifyPasswordHash(string password, string storedHash)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(storedHash)) throw new ArgumentException("Invalid hash", nameof(storedHash));

            var computedHash = CreatePasswordHash(password);
            return computedHash == storedHash;
        }

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password is required");

            if (_context.Users.Any(x => x.Username == user.Username))
                throw new ArgumentException("Username \"" + user.Username + "\" is already taken");

            user.PasswordHash = CreatePasswordHash(password);
            user.Role = "User";
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        private static string CreatePasswordHash(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
