using Assessment.Tool.Xilvr.EmailConnector.Contracts;
using Assessment.Tool.Xilvr.EmailConnector.Dtos;
using FluentEmail.Core.Models;
using FluentEmail.Core;
using Assessment.Tool.Xilvr.Base.Helpers;

namespace Assessment.Tool.Xilvr.EmailConnector.Services;

/// <summary>
/// Email Service
/// </summary>
public class EmailService : IEmailService
{
    /// <summary>
    /// Fluent Email
    /// </summary>
    private readonly IFluentEmail _fluentEmail;

    /// <summary>
    /// Constructor for Email service
    /// </summary>
    public EmailService(IFluentEmail fluentEmail)
    {
        Ensure.IsNotNull(fluentEmail, nameof(fluentEmail));
        _fluentEmail = fluentEmail;
    }

    public async Task<SendResponse> SendEmailAsync(string toEmail, string subject, string body, List<AttachmentDto> attachments = null)
    {
        var email = _fluentEmail
            .To(toEmail)
            .Subject(subject)
            .Body(body);

        if (attachments != null)
        {
            foreach (var attachment in attachments)
            {
                var fileContent = Convert.FromBase64String(attachment.FileContent);
                var attachmentStream = new MemoryStream(fileContent);

                email = email.Attach(new Attachment
                {
                    Filename = attachment.FileName,
                    Data = attachmentStream,
                    ContentType = "application/octet-stream"
                });
            }
        }

        return await email.SendAsync();
    }
}