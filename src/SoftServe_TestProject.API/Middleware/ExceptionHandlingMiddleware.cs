using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SoftServe_TestProject.API.Responses;
using System;
using System.Net;

namespace SoftServe_TestProject.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            Log.Information(ex, "An unexpected error occurred.");

            ExceptionResponse response = ex switch
            {
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The requested resource was not found."),
                UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized access."),
                ValidationException validationEx => new ExceptionResponse(HttpStatusCode.BadRequest,
                    string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage))),
                DbUpdateException _ => new ExceptionResponse(HttpStatusCode.Conflict, "A database update error occurred."),
                ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, ex.Message),
                ArgumentException _ or ArgumentNullException _ => new ExceptionResponse(HttpStatusCode.BadRequest, ex.Message),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please try again later.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
