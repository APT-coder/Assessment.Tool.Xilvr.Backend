using System.Text.RegularExpressions;
using Assessment.Tool.Xilvr.Base;

namespace Assessment.Tool.Xilvr.Domain.SharedKernel;

/// <summary>
/// Defines the <see cref="Email" />.
/// </summary>
public class Email
{
    /// <summary>
    /// Initialize empty constructor
    /// </summary>
    private Email()
    {
    }

    /// <summary>
    /// Specifies email id
    /// </summary>
    public string EmailId { get; private set; } = default!;

    /// <summary>
    /// Constructor for email 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="UserDomainException"></exception>
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == string.Empty || !IsValidEmail(value))
        {
            throw new Exception(ExceptionCode.BadRequest.ToString());
        }
        EmailId = value;
    }

    /// <summary>
    /// Checks the given email is valid or not
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, DomainConstants.EMAIL_REGEX);
    }
}
