using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Assessment.Tool.Xilvr.Application.Requests.Authentication;

/// <summary>
/// Command for login
/// </summary>
public class UserLoginCommand : IQuery<ApiResponse<string>>
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool UseOtp { get; set; }

    public string? OtpCode { get; set; }
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
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Memory cache
    /// </summary>
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLoginCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public UserLoginCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IMemoryCache memoryCache)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(memoryCache, nameof(memoryCache));
        _cache = memoryCache;
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

        if (user == null)
        {
            throw new XilvrException(ExceptionCode.UnauthorizedAccess, Constants.INVALID_CREDENTIAL);
        }

        if (request.UseOtp)
        {
            if (string.IsNullOrWhiteSpace(request.OtpCode))
            {
                throw new XilvrException(ExceptionCode.BadRequest, "OTP code is required.");
            }

            var cacheKey = $"OTP_{user.Email.EmailId}";
            if (!_cache.TryGetValue(cacheKey, out string cachedOtp) || cachedOtp != request.OtpCode)
            {
                throw new XilvrException(ExceptionCode.UnauthorizedAccess, "Invalid or expired OTP.");
            }
            _cache.Remove(cacheKey);
        }
        else
        {
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                throw new XilvrException(ExceptionCode.UnauthorizedAccess, Constants.INVALID_CREDENTIAL);
            }
        }
        var token = _tokenService.GenerateToken(user);
        return new ApiResponse<string>(token, Constants.SUCCESS_MSG);
    }
}
