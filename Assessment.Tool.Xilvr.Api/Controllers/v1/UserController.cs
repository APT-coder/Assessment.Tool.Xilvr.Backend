using Assessment.Tool.Xilvr.Application.Dtos.Users;
using Assessment.Tool.Xilvr.Application.Requests.Users;
using Assessment.Tool.Xilvr.Base.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assessment.Tool.Xilvr.Api.Controllers.v1;

/// <summary>
/// Defines the <see cref="UserController" />
/// </summary>
public class UserController : BaseController
{
    /// <summary>
    /// Method to get user info by email id
    /// </summary>
    ///<param name="emailId"></param>
    /// <returns></returns>
    [HttpGet("users")]
    [ProducesResponseType(typeof(ApiResponse<UserInfoDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetUserInfoByEmailId([FromQuery] string? emailId)
    {
        var result = await Mediator.Send(new GetUserInfoQuery { Email = emailId });
        return Ok(result);
    }
}
