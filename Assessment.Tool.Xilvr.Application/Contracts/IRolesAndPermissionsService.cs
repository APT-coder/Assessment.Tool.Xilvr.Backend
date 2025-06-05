using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;

namespace Assessment.Tool.Xilvr.Application.Contracts;

/// <summary>
/// Interface for RolesAndPermissions service.
/// </summary>
public interface IRolesAndPermissionsService
{
    /// <summary>
    /// Update user roles.
    /// </summary>
    public Task UpdateUserRole(List<Role> roles, long userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get Role by role internal name.
    /// </summary>
    public Task<Role> GetRole(string roleInternalName, CancellationToken cancellationToken);
}
