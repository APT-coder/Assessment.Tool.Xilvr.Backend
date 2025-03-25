namespace Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;

/// <summary>
/// Represents a role within the system.
/// </summary>
public class Role
{
    /// <summary>
    /// Gets or sets the unique identifier of the role.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the description of the role.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the role is a system role.
    /// </summary>
    public bool IsSystemRole { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the role is a default role.
    /// </summary>
    public bool IsDefaultRole { get; set; }

    /// <summary>
    /// Gets or sets the internal name of the role
    /// </summary>
    public string RoleInternalName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the role permissions
    /// </summary>
    public ICollection<RolePermission>? RolePermissions { get; set; }
}
