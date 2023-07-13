using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthArmor.Samples.AspNetCore.WebApi.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthArmor.Samples.AspNetCore.WebApi.Services;

public class AuthenticationTokenService
{
    private readonly AuthenticationTokenConfiguration _configuration;

    public AuthenticationTokenService(AuthenticationTokenConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetToken(string userId, string username)
    {
        var userIdentity = new ClaimsIdentity(new Claim[]
        {
            new (JwtRegisteredClaimNames.Sub, userId),
            new (JwtRegisteredClaimNames.NameId, username)
        });

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = userIdentity,
            Expires = DateTime.UtcNow.AddSeconds(_configuration.ValidityPeriod),
            Issuer = _configuration.Issuer,
            Audience = _configuration.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.IssuerSigningKey)),
                SecurityAlgorithms.HmacSha512Signature
            )
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string serializedToken = tokenHandler.WriteToken(token);

        return serializedToken;
    }
}
