using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebApplication4.Settings;

namespace WebApplication4.Services
{
    public class TokenService : ITokenService
    {
        public string GetToken()
        {
            var handler = new JsonWebTokenHandler();
            DateTime createdAt = DateTime.UtcNow;
            DateTime expiresAt = createdAt.AddDays(7);
            return handler.CreateToken(new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, "fulviocanducci@hotmail.com"),
                    new Claim(ClaimTypes.Email, "fulviocanducci@hotmail.com")
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Secret.Bytes), SecurityAlgorithms.HmacSha256Signature),
                Expires = expiresAt
            });
        }
    }
}
