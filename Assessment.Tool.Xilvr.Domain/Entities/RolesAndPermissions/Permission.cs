namespace Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;

/// <summary>
/// Represents a permission within the system.
/// </summary>
public class Permission
{
    /// <summary>
    /// Gets or sets the unique identifier of the permission.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the permission.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the key associated with the permission.
    /// </summary>
    public string PermissionKey { get; set; } = default!;

    /// <summary>
    /// Gets or sets the permission group
    /// </summary>
    public PermissionGroup? PermissionGroup { get; set; }

    /// <summary>
    /// Gets or sets the permission group id
    /// </summary>
    public int? PermissionGroupId { get; set; }

    /// <summary>
    /// Gets or sets the role permissions
    /// </summary>
    public ICollection<RolePermission>? RolePermissions { get; set; }

    /// <summary>
    /// Gets or sets whether the permission is an internal permission
    /// </summary>
    public bool IsInternalPermission { get; set; }
}
