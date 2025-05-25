using Assessment.Tool.Xilvr.EmailConnector.Dtos;
using FluentEmail.Core.Models;

namespace Assessment.Tool.Xilvr.EmailConnector.Contracts;

/// <summary>
/// Interface for Email Service
/// </summary>
public interface IEmailService
{
    Task<SendResponse> SendEmailAsync(string toEmail, string subject, string body, List<AttachmentDto> attachments = null);
}

