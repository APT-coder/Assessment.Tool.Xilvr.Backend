using Assessment.Tool.Xilvr.Application.Dtos.RolesAndPermissions;
using Assessment.Tool.Xilvr.Application.Requests.RolesAndPermissions;
using Assessment.Tool.Xilvr.Base.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assessment.Tool.Xilvr.Api.Controllers.v1;

/// <summary>
/// Defines the <see cref="RolesAndPermissionsController" />
/// </summary>
public class RolesAndPermissionsController : BaseController
{
    /// <summary>
    /// Method to get user roles and permissions
    /// </summary>
    /// <returns></returns>
    [HttpGet("role-permissions/{email}")]
    [ProducesResponseType(typeof(ApiResponse<UserRolePermissionDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserRolePermissions([FromRoute] string email)
    {
        var result = await Mediator.Send(new GetUserRolesAndPermissionsByUserEmailQuery { Email = email });
        return Ok(result);
    }

    /// <summary>
    /// Method to update user roles
    /// </summary>
    /// <returns></returns>
    [HttpPut("role-permissions/{userId}/roles")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateUserRoles([FromRoute] long userId,
        [FromQuery] List<string> roles)
    {
        var result = await Mediator.Send(new UpdateUserRoleCommand { UserId = userId, Roles = roles });
        return Ok(result);
    }
}
