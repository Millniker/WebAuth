using Pathnostics.Web.Services.Interface;

namespace Pathnostics.Web.Services;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class JwtService:ITokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;

    public JwtService()
    {
        _secretKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        _issuer = "issuer";
    }
    public string GenerateToken(Guid userId, string email)
    {
        var key = Convert.FromBase64String(_secretKey);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("email", email) 
        };

        var token = new JwtSecurityToken(
            _issuer,
            _issuer,
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public JwtPayload ParseToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new ArgumentException("Invalid JWT token");
            }

            var payload = jwtToken.Payload;
            return payload;
        }
        catch (Exception ex)
        {
            // Обработка ошибок парсинга токена
            throw new Exception("Failed to parse JWT token", ex);
        }
    }
}