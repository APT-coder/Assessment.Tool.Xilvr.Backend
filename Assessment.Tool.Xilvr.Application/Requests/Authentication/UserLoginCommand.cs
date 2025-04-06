using Assessment.Tool.Xilvr.Application.Dtos.Authentication;
using Assessment.Tool.Xilvr.Application.Dtos.Users;
using Assessment.Tool.Xilvr.Application.Requests.Users;
using Assessment.Tool.Xilvr.Application.Services;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Authentication;

/// <summary>
/// Command for login
/// </summary>
public class UserLoginCommand : IQuery<ApiResponse<LoginResponseDto>>
{
    public string? Token { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }
}

/// <summary>
/// Handler class for UserLoginCommand
/// </summary>
public class UserLoginCommandHandler : IQueryHandler<UserLoginCommand, ApiResponse<LoginResponseDto>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Authentication service
    /// </summary>
    private readonly AuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLoginCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public UserLoginCommandHandler(IApplicationDbContext dbContext, AuthService authService)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
        ArgumentNullException.ThrowIfNull(authService);
        _authService = authService;
    }

    /// <summary>
    /// The hande method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<LoginResponseDto>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        if(request.Token is not null)
        {
            var ssoEmail = _authService.GetEmailId();
            var doesUserExist = _authService.DoesUserExist(ssoEmail);
            if (doesUserExist.Result == false)
            {
                throw new Exception(ExceptionCode.UnauthorizedAccess.ToString());
            }

            var user = _authService.GetUserIdAndEmployeeId(ssoEmail);
            var response = new LoginResponseDto
            {
                UserId = user.Result.UserId,
                EmployeeId = user.Result.Id,
                EmailId = ssoEmail,
                UserStatus = user.Result.User.UserStatus.Status.ToString()
            };

            return new ApiResponse<LoginResponseDto>(response, Constants.SUCCESS_MSG);
        }

        return new ApiResponse<LoginResponseDto>(response, Constants.SUCCESS_MSG);
    }
}
