using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClubSync.Api.Database;
using Microsoft.IdentityModel.Tokens;

namespace ClubSync.Api.Features.Users
{
    public class TokenProvider
    {
        private readonly byte[] _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        public TokenProvider(IConfiguration config)
        {
            _secretKey = Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]!);
            _issuer = config["JwtSettings:Issuer"]!;
            _audience = config["JwtSettings:Audience"]!;
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>{
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}