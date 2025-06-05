using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Services;

/// <summary>
/// RolesAndPermissions Service
/// </summary>
public class RolesAndPermissionService : IRolesAndPermissionsService
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token service
    /// </summary>
    private readonly ITokenService _tokenService;

    public RolesAndPermissionService(IApplicationDbContext dbContext, ITokenService tokenService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
    }

    public async Task UpdateUserRole(List<Role> roles, long userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, Constants.UPDATE_FAILED);
        }

        var existingUserRoles = await _dbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .ToListAsync(cancellationToken);

        _dbContext.UserRoles.RemoveRange(existingUserRoles);

        var newUserRoles = roles.Select((role, index) => new UserRole
        {
            UserId = userId,
            RoleId = role.Id,
            SortOrder = index + 1
        }).ToList();

        await _dbContext.UserRoles.AddRangeAsync(newUserRoles, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Role> GetRole(string roleInternalName, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.RoleInternalName == roleInternalName, cancellationToken);

        return role;
    }
}
