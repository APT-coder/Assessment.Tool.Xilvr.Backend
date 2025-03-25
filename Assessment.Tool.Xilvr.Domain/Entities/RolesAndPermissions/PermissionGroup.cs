namespace Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;

/// <summary>
/// Represents a group of permissions within the system.
/// </summary>
public class PermissionGroup
{
    /// <summary>
    /// Gets or sets the unique identifier of the permission group.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the permission group.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the permissions collection
    /// </summary>
    public ICollection<Permission> Permissions { get; set; } = default!;
}
