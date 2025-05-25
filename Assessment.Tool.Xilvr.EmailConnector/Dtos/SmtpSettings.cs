namespace Assessment.Tool.Xilvr.EmailConnector.Dtos;

/// <summary>
/// Defines the <see cref="SmtpSettings" />.
/// </summary>
public class SmtpSettings
{
    /// <summary>
    /// Gets or sets host
    /// </summary>
    public string Host { get; set; } = default!;

    /// <summary>
    /// Gets or sets port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets username
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// Gets or sets password
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// Gets or sets from email
    /// </summary>
    public string FromEmail { get; set; } = default!;

    /// <summary>
    /// Gets or sets from name
    /// </summary>
    public string FromName { get; set; } = default!;
}
