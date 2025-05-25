using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Assessment.Tool.Xilvr.Shared.Constants;
using Assessment.Tool.Xilvr.Shared.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assessment.Tool.Xilvr.Application.Requests.Authentication;

/// <summary>
/// Query class for ExternalLoginCallBack
/// </summary>
public class ExternalLoginCallbackQuery : IQuery<ApiResponse<string>>
{
}

/// <summary>
/// handler for ExternalLoginCallbackQuery
/// </summary>
public class ExternalLoginCallbackQueryHandler : IQueryHandler<ExternalLoginCallbackQuery, ApiResponse<string>>
{
    /// <summary>
    /// Specifies IHttpContextAccessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token Service
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Constructor for ExternalLoginCallbackQueryHandler
    /// </summary>
    public ExternalLoginCallbackQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IApplicationDbContext dbContext,
        ITokenService tokenService)
    {
        Ensure.IsNotNull(httpContextAccessor, nameof(httpContextAccessor));
        _httpContextAccessor = httpContextAccessor;
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
    }

    /// <summary>
    /// handle method for ExternalLoginCallbackQuery
    /// </summary>
    public async Task<ApiResponse<string>> Handle(ExternalLoginCallbackQuery request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return new ApiResponse<string>(null, Constants.LOGIN_FAILED);

        var result = await httpContext.AuthenticateAsync("ExternalCookies");

        if (!result.Succeeded)
            return new ApiResponse<string>(null, Constants.LOGIN_FAILED);

        var externalId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var firstName = result.Principal.FindFirst(ClaimTypes.GivenName)?.Value;
        var lastName = result.Principal.FindFirst(ClaimTypes.Surname)?.Value;
        //var profileImageUrl = result.Principal.FindFirst("picture")?.Value;
        var provider = result.Properties.Items[".AuthScheme"];

        if (string.IsNullOrWhiteSpace(externalId) || string.IsNullOrWhiteSpace(provider))
            return new ApiResponse<string>(null, Constants.INVALID_CREDENTIAL);

        var profileImageUrl = await GetProfileImageUrl(result, provider, cancellationToken);

        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.User.UserProvider.ToString() == provider && e.User.ProviderId == externalId, cancellationToken);

        if (employee == null)
        {
            var userProvider = Enum.Parse<UserProvider>(provider);
            //user = User.CreateUser(null, null, null, null, new Email(email), Constants.SYSTEM, null, UserStatus.SetFrom(UserStatusValues.Pending),
            //    userProvider, externalId);

            employee = Employee.Create(profileImageUrl, firstName, lastName, email, [], null, Constants.SYSTEM,
                null, null, true, userProvider, externalId);

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        //else if (user.UserStatusId == (short)UserStatusValues.Pending)
        //{
        //    return new ApiResponse<string>(null, Constants.PENDING_USER);
        //}

        var token = _tokenService.GenerateToken(employee.User);
        return new ApiResponse<string>(token, Constants.SUCCESS_MSG);
    }

    private async Task<string> GetProfileImageUrl(AuthenticateResult result, string provider, CancellationToken cancellationToken)
    {
        var profileImageUrl = "";
        if (provider == "Google")
        {
            profileImageUrl = result.Principal.FindFirst("picture")?.Value;
        }
        else if (provider == "Microsoft")
        {
            var accessToken = result.Properties.GetTokenValue("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    var response = await client.GetAsync("https://graph.microsoft.com/v1.0/me/photo/$value", cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var imageBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
                        profileImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(imageBytes);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Microsoft Graph] Failed to fetch profile image: {ex.Message}");
                }
            }
        }
        return profileImageUrl;
    }
}
