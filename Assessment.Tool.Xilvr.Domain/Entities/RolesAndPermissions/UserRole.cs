using Assessment.Tool.Xilvr.Domain.SharedKernel;

namespace Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;

/// <summary>
/// Represents the association between a user and a role in the system.
/// </summary>
public class UserRole
{
    /// <summary>
    /// Gets or sets the unique identifier for the user role.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user associated with this user role.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the user associated with this user role.
    /// </summary>
    /// <remarks>
    /// This property represents the user who is assigned the role.
    /// </remarks>
    public User User { get; set; } = default!;

    /// <summary>
    /// Gets or sets the ID of the role associated with this user role.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the role associated with this user role.
    /// </summary>
    /// <remarks>
    /// This property represents the role that the user is assigned to.
    /// </remarks>
    public Role Role { get; set; } = default!;

    /// <summary>
    /// Gets or sets sort order
    /// </summary>
    public int SortOrder { get; set; }
}
