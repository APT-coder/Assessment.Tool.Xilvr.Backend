namespace Assessment.Tool.Xilvr.Application.Dtos.Authentication;

/// <summary>
/// Defines login response dto
/// </summary>
/// <seealso cref="LoginResponseDto" />
public class LoginResponseDto
{
    /// <summary>
    /// Specifies user id
    /// </summary>
    public long UserId { get; set; } = default!;

    /// <summary>
    /// Specifies employee id
    /// </summary>
    public long EmployeeId { get; set; } = default!;

    /// <summary>
    /// Specifies employee email
    /// </summary>
    public string EmailId { get; set; } = default!;

    /// <summary>
    /// Specifies user status
    /// </summary>
    public string UserStatus { get; set; } = default!;
}
