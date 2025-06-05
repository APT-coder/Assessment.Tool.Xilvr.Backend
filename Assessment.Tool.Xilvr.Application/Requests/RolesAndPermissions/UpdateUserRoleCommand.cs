using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Entities.RolesAndPermissions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.RolesAndPermissions;

/// <summary>
/// Query class for UpdateUserRoleCommand
/// </summary>
public class UpdateUserRoleCommand : IQuery<ApiResponse<bool>>
{
    public long UserId { get; set; }

    public List<string> Roles { get; set; } = default!;
}

/// <summary>
/// Handler class for GetScheduledAssessmentAnswerByIdQuery
/// </summary>
public class UpdateUserRoleCommandHandler : IQueryHandler<UpdateUserRoleCommand, ApiResponse<bool>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token Service
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    /// RolesAndPermissions Service
    /// </summary>
    private readonly IRolesAndPermissionsService _rolesAndPermissionsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserRoleCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public UpdateUserRoleCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IRolesAndPermissionsService rolesAndPermissionsService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(rolesAndPermissionsService, nameof(rolesAndPermissionsService));
        _rolesAndPermissionsService = rolesAndPermissionsService;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<bool>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new XilvrException(ExceptionCode.BadRequest, Constants.NO_DATA);
        }

        try
        {
            var RolesList = new List<Role>();
            foreach (var role in request.Roles)
            {
                var roleObject = await _rolesAndPermissionsService.GetRole(role, cancellationToken);
                RolesList.Add(roleObject);
            }
            await _rolesAndPermissionsService.UpdateUserRole(RolesList, request.UserId, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new XilvrException(ExceptionCode.InternalServerError, ex.Message, ex);
        }
        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
