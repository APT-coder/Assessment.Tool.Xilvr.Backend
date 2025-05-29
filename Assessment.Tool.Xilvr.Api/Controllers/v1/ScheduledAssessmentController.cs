using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessments;
using Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessments;
using Assessment.Tool.Xilvr.Base.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assessment.Tool.Xilvr.Api.Controllers.v1;

/// <summary>
/// Defines the <see cref="ScheduledAssessmentController" />
/// </summary>
public class ScheduledAssessmentController : BaseController
{
    /// <summary>
    /// Method to schedule new assessment
    /// </summary>
    /// <returns></returns>
    [HttpPost("scheduled-assessments/schedule")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> ScheduleAssessment([FromBody] CreateScheduledAssessmentCommand createScheduledAssessmentCommand)
    {
        var result = await Mediator.Send(createScheduledAssessmentCommand);
        return Ok(result);
    }

    /// <summary>
    /// Method to get scheduled assessment by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("scheduled-assessments/{scheduledAssessmentId}")]
    [ProducesResponseType(typeof(ApiResponse<ScheduledAssessmentDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> GetScheduledAssessmentById([FromRoute] int scheduledAssessmentId)
    {
        var result = await Mediator.Send(new GetScheduledAssessmentByIdQuery { ScheduledAssessmentId = scheduledAssessmentId });
        return Ok(result);
    }

    /// <summary>
    /// Method to get scheduled assessment by employee id
    /// </summary>
    /// <returns></returns>
    [HttpGet("scheduled-assessments/for-employee")]
    [ProducesResponseType(typeof(ApiResponse<List<ScheduledAssessmentListDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> GetScheduledAssessmentsByEmployeeId([FromQuery] long? employeeId,
        [FromQuery] bool? forEmployee, [FromQuery] int skip, [FromQuery] int take)
    {
        var result = await Mediator.Send(new GetScheduledAssessmentsByEmployeeIdQuery
        {
            EmployeeId = employeeId,
            ForEmployee = forEmployee,
            Skip = skip,
            Take = take
        });
        return Ok(result);
    }
}
