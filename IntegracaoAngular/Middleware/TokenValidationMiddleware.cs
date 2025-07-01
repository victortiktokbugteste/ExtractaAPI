using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace IntegracaoAngular.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public TokenValidationMiddleware(RequestDelegate next, ITokenBlacklistService tokenBlacklistService)
        {
            _next = next;
            _tokenBlacklistService = tokenBlacklistService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verifica se há um token de autorização
            string authHeader = context.Request.Headers["Authorization"];
            
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();
                
                // Verifica se o token está na blacklist
                if (_tokenBlacklistService.IsBlacklisted(token))
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Token inválido ou expirado");
                    return;
                }
            }

            // Continua a execução do pipeline
            await _next(context);
        }
    }
} 