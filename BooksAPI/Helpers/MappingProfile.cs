using BooksAPI.DTO.User;
using BooksAPI.Model;

namespace BooksAPI.Helpers
{
    public static class MappingProfile
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        public static User ToEntity(this UpdateUserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Username = dto.Username,
                Role = dto.Role
            };
        }
    }
}
