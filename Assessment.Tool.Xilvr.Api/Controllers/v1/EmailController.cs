using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.EmailConnector.Contracts;
using Assessment.Tool.Xilvr.EmailConnector.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assessment.Tool.Xilvr.Api.Controllers.v1;

/// <summary>
/// Defines the <see cref="EmailController" />
/// </summary>
public class EmailController(IEmailService emailService) : BaseController
{
    private readonly IEmailService _emailService = emailService;

    /// <summary>
    /// Method to send an email
    /// </summary>
    /// <param name="request">Email request data</param>
    /// <returns>API response indicating success or failure</returns>
    [HttpPost("send")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> SendEmail([FromBody] List<SendEmailDto> requests)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<bool>(false, "Invalid request data"));

        try
        {
            foreach (var request in requests)
            {
                var response = await _emailService.SendEmailAsync(
                    request.ToEmail,
                    request.Subject,
                    request.Body,
                    request.Attachments
                );
            }
            return Ok(new ApiResponse<bool>(true, $"{requests.Count} emails sent successfully"));
        }
        catch (Exception ex)
        {
            throw new XilvrException(ExceptionCode.ServiceUnavailable, ex.Message);
        }
    }
}