using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Assessment.Tool.Xilvr.EmailConnector.Contracts;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Assessment.Tool.Xilvr.Application.Requests.Authentication;

/// <summary>
/// Query class for generating otp
/// </summary>
public class GenerateOtpCommand : IQuery<ApiResponse<string>>
{
    public string? Email { get; set; }

    public int Length { get; set; }
}

/// <summary>
/// Handler class for GenerateOtpCommand
/// </summary>
public class GenerateOtpCommandHandler : IQueryHandler<GenerateOtpCommand, ApiResponse<string>>
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
    /// Memory cache
    /// </summary>
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Email service
    /// </summary>
    private readonly IEmailService _emailService;

    /// <summary>
    /// Constructor for GenerateOtpCommandHandler
    /// </summary>
    public GenerateOtpCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IMemoryCache cache, IEmailService emailService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(cache, nameof(cache));
        _cache = cache;
        Ensure.IsNotNull(emailService, nameof(emailService));
        _emailService = emailService;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    public async Task<ApiResponse<string>> Handle(GenerateOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserStatus)
            .FirstOrDefaultAsync(u => u.Email.EmailId == request.Email, cancellationToken);

        if (user == null || user.UserStatus.Status == UserStatusValues.InActive)
        {
            throw new XilvrException(ExceptionCode.NotFound, Constants.NO_DATA);
        }

        var otp = _tokenService.GenerateOtp(request.Length);

        var cacheKey = $"OTP_{user.Email.EmailId}";
        _cache.Set(cacheKey, otp, TimeSpan.FromMinutes(5));

        try
        {
            await _emailService.SendEmailAsync(request.Email, "OTP for Password Reset - Xilvr App", $"Your OTP code is: {otp}");
        }
        catch (Exception ex)
        {
            throw new XilvrException(ExceptionCode.ServiceUnavailable, ex.Message);
        }

        return new ApiResponse<string>("OTP sent successfully.", Constants.SUCCESS_MSG);
    }
}