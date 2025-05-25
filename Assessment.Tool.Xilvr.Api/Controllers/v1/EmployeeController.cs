using Assessment.Tool.Xilvr.Application.Requests.Employees;
using Assessment.Tool.Xilvr.Base.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assessment.Tool.Xilvr.Api.Controllers.v1;

/// <summary>
/// Defines the <see cref="EmployeeController" />
/// </summary>
public class EmployeeController : BaseController
{
    /// <summary>
    /// Method to update employee details
    /// </summary>
    /// <returns></returns>
    [HttpPut("employees")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeCommand updateEmployeeCommand)
    {
        var result = await Mediator.Send(updateEmployeeCommand);
        return Ok(result);
    }

    /// <summary>
    /// Patch method to update employee IsActive and UserStatusId
    /// </summary>
    [HttpPatch("employees/{employeeId}/status")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateEmployeeStatus([FromRoute] long employeeId, [FromBody] UpdateEmployeeStatusCommand command)
    {
        var updateEmployeeStatusCommand = new UpdateEmployeeStatusCommand
        {
            EmployeeId = employeeId,
            IsActive = command.IsActive,
            UserStatusId = command.UserStatusId,
        };
        var result = await Mediator.Send(updateEmployeeStatusCommand);
        return Ok(result);
    }
}
