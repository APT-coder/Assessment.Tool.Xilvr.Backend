using Assessment.Tool.Xilvr.Application.Dtos.Users;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Users;

/// <summary>
/// Query class for getting user info
/// </summary>
public class GetUserInfoQuery : IQuery<ApiResponse<UserInfoDto>>
{
    public string? Email { get; set; }
}

/// <summary>
/// Handler class for GetUserInfoQuery
/// </summary>
public class GetUserInfoQueryHandler : IQueryHandler<GetUserInfoQuery, ApiResponse<UserInfoDto>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserInfoQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetUserInfoQueryHandler(IApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    /// <summary>
    /// The hande method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<UserInfoDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var email = request.Email ?? "aswin@gmail.com";

        var user = await _dbContext.Employees
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.User.Email.EmailId == email, cancellationToken);

        if (user == null)
        {
            throw new Exception(ExceptionCode.BadRequest.ToString());
        }

        var batchNames = await _dbContext.Batches
            .Where(b => user.BatchIds.Contains(b.Id.ToString()))
            .Select(b => b.Name)
            .ToListAsync(cancellationToken);

        var response = new UserInfoDto
        {
            UserId = user.UserId,
            EmployeeId = user.Id,
            FirstName = user.User.FirstName,
            LastName = user.User.LastName,
            Email = user.User.Email.EmailId,
            Phone = user.User.Phone,
            ProfileImageUrl = user.User.ProfileImageUrl,
            Designation = user.Designation,
            Batches = batchNames
        };

        return new ApiResponse<UserInfoDto>(response, Constants.SUCCESS_MSG);
    }
}
