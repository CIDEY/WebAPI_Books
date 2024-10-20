using BooksAPI.DBContextAPI;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using BooksAPI.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using BooksAPI.Middleware.CustomException; // Добавьте эту строку

namespace BooksAPI.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<BooksApiDb> _mockContext;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockContext = new Mock<BooksApiDb>(new DbContextOptions<BooksApiDb>());
            _mockCacheService = new Mock<ICacheService>();
            _bookService = new BookService(_mockContext.Object, _mockCacheService.Object);
        }

        [Fact]
        public async Task GetBookForIdAsync_ReturnsBook_WhenBookExists()
        {
            // Arrange
            var bookId = 1;
            var expectedBook = new Books { Id = bookId, Title = "Test Book" };

            var mockSet = new Mock<DbSet<Books>>();
            mockSet.Setup(m => m.FindAsync(bookId)).ReturnsAsync(expectedBook);
            _mockContext.Setup(m => m.Books).Returns(mockSet.Object);

            _mockCacheService.Setup(m => m.GetOrCreate(
                It.IsAny<string>(),
                It.IsAny<Func<Books>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<TimeSpan?>()))
                .Returns(expectedBook);

            // Act
            var result = await _bookService.GetBookForIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
            Assert.Equal("Test Book", result.Title);
        }

        [Fact]
        public async Task GetBookForIdAsync_ThrowsNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = 1;

            var mockSet = new Mock<DbSet<Books>>();
            mockSet.Setup(m => m.FindAsync(bookId)).ReturnsAsync((Books)null);
            _mockContext.Setup(m => m.Books).Returns(mockSet.Object);

            _mockCacheService.Setup(m => m.GetOrCreate(
                It.IsAny<string>(),
                It.IsAny<Func<Books>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<TimeSpan?>()))
                .Throws(new NotFoundException($"Book with ID {bookId} not found."));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _bookService.GetBookForIdAsync(bookId));
        }
    }
}