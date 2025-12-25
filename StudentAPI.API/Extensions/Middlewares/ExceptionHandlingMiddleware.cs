using System.Net;

namespace StudentAPI.API.Extensions.Middlewares
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            object errors = null;

            if(exception is FluentValidation.ValidationException validationException)
            {
                statusCode = HttpStatusCode.BadRequest;

                errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                    );
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = "Validation Failed",
                Errors = errors ?? exception.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
