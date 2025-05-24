using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assessment.Tool.Xilvr.Application.Services;

public class TokenService : ITokenService
{
    /// <summary>
    /// IConfiguration
    /// </summary>
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        Ensure.IsNotNull(config, nameof(config));
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.EmailId ?? ""),
            new Claim("provider", user.UserProvider.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = _config["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = _config["Jwt:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    public string TryGetEmailFromToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = _config["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = _config["Jwt:Issuer"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            return email;
        }
        catch
        {
            return null;
        }
    }
}
