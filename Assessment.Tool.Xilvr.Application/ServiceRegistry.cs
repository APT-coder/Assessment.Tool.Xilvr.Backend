using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Services;
using Assessment.Tool.Xilvr.Base.Services.Extensions;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Assessment.Tool.Xilvr.Application;

/// <summary>
/// Defines the <see cref="ServiceRegistry" />.
/// </summary>
public static class ServiceRegistry
{
    /// <summary>
    /// Add application services
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = new Assembly[] { Assembly.GetExecutingAssembly() };
        services.AddRequestHandlingServicesWithNoTransaction(assemblies);

        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddScoped<ITokenService, TokenService>();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"])),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    {
                        context.Token = authHeader.Substring("Bearer ".Length).Trim();
                        Console.WriteLine($"[JWT] OnMessageReceived Token set to: '{context.Token}'");
                    }
                    else
                    {
                        Console.WriteLine("[JWT] OnMessageReceived no valid Bearer token found.");
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"[JWT] Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("[JWT] Token successfully validated.");
                    return Task.CompletedTask;
                }
            };
        })
        .AddCookie("ExternalCookies")
        .AddGoogle("Google", options =>
        {
            options.SignInScheme = "ExternalCookies";
            options.ClientId = configuration["Authentication:Google:ClientId"];
            options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            options.CallbackPath = "/signin-google";
            options.Scope.Add("profile");
            options.Scope.Add("email");

            options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
            options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
            options.ClaimActions.MapJsonKey("picture", "picture");
        })
        .AddMicrosoftAccount("Microsoft", options =>
        {
            options.SignInScheme = "ExternalCookies";
            options.ClientId = configuration["Authentication:Microsoft:ClientId"];
            options.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
            options.CallbackPath = "/signin-microsoft";

            options.Scope.Add("openid");
            options.Scope.Add("email");
            options.Scope.Add("profile");
            options.Scope.Add("User.Read");

            options.SaveTokens = true;

            options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
            options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        });
    }
}
