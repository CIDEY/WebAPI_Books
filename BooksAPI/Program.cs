using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BooksAPI.DBContextAPI;
using Microsoft.Extensions;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Configuration;
using BooksAPI.Service.Interface;
using BooksAPI.Service;
using BooksAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IAuthorService, AuthorService>();

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add DbContext
builder.Services.AddDbContext<BooksApiDb>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
