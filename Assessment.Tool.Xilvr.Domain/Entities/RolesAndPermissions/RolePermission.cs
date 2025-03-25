namespace Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;

/// <summary>
/// Represents the mapping between a role and its associated permissions.
/// </summary>
public class RolePermission
{
    /// <summary>
    /// Gets or sets the unique identifier for the role permission.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the role.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the role associated with this role permission.
    /// </summary>
    public Role Role { get; set; } = default!;

    /// <summary>
    /// Gets or sets the identifier of the permission.
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// Gets or sets the permission associated with this role permission.
    /// </summary>
    public Permission Permission { get; set; } = default!;
}
