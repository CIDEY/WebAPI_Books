namespace BooksAPI.DTO.User
{
    /// <summary>
    /// To update user data
    /// </summary>
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
