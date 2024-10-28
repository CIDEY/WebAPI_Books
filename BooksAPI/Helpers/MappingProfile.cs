using BooksAPI.DTO.Author;
using BooksAPI.DTO.Book;
using BooksAPI.DTO.Genre;
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
                Role = user.Role.ToString()
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

        public static BookDto ToDto(this Books book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorName = book.Author.Name,
                GenreName = book.Genres.Name,
                PublicationYear = book.PublicationYear,
                Rating = book.Rating
            };
        }

        public static Books ToEntity(this CreateBookDto dto)
        {
            return new Books
            {
                Title = dto.Title,
                Description = dto.Description,
                AuthorId = dto.AuthorId,
                GenreId = dto.GenreId,
                PublicationYear = dto.PublicationYear,
                Rating = dto.Rating
            };
        }

        public static AuthorDto ToDto(this Authors author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name
            };
        }

        public static GenreDto ToDto(this Genres genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }
    }
}
