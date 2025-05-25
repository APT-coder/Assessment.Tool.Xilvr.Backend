using Assessment.Tool.Xilvr.Application.Requests.Authentication;
using Assessment.Tool.Xilvr.Base.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assessment.Tool.Xilvr.Api.Controllers.v1;

/// <summary>
/// Defines the <see cref="AuthController" />
/// </summary>
public class AuthController : BaseController
{
    /// <summary>
    /// Authenticates a user by username and password and returns a JWT token.
    /// </summary>
    ///<param name="UserLoginCommand"></param>
    /// <returns></returns>
    [HttpPost("auth/login")]
    [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Login([FromBody] UserLoginCommand userLoginCommand)
    {
        var result = await Mediator.Send(userLoginCommand);
        return Ok(result);
    }

    /// <summary>
    /// Initiates the external login challenge.
    /// </summary>
    /// <param name="provider">The external login provider (e.g., Google, Microsoft).</param>
    /// <returns>Challenge result to redirect to external provider.</returns>
    [HttpGet("auth/external-login")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Redirect)]
    public IActionResult ExternalLogin([FromQuery] string provider)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback));
        var props = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(props, provider);
    }

    /// <summary>
    /// Handles the external login callback and issues a JWT token if successful.
    /// </summary>
    /// <returns>JWT token in ApiResponse.</returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("auth/external-login-callback")]
    [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> ExternalLoginCallback()
    {
        var result = await Mediator.Send(new ExternalLoginCallbackQuery());
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to refresh JWT token using expired token
    /// </summary>
    /// <param name="dto">TokenRequestDto containing expired token</param>
    /// <returns>New JWT token</returns>
    [HttpPost("auth/refresh-token")]
    [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand refreshTokenCommand)
    {
        var result = await Mediator.Send(refreshTokenCommand);
        return Ok(result);
    }

    /// <summary>
    /// Endpoint to generate otp
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("auth/generate-otp/{email}")]
    [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GenerateOtp([FromRoute] string email, [FromQuery] int length)
    {
        var command = new GenerateOtpCommand
        {
            Email = email,
            Length = length
        };
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}
