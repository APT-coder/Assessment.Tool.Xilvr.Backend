namespace Assessment.Tool.Xilvr.Domain.SharedKernel;

/// <summary>
/// Defines the <see cref="UserStatus" />.
/// </summary>
public class UserStatus
{
    /// <summary>
    /// Initialize empty constructor
    /// </summary>
    private UserStatus()
    {

    }

    /// <summary>
    /// Constructor for user status
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userStatus"></param>
    /// <param name="isActive"></param>
    private UserStatus(short id, UserStatusValues userStatus, bool isActive)
    {
        Id = id;
        Status = userStatus;
        IsActive = isActive;
    }

    /// <summary>
    /// Specifies user status id
    /// </summary>
    public short Id { get; private set; }

    /// <summary>
    /// Specifies user status
    /// </summary>
    public UserStatusValues Status { get; private set; } = default!;

    /// <summary>
    /// Specifies active status
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Create user status
    /// </summary>
    /// <param name="Status"></param>
    /// <returns></returns>
    public static UserStatus SetFrom(UserStatusValues Status)
    {
        return new UserStatus((short)Status, Status, true);
    }
}
