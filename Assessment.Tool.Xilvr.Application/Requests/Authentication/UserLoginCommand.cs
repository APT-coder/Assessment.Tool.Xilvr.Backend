using Assessment.Tool.Xilvr.Application.Services;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Authentication;

/// <summary>
/// Command for login
/// </summary>
public class UserLoginCommand : IQuery<ApiResponse<string>>
{
    public string? Email { get; set; }

    public string? Password { get; set; }
}

/// <summary>
/// Handler class for UserLoginCommand
/// </summary>
public class UserLoginCommandHandler : IQueryHandler<UserLoginCommand, ApiResponse<string>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Authentication service
    /// </summary>
    private readonly TokenService _tokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLoginCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public UserLoginCommandHandler(IApplicationDbContext dbContext, TokenService tokenService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(_tokenService, nameof(tokenService));
        _tokenService = tokenService;
    }

    /// <summary>
    /// The hande method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<string>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.EmailId == request.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return new ApiResponse<string>(null, Constants.INVALID_CREDENTIAL);
        }

        var token = _tokenService.GenerateToken(user);
        return new ApiResponse<string>(token, Constants.SUCCESS_MSG);
    }
}
