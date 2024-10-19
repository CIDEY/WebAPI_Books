# WebAPI_Books
![WebAPI_Books Logo](https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRuBQre0NeC1be_jIXcKP2uWKDXwXRhhgg5Tg&s)

## Description
**WebAPI_Books** — is a WebAPI project on ASP.NET Core that provides functionality for managing books, authors, and genres.

## Features
- CRUD operations for books, authors and genres
- Input validation
- Exception handling
- Using Entity Framework Core with PostgreSQL
- Dependency Injection
- Logging

## Technologies
- <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/e/ee/.NET_Core_Logo.svg/2048px-.NET_Core_Logo.svg.png" alt="ASP.NET Core" width="25" height="25"> ASP.NET Core 6.0
- <img src="https://wiki.postgresql.org/images/a/a4/PostgreSQL_logo.3colors.svg" alt="PostgreSQL" width="25" height="25"> PostgreSQL for database management
- <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS6q-GXwQdPXi7X-f2zs0X1BJArGFvhE9keyw&usqp=CAU" alt="Entity Framework" width="25" height="25"> Entity Framework to communicate with the database
- <img src="https://static1.smartbear.co/swagger/media/assets/images/swagger_logo.svg" alt="Swagger" width="25" height="25"> Swagger for API testing

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

## API Endpoints
- GET /api/books/all - Get all books
- GET /api/books/{id} - Get book by ID
- POST /api/books - Add a new book
- DELETE /api/books/{id} - Delete a book

Similar endpoints are available for authors (/api/author) and genres (/api/genres).

### API Documentation
Once the application is running, the Swagger documentation will be available at:
`https://localhost:5001/swagger`.

### Error handling
The project uses centralized error handling via middleware. All exceptions are handled and returned as a structured JSON response.

### Validation
Input validation is implemented at the model level using validation attributes, and at the service level for more complex checks.

### Logging
Logging is configured in the project using the built-in ASP.NET Core logging provider. Logs are output to the console and debug window.

### Contribution
Pull-requests are welcome. For major changes, please open an issue first to discuss proposed changes.

### License
[MIT](https://choosealicense.com/licenses/mit/)
