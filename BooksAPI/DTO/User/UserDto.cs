namespace BooksAPI.DTO.User
{
    /// <summary>
    /// To return user information without sensitive data
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
