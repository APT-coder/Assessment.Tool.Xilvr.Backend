using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.Authentication;

/// <summary>
/// Query class for refresh token command
/// </summary>
public class RefreshTokenCommand : IQuery<ApiResponse<string>>
{
    public string Token { get; set; } = string.Empty;
}

/// <summary>
/// Handler for RefreshTokenCommand
/// </summary>
public class RefreshTokenCommandHandler : IQueryHandler<RefreshTokenCommand, ApiResponse<string>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token service
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Constructor for RefreshTokenCommandHandler
    /// </summary>
    public RefreshTokenCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
    }

    /// <summary>
    /// Handle method
    /// </summary>
    public async Task<ApiResponse<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
        if (principal == null)
        {
            throw new Exception(ExceptionCode.UnauthorizedAccess.ToString());
        }

        var userId = principal.FindFirst("email")?.Value;
        if (userId == null)
        {
            throw new Exception(ExceptionCode.UnauthorizedAccess.ToString());
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.EmailId == userId, cancellationToken);
        if (user == null)
        {
            throw new Exception(ExceptionCode.UnauthorizedAccess.ToString());
        }

        var newToken = _tokenService.GenerateToken(user);
        return new ApiResponse<string>(newToken, Constants.SUCCESS_MSG);
    }
}
