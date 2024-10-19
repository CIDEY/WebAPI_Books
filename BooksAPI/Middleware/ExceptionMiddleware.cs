using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using static BooksAPI.Middleware.Exceptions;
using ValidationException = BooksAPI.Middleware.Exceptions.ValidationException;
using BooksAPI.Middleware.Model;

namespace BooksAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorResponse
            {
                Success = false,
                TraceId = context.TraceIdentifier
            };

            string errorCategory;

            switch (exception)
            {
                case BadRequestException badRequestEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = badRequestEx.Message;
                    errorResponse.ErrorCode = "BAD_REQUEST";
                    errorCategory = "Client Error";
                    break;
                case NotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = notFoundEx.Message;
                    errorResponse.ErrorCode = "NOT_FOUND";
                    errorCategory = "Not Found Error";
                    break;
                case UnauthorizedAccessException unauthorizedEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Message = "Unauthorized access";
                    errorResponse.ErrorCode = "UNAUTHORIZED";
                    errorCategory = "Authentication Error";
                    break;
                case DbUpdateException dbUpdateEx:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "An error occurred while updating the database";
                    errorResponse.ErrorCode = "DB_UPDATE_ERROR";
                    errorCategory = "Database Error";
                    break;
                case ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = "Validation failed";
                    errorResponse.ErrorCode = "VALIDATION_ERROR";
                    errorResponse.Errors = validationEx.Errors?.Select(e => new ValidationError
                    {
                        Field = e.Field,
                        Message = e.Message
                    }).ToList();
                    errorCategory = "Validation Error";
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "An unexpected error occurred";
                    errorResponse.ErrorCode = "INTERNAL_SERVER_ERROR";
                    errorCategory = "Server Error";
                    break;
            }

            // Добавляем дополнительную информацию для разработчиков в режиме разработки
            if (_env.IsDevelopment())
            {
                errorResponse.DeveloperMessage = exception.ToString();
            }

            // Расширенное логирование
            var userId = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "Anonymous";
            var requestBody = string.Empty;

            // Прочитать тело запроса, если оно доступно
            if (context.Request.ContentLength.HasValue && context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            // Логирование расширенной информации об ошибке
            _logger.LogError(
                exception,
                "An error occurred while processing the request. " +
                "Error: {ErrorMessage}. " +
                "TraceId: {TraceId}. " +
                "ErrorCode: {ErrorCode}. " +
                "ErrorCategory: {ErrorCategory}. " +
                "StatusCode: {StatusCode}. " +
                "Path: {Path}. " +
                "Method: {Method}. " +
                "QueryString: {QueryString}. " +
                "User: {UserId}. " +
                "RequestBody: {RequestBody}",
                errorResponse.Message,
                errorResponse.TraceId,
                errorResponse.ErrorCode,
                errorCategory,
                response.StatusCode,
                context.Request.Path,
                context.Request.Method,
                context.Request.QueryString,
                userId,
                requestBody
            );

            if (exception.InnerException != null)
            {
                _logger.LogError("Inner Exception: {InnerExceptionMessage}", exception.InnerException.Message);
            }

            var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(result);

        }

    }
}