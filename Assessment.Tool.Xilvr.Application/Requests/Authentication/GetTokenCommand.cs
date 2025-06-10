using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assessment.Tool.Xilvr.Application.Requests.Authentication;

/// <summary>
/// Query class for get token command
/// </summary>
public class GetTokenCommand : IQuery<ApiResponse<string>>
{
}

/// <summary>
/// Handler for GetTokenCommand
/// </summary>
public class GetTokenCommandHandler : IQueryHandler<GetTokenCommand, ApiResponse<string>>
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
    /// Http context accessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructor for GetTokenCommandHandler
    /// </summary>
    public GetTokenCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(httpContextAccessor, nameof(httpContextAccessor));
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Handle method
    /// </summary>
    public async Task<ApiResponse<string>> Handle(GetTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated == false)
        {
            throw new XilvrException(ExceptionCode.UnauthorizedAccess, "User not authenticated");
        }

        var email = user.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            throw new XilvrException(ExceptionCode.UnauthorizedAccess, "Email not found in token");
        }

        var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.EmailId == email, cancellationToken);
        if (dbUser == null)
        {
            throw new XilvrException(ExceptionCode.NotFound, "User not found");
        }

        var newToken = _tokenService.GenerateToken(dbUser);
        return new ApiResponse<string>(newToken, Constants.SUCCESS_MSG);
    }
}