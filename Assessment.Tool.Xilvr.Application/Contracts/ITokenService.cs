using Assessment.Tool.Xilvr.Domain.SharedKernel;
using System.Security.Claims;

namespace Assessment.Tool.Xilvr.Application.Contracts;

/// <summary>
/// Interface for token service.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Returns token for the given user
    /// </summary>
    public string GenerateToken(User user);

    /// <summary>
    /// Returns claims from token
    /// </summary>
    public ClaimsPrincipal GetPrincipalFromExpiredToken();

    /// <summary>
    /// Returns email from a valid token if user exists
    /// </summary>
    public string TryGetEmailFromToken();

    /// <summary>
    /// Returns token from authorisation header
    /// </summary>
    public string? GetTokenFromAuthorizationHeader();

    /// <summary>
    /// Returns otp
    /// </summary>
    public string GenerateOtp(int length);
}
