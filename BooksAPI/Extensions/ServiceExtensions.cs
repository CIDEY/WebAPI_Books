using BooksAPI.DBContextAPI;
using BooksAPI.Service.Interface;
using BooksAPI.Service;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IGenreService, GenreService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddDbContext<BooksApiDb>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnection")));

            return services;
        }
    }
}
