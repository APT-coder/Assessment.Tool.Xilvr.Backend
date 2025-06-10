using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.AspNetCore.Http;
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
    /// Employee repository
    /// </summary>
    private readonly IEmployeeRepository _employeeRepository;

    /// <summary>
    /// Http context accessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLoginCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public UserLoginCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IMemoryCache memoryCache, IEmployeeRepository employeeRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(memoryCache, nameof(memoryCache));
        _cache = memoryCache;
        Ensure.IsNotNull(employeeRepository, nameof(employeeRepository));
        _employeeRepository = employeeRepository;
        Ensure.IsNotNull(httpContextAccessor, nameof(httpContextAccessor));
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// The hande method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<string>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByEmailAsync(request.Email, cancellationToken);

        if (employee == null || employee.User.UserStatusId == (short)UserStatusValues.InActive)
        {
            throw new XilvrException(ExceptionCode.NotFound, Constants.NO_DATA);
        }
        else if (employee.User.UserStatusId == (short)UserStatusValues.PendingProfileCompletion)
        {
            return new ApiResponse<string>(null, Constants.PENDING_USER);
        }
        else if (employee.User.UserStatusId == (short)UserStatusValues.PendingApproval)
        {
            return new ApiResponse<string>(null, Constants.WAITING_APPROVAL);
        }

        if (request.UseOtp)
        {
            if (string.IsNullOrWhiteSpace(request.OtpCode))
            {
                throw new XilvrException(ExceptionCode.BadRequest, "OTP code is required.");
            }

            var cacheKey = $"OTP_{employee.User.Email.EmailId}";
            if (!_cache.TryGetValue(cacheKey, out string cachedOtp) || cachedOtp != request.OtpCode)
            {
                throw new XilvrException(ExceptionCode.UnauthorizedAccess, "Invalid or expired OTP.");
            }
            _cache.Remove(cacheKey);
        }
        else
        {
            if (!BCrypt.Net.BCrypt.Verify(request.Password, employee.User.Password))
            {
                throw new XilvrException(ExceptionCode.UnauthorizedAccess, Constants.INVALID_CREDENTIAL);
            }
            else if (!employee.IsActive &&
                employee.User.LasttPasswordReset < DateTime.UtcNow.AddMonths(-3))
            {
                throw new XilvrException(ExceptionCode.PreconditionFailed, Constants.LOGIN_FAILED);
            }
        }
        var token = _tokenService.GenerateToken(employee.User);
        var httpContext = _httpContextAccessor.HttpContext;
        await _tokenService.SignInUserWithCookie(request.Email, employee, null, httpContext);
        return new ApiResponse<string>(token, Constants.SUCCESS_MSG);
    }
}
