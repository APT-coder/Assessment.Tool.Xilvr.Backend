using Assessment.Tool.Xilvr.EmailConnector.Contracts;
using Assessment.Tool.Xilvr.EmailConnector.Dtos;
using Assessment.Tool.Xilvr.EmailConnector.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;

namespace Assessment.Tool.Xilvr.EmailConnector;

/// <summary>
/// Defines the <see cref="ServiceRegistry" />.
/// </summary>
public static class ServiceRegistry
{
    /// <summary>
    /// Add email services
    /// </summary>
    /// <param name="services"></param>
    public static void AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();

        var smtpSettings = new SmtpSettings
        {
            Host = configuration["SmtpSettings:Host"],
            Port = int.Parse(configuration["SmtpSettings:Port"]),
            UserName = configuration["SmtpSettings:UserName"],
            Password = configuration["SmtpSettings:Password"],
            FromEmail = configuration["SmtpSettings:FromEmail"],
            FromName = configuration["SmtpSettings:FromName"]
        };

        services.AddFluentEmail(smtpSettings.FromEmail, smtpSettings.FromName)
            .AddRazorRenderer()
            .AddSmtpSender(new SmtpClient(smtpSettings.Host)
            {
                Port = smtpSettings.Port,
                Credentials = new System.Net.NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
                EnableSsl = true,


                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            });
    }
}
