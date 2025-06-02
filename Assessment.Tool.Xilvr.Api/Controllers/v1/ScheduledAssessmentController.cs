using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentAnswers;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessments;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentScores;
using Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessmentAnswers;
using Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessments;
using Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessmentScores;
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

    /// <summary>
    /// Method to update scheduled assessment by id
    /// </summary>
    /// <returns></returns>
    [HttpPut("scheduled-assessments/{scheduledAssessmentId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> UpdateScheduleAssessmentCommand([FromRoute] int scheduledAssessmentId,
        [FromBody] ScheduledAssessmentDto scheduledAssessmentDto)
    {
        var result = await Mediator.Send(new UpdateScheduledAssessmentCommand
        {
            ScheduledAssessmentId = scheduledAssessmentId,
            BatchId = scheduledAssessmentDto.BatchId,
            AssessmentId = scheduledAssessmentDto.AssessmentId,
            AssessmentDuration = scheduledAssessmentDto.AssessmentDuration,
            StartDate = scheduledAssessmentDto.StartDate,
            EndDate = scheduledAssessmentDto.EndDate,
            CanDisplayResult = scheduledAssessmentDto.CanDisplayResult,
            CanRandomizeQuestion = scheduledAssessmentDto.CanRandomizeQuestion,
            CanSubmitBeforeEnd = scheduledAssessmentDto.CanSubmitBeforeEnd,
            Link = scheduledAssessmentDto.Link,
        });
        return Ok(result);
    }

    /// <summary>
    /// Method to update scheduled assessment by id
    /// </summary>
    /// <returns></returns>
    [HttpPost("scheduled-assessments/scheduled-assessment-answer")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> SaveScheduledAssessmentAnswer([FromBody] SaveScheduledAssessmentAnswerCommand saveScheduledAssessmentAnswerCommand)
    {
        var result = await Mediator.Send(saveScheduledAssessmentAnswerCommand);
        return Ok(result);
    }

    /// <summary>
    /// Method to get scheduled assessment answers by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("scheduled-assessments/{scheduledAssessmentId}/scheduled-assessment-answer/{employeeId}")]
    [ProducesResponseType(typeof(ApiResponse<ScheduledAssessmentAnswerDetailsDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> GetScheduledAssessmentAnswers([FromRoute] int scheduledAssessmentId, [FromRoute] long employeeId)
    {
        var result = await Mediator.Send(new GetScheduledAssessmentAnswerByIdQuery
        {
            ScheduledAssessmentId = scheduledAssessmentId,
            EmployeeId = employeeId
        });
        return Ok(result);
    }

    /// <summary>
    /// Method to get scheduled assessment attendance with score
    /// </summary>
    /// <returns></returns>
    [HttpGet("scheduled-assessments/{scheduledAssessmentId}/attendance")]
    [ProducesResponseType(typeof(ApiResponse<ScheduledAssessmentScoreDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> GetScheduledAssessmentAttendance([FromRoute] int scheduledAssessmentId)
    {
        var result = await Mediator.Send(new GetScheduledAssessmentScoreByIdQuery
        {
            ScheduledAssessmentId = scheduledAssessmentId,
        });
        return Ok(result);
    }
}
