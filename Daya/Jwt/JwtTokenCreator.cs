using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Jwt
{
    public class JwtTokenCreator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenCreator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateAccessToken(IEnumerable<Claim> claims)
        {
            var mySecretKey = Encoding.UTF8.GetBytes(_configuration["AuthenticationOptions:SecretKey"]);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(mySecretKey),
                SecurityAlgorithms.HmacSha256Signature);
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["AuthenticationOptions:Issuer"],
                Audience = _configuration["AuthenticationOptions:Audience"],
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return accessToken;
        }
    }

}