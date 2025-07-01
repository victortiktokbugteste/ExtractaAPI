﻿using Application.Dtos;
using Application.Interfaces;
using System.Net;

namespace IntegracaoAngular.Middleware
{
    public class AuthenticationLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogService _logService;

        public AuthenticationLoggingMiddleware(RequestDelegate next, ILogService logService)
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
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    var log = new LogDto
                    {
                        CreateDate = DateTime.Now,
                        StatusCode = context.Response.StatusCode,
                        Method = context.Request.Method,
                        Trace = context.Request.Path,
                        Exception = "Unauthorized: Token inválido ou não fornecido"
                    };

                    await _logService.RegistrarAsync(log);

                    // Retorna resposta de erro de validação
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        errors = new
                        {
                            field = "Login",
                            message = "Não autorizado."
                        }
                    };

                    await context.Response.WriteAsJsonAsync(response);
                }
            }
        }
    }
}
