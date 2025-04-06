using Assessment.Tool.Xilvr.Domain.SharedKernel;

namespace Assessment.Tool.Xilvr.Application.Dtos.Users;

/// <summary>
/// Defines user info dto
/// </summary>
/// <seealso cref="UserInfoDto" />
public class UserInfoDto
{
    /// <summary>
    /// Specifies user id
    /// </summary>
    public long UserId { get; set; } = default!;

    /// <summary>
    /// Specifies employee id
    /// </summary>
    public long EmployeeId {  get; set; } = default!;

    /// <summary>
    /// Specifies the first name
    /// </summary>
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Specifies the last name
    /// </summary>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Specifies email id
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Specifies profile image url
    /// </summary>
    public string? ProfileImageUrl { get; set; } = default!;

    /// <summary>
    /// Specifies profile image url
    /// </summary>
    public string Phone { get; set; } = default!;

    /// <summary>
    /// Specifies designation
    /// </summary>
    public string Designation {  get; set; } = default!;

    /// <summary>
    /// Specifies list of batchIds
    /// </summary>
    public List<string> Batches { get; set; } = default!;
}
