using System.IdentityModel.Tokens.Jwt;

namespace Pathnostics.Web.Services.Interface;

public interface ITokenService
{
    public string GenerateToken(Guid userId, string email);
    public JwtPayload ParseToken(string token);

}