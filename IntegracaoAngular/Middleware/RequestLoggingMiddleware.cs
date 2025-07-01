using Application.Dtos;
using Application.Interfaces;
using System.Net;

namespace IntegracaoAngular.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogService _logService;

        public RequestLoggingMiddleware(RequestDelegate next, ILogService logService)
        {
            _next = next;
            _logService = logService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                var request = context.Request;
                // Log do erro de validação
                var log = new LogDto
                {
                    CreateDate = DateTime.Now,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Method = request.Method,
                    Trace = request.Path,
                    Exception = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        errors = ex.Errors.Select(e => new
                        {
                            field = e.PropertyName,
                            message = e.ErrorMessage
                        })
                    })
                };

                await _logService.RegistrarAsync(log);

                // Retorna resposta de erro de validação
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    errors = ex.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    })
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                var request = context.Request;
                var log = new LogDto
                {
                    CreateDate = DateTime.Now,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Method = request.Method,
                    Trace = request.Path,
                    Exception = ex.ToString()
                };

                await _logService.RegistrarAsync(log);

                // Retorna resposta de erro genérico
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Ocorreu um erro interno no servidor"
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
