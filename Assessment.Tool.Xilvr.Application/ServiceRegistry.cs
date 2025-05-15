using Assessment.Tool.Xilvr.Application.Services;
using Assessment.Tool.Xilvr.Base.Services.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
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
        services.AddScoped<TokenService>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddCookie("ExternalCookies")
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidateIssuerSigningKey = true
            };
        })
        .AddGoogle("Google", options =>
        {
            options.SignInScheme = "ExternalCookies";
            options.ClientId = configuration["Authentication:Google:ClientId"];
            options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            options.CallbackPath = "/signin-google";
        })
        .AddMicrosoftAccount("Microsoft", options =>
        {
            options.SignInScheme = "ExternalCookies";
            options.ClientId = configuration["Authentication:Microsoft:ClientId"];
            options.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
            options.CallbackPath = "/signin-microsoft";
        });
    }
}
