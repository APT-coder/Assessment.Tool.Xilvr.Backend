using Assessment.Tool.Xilvr.Application.Requests.RolesAndPermissions;
using Assessment.Tool.Xilvr.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

//
// Summary:
//     Represents an attribute that enforces permission checks on APIs. This attribute
//     implements asynchronous authorization filtering based on specified permissions.
//
//
// Remarks:
//     The permissions can be checked against various authentication schemes, and additional
//     permission checks can be configured as needed.
public class PermissionAttribute : Attribute, IAsyncAuthorizationFilter, IFilterMetadata
{
    private Guid _logIdentifier;

    private readonly string[] _permissions;

    private readonly IConfiguration _config;

    //
    // Summary:
    //     Initializes a new instance of the Bayada.Joy.Utilities.Application.Attributes.PermissionAttribute
    //     class with the specified permissions.
    //
    // Parameters:
    //   permissions:
    //     An array of permission strings that are required for the authorization.
    public PermissionAttribute(params string[] permissions)
    {
        _permissions = permissions;
    }

    //
    // Summary:
    //     Called early in the filter pipeline to confirm request is authorized.
    //
    // Parameters:
    //   context:
    //     The Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext.
    //
    // Returns:
    //     A System.Threading.Tasks.Task that on completion indicates the filter has executed.
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        _logIdentifier = Guid.NewGuid();
        HttpContext httpContext = context.HttpContext;
        ILogger<PermissionAttribute> logger = httpContext.RequestServices.GetRequiredService<ILogger<PermissionAttribute>>();
        logger.LogInformation("{LogIdentifier} - Permission attribute filter entered...", _logIdentifier);
        HttpRequest request = httpContext.Request;
        string text = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        logger.LogInformation("{LogIdentifier} - Requested Url: {Url}", _logIdentifier, text);
        logger.LogInformation("{LogIdentifier} - Decorated Permissions for the API are {@Permissions}", _logIdentifier, _permissions);

        string authHeader = request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            logger.LogWarning("{LogIdentifier} - Missing or invalid Authorization header", _logIdentifier);
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.HttpContext.Response.WriteAsync("Unauthorized: Missing or invalid token");
            return;
        }

        var tokenService = httpContext.RequestServices.GetRequiredService<TokenService>();
        string emailId = tokenService.TryGetEmailFromToken();

        if (emailId == null)
        {
            logger.LogWarning("{LogIdentifier} - Token is invalid or expired", _logIdentifier);
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.HttpContext.Response.WriteAsync("Unauthorized: Invalid or expired token");
            return;
        }

        logger.LogInformation("{LogIdentifier} - Token validated successfully. Email: {Email}", _logIdentifier, emailId);
        var mediator = httpContext.RequestServices.GetRequiredService<IMediator>();
        try
        {
            var userDetails = await mediator.Send(new GetUserRolesAndPermissionsByUserEmailQuery { Email = emailId });
            var userPermissionsList = userDetails.Data.Permissions.Select(p => p.ToLowerInvariant()).ToList();
            logger.LogInformation("{LogIdentifier} - System Permissions received for user are {@Permissions}", _logIdentifier, userPermissionsList);

            if (_permissions.Any(userPermissionsList.Contains))
            {
                logger.LogInformation("{LogIdentifier} - User has system permission to access.", _logIdentifier);
                return;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning("{LogIdentifier} - User does not exist", _logIdentifier);
            logger.LogInformation("{LogIdentifier} - User does not have the required permissions. Denying access.", _logIdentifier);
            context.Result = new ForbidResult();
        }
    }
}

