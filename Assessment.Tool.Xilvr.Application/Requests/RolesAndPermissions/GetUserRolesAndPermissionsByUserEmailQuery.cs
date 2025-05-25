using Assessment.Tool.Xilvr.Application.Dtos.RolesAndPermissions;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.RolesAndPermissions;

/// <summary>
/// Represents a query to retrieve user roles and permissions by user email id.
/// </summary>
public class GetUserRolesAndPermissionsByUserEmailQuery : IQuery<ApiResponse<UserRolePermissionDto>>
{
    /// <summary>
    /// The user email address
    /// </summary>
    public string Email { get; set; }
}

/// <summary>
/// Handles the request to retrieve user roles and permissions.
/// </summary>
public class GetUserRolesAndPermissionsByUserEmailQueryHandler
    : IQueryHandler<GetUserRolesAndPermissionsByUserEmailQuery, ApiResponse<UserRolePermissionDto>>
{
    /// <summary>
    /// Represents the user dbcontext.
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserRolesAndPermissionsByUserEmailQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user db context.</param>
    public GetUserRolesAndPermissionsByUserEmailQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Handles the request to retrieve user roles and permissions.
    /// </summary>
    /// <param name="request">The request containing the query parameters.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>An API response containing user roles and permissions.</returns>
    /// <exception cref="XilvrException">Thrown when the user is not found.</exception>
    public async Task<ApiResponse<UserRolePermissionDto>> Handle(
    GetUserRolesAndPermissionsByUserEmailQuery request,
    CancellationToken cancellationToken)
    {
        // Retrieve the user with their roles and permissions from the database not including internal permissions.
        var userWithRolesAndPermissions = await _dbContext.Users
            .Where(x => x.Email.EmailId == request.Email && x.IsActiveUser())
            .Select(x => new
            {
                Roles = x.UserRoles.Select(ur => ur.Role.Name),
                Permissions = x.UserRoles
                    .SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.PermissionKey))
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception(ExceptionCode.BadRequest.ToString());

        // Extract and process roles and permissions.
        var roles = userWithRolesAndPermissions.Roles.ToList();
        var permissions = userWithRolesAndPermissions.Permissions
            .Distinct()
            .OrderBy(p => p)
            .ToList();

        // Return the API response containing user roles and permissions.
        return new ApiResponse<UserRolePermissionDto>(new UserRolePermissionDto
        {
            Permissions = permissions,
            Roles = roles
        }, Constants.SUCCESS_MSG);
    }
}
