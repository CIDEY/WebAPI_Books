# WebAPI_Books
![WebAPI_Books Logo](https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRuBQre0NeC1be_jIXcKP2uWKDXwXRhhgg5Tg&s)

## Description
**WebAPI_Books** — is a WebAPI project on ASP.NET Core that provides functionality for managing books, authors, and genres.

## Features
- CRUD operations for books, authors and genres
- Pagination, filtering and sorting of data
- JWT authorization with roles (User, Administrator)
- Caching with IMemoryCache
- API versioning
- Centralized error handling
- Input data validation
- Detailed Swagger documentation
- Action and error logging

## Technologies
- <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/e/ee/.NET_Core_Logo.svg/2048px-.NET_Core_Logo.svg.png" alt="ASP.NET Core" width="25" height="25"> ASP.NET Core 6.0
- <img src="https://wiki.postgresql.org/images/a/a4/PostgreSQL_logo.3colors.svg" alt="PostgreSQL" width="25" height="25"> PostgreSQL for database management
- <img src="https://static.gunnarpeipman.com/wp-content/uploads/2020/09/dotnet-featured.png" alt="Entity Framework" width="25" height="25"> Entity Framework to communicate with the database
- <img src="https://spng.hippopng.com/20190729/wgx/kisspng-smiley-green-text-messaging-meter-professional-odoo-rest-api-odoo-apps-5d3f7a46db7da5.509542591564441158899.jpg" alt="Swagger" width="25" height="25"> Swagger for API testing

## Scheme
![API circuit design](https://github.com/CIDEY/WebAPI_Books/blob/main/schema.png)

## Installation and startup
1. Make sure you have the .NET 6.0 SDK installed.
2. Clone the repository:
```bash
git clone https://github.com/CIDEY/WebAPI_Books.git
cd WebAPI_Books
```
3. Re-establish dependencies:
```bash
dotnet restore
```
4. Update the database connection string in `appsettings.json`.
5. Apply the migrations to create the database:
```bash
dotnet ef database update
```

## Getting Started
To launch the application, execute:
```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

## Authorization
The API uses JWT tokens for authorization. To obtain a token you need to:

1. Register a user via POST /api/Auth/register
2. Get token via POST /api/Auth/login
3. Use the received token in the header Authorization: Bearer {token}

## API Endpoints
### Books
- GET /api/v1/books - Get all books (with pagination and filtering)
- GET /api/v1/books/{id} - Get the book by ID
- POST /api/v1/books - Add a new book
- PUT /api/v1/books/{id} - Update book
- DELETE /api/v1/books/{id} - Delete a book

### Authors
- GET /api/v1/authors - Get all the authors
- GET /api/v1/authors/{id} - Get author by ID
- POST /api/v1/authors - Add a new author
- PUT /api/v1/authors/{id} - Update the author
- DELETE /api/v1/authors/{id} - Delete the author

### Жанры
- GET /api/v1/genres - Get all genres
- GET /api/v1/genres/{id} - Get genre by ID
- POST /api/v1/genres - Add a new genre
- PUT /api/v1/genres/{id} - Update the genre
- DELETE /api/v1/genres/{id} - Delete genre

### API Documentation
Once the application is running, the Swagger documentation will be available at:
`https://localhost:5001/swagger`.

### Error handling
The API uses centralised error handling and returns structured JSON responses with error information:
```json
{
    "success": false,
    "message": "Error description",
    "errorCode": "ERROR_CODE",
    "traceId": "Tracking ID",
    "errors": [] // Additional details during validation
}
```

### Caching
The API uses built-in caching to optimize performance. The cache is automatically invalidated when data changes.

### Validation
Input validation is implemented at the model level using validation attributes, and at the service level for more complex checks.

### Logging
Logging is configured in the project using the built-in ASP.NET Core logging provider. Logs are output to the console and debug window.

### Contribution
Pull-requests are welcome. For major changes, please open an issue first to discuss proposed changes.

### License
[MIT](https://choosealicense.com/licenses/mit/)
