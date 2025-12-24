

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentAPI.Application.Interfaces;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Enums;
using StudentAPI.Infrastructure.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAPI.Infrastructure.Services
{
    public class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username.ToString()),
                new(ClaimTypes.Role, user.Role),
            };

            foreach (var permission in user.Permissions)
            {
                claims.Add(new Claim("Permission", ((Permission)permission.PermissionId).ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var tokenDescripter = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Value.Lifetime),
                Issuer = jwtOptions.Value.Issuer,
                Audience = jwtOptions.Value.Audience,
                SigningCredentials = creds
            };

            var securityToken = tokenHandler.CreateToken(tokenDescripter);
            return tokenHandler.WriteToken(securityToken); // Access token
        }
    }
}
