using Application.Models;
using Infrastructure.Models;
using Infrastructure.Services;
using IntegracaoAngular.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace IntegracaoAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public AuthController(IOptions<JwtSettings> jwtOptions, ITokenBlacklistService tokenBlacklistService)
        {
            _jwtSettings = jwtOptions.Value;
            _tokenBlacklistService = tokenBlacklistService;
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Realiza o login do usuário",
            Description = "Endpoint para autenticação do usuário e geração do token JWT",
            OperationId = "Login",
            Tags = new[] { "Autenticação" }
        )]
        [SwaggerResponse(200, "Login realizado com sucesso", typeof(LoginResponse))]
        [SwaggerResponse(401, "Credenciais inválidas")]
        public IActionResult Login(
            [FromBody]
            [SwaggerParameter(Description = "Dados de login do usuário", Required = true)]
            LoginModel model)
        {

            if (model.Username != "teste@gmail.com" || model.Password != "1234")
                return Unauthorized(new { message = "Credenciais inválidas" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "Administrador")
            }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new LoginResponse { Token = tokenString });
        }

        [Authorize]
        [HttpPost("logout")]
        [SwaggerOperation(
            Summary = "Realiza o logout do usuário",
            Description = "Endpoint para invalidar o token JWT atual",
            OperationId = "Logout",
            Tags = new[] { "Autenticação" }
        )]
        [SwaggerResponse(200, "Logout realizado com sucesso")]
        [SwaggerResponse(401, "Não autorizado")]
        public IActionResult Logout()
        {
            // Extrai o token do cabeçalho Authorization
            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new { message = "Token não fornecido" });
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();
            
            // Extrai a data de expiração do token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var expiryTimeUnix = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            
            if (expiryTimeUnix != null && long.TryParse(expiryTimeUnix, out long expiryTimeSeconds))
            {
                var expiryTime = DateTimeOffset.FromUnixTimeSeconds(expiryTimeSeconds).DateTime;
                
                // Adiciona o token à blacklist
                _tokenBlacklistService.BlacklistToken(token, expiryTime);
                
                return Ok(new { message = "Logout realizado com sucesso" });
            }
            
            return BadRequest(new { message = "Token inválido" });
        }
    }
}
