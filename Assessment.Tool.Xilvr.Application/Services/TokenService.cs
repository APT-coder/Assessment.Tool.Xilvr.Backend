using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assessment.Tool.Xilvr.Application.Services;

/// <summary>
/// Token Service
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    /// IConfiguration
    /// </summary>
    private readonly IConfiguration _config;

    /// <summary>
    /// IHttpContextAccessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        Ensure.IsNotNull(config, nameof(config));
        _config = config;
        Ensure.IsNotNull(httpContextAccessor, nameof(httpContextAccessor));
        _httpContextAccessor = httpContextAccessor;
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

    public ClaimsPrincipal GetPrincipalFromExpiredToken()
    {
        var token = GetTokenFromAuthorizationHeader();
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

    public string TryGetEmailFromToken()
    {
        var token = GetTokenFromAuthorizationHeader();
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

    /// <summary>
    /// Extracts the Bearer token from the current HttpContext's Authorization header if present.
    /// Returns null if no valid Bearer token found.
    /// </summary>
    public string? GetTokenFromAuthorizationHeader()
    {
        var request = _httpContextAccessor.HttpContext?.Request;

        if (request == null)
        {
            return null;
        }

        if (request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var bearerToken = authHeader.ToString();
            if (!string.IsNullOrEmpty(bearerToken) && bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return bearerToken.Substring("Bearer ".Length).Trim();
            }
        }
        return null;
    }

    public string GenerateOtp(int length)
    {
        int min = (int)Math.Pow(10, length - 1);
        int max = (int)Math.Pow(10, length) - 1;
        Random generator = new Random();
        var otp = generator.Next(min, max).ToString();
        return otp;
    }
}
