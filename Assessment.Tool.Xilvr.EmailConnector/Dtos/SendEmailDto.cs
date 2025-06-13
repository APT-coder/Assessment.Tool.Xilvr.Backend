using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Tool.Xilvr.EmailConnector.Dtos;

/// <summary>
/// Represents SendEMailDto
/// </summary>
public class SendEmailDto
{
    /// <summary>
    /// Gets or sets ToEmail
    /// </summary>
    public string? ToEmail { get; set; }

    /// <summary>
    /// Gets or sets Subject
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Gets or sets Body
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// Gets or sets attachments
    /// </summary>
    public List<AttachmentDto> Attachments { get; set; } = default!;
}
