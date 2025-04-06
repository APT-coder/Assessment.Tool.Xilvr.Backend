using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Assessment.Tool.Xilvr.Application.Services;

public class AuthService
{
    /// <summary>
    /// application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// HttpContext accessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
    }

    public JwtSecurityToken ReadJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(token);
    }

    public object IsTokenExpiredOrReturnToken()
    {
        string _token = string.Empty;

        if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var value))
        {
            string bearer = value.FirstOrDefault();
            if (!string.IsNullOrEmpty(bearer) && bearer.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                _token = bearer.Substring("Bearer ".Length);
            }
        }

        Ensure.IsNotNull(_token, "authorizationToken");

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(_token);
        bool isExpired = jwtToken.ValidTo < DateTime.UtcNow;

        return isExpired ? true : _token;
    }

    public string GetJwtToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenStatus = IsTokenExpiredOrReturnToken();

        if(tokenStatus is string _token)
        {
            return (string)tokenStatus;
        }

        var claims = new[]
        {
                new Claim("upn", email),
                new Claim("app_displayname", "Xilvr"),
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("HelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorldHelloWorld"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static string GenerateOtp(int length)
    {
        int min = (int)Math.Pow(10, length - 1);
        int max = (int)Math.Pow(10, length) - 1;
        Random generator = new Random();
        var otp = generator.Next(min, max).ToString();
        return otp;
    }

    public async Task<bool> DoesUserExist(string email)
    {
        var isUserExist = await _dbContext.Users.AnyAsync(u => u.Email.EmailId == email);
        return isUserExist;
    }

    public async Task<Employee> GetUserIdAndEmployeeId(string email)
    {
        var employee = await _dbContext.Employees
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.User.Email.EmailId.Equals(email));
        return employee;
    }

    public string GetEmailId()
    {
        string text = string.Empty;
        if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var value))
        {
            string text2 = value.FirstOrDefault();
            if (!string.IsNullOrEmpty(text2) && text2.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                text = text2.Substring("Bearer ".Length);
            }
        }

        Ensure.IsNotNull(text, "authorizationToken");
        string value2 = new JwtSecurityTokenHandler().ReadJwtToken(text).Claims.First((Claim claim) => claim.Type == "upn").Value;
        Ensure.IsNotNull(value2, "User id not available in the identity claims");
        return value2;
    }
    
    public async Task<bool> Login(string email, string password)
    {

    }
}
