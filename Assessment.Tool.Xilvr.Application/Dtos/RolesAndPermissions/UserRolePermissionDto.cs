namespace Assessment.Tool.Xilvr.Application.Dtos.RolesAndPermissions;

/// <summary>
/// Represents a dto for user roles and permissions.
/// </summary>
public class UserRolePermissionDto
{
    /// <summary>
    /// Gets or sets the collection of roles associated with the user.
    /// </summary>
    public IEnumerable<string> Roles { get; set; }

    /// <summary>
    /// Gets or sets the collection of permissions associated with the user.
    /// </summary>
    public IEnumerable<string> Permissions { get; set; }
}
