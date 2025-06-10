namespace Assessment.Tool.Xilvr.Domain.SharedKernel;

/// <summary>
/// Defines the <see cref="UserStatusValues" />.
/// </summary>
public enum UserStatusValues
{
    /// <summary>
    /// Specifies pending profile completion status - user has registered but needs to complete their profile
    /// </summary>
    PendingProfileCompletion = 1,

    /// <summary>
    /// Specifies pending approval status - user has completed profile but is awaiting approval
    /// </summary>
    PendingApproval = 2,

    /// <summary>
    /// Specifies active status - user is approved and fully active
    /// </summary>
    Active = 3,

    /// <summary>
    /// Specifies inactive status - user account is deactivated
    /// </summary>
    InActive = 4,
}
