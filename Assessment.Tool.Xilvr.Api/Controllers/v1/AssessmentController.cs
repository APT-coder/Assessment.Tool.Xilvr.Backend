using Assessment.Tool.Xilvr.Application.Dtos.Assessments;
using Assessment.Tool.Xilvr.Application.Requests.Assessments;
using Assessment.Tool.Xilvr.Base.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assessment.Tool.Xilvr.Api.Controllers.v1;

/// <summary>
/// Defines the <see cref="AssessmentController" />
/// </summary>
public class AssessmentController : BaseController
{
    /// <summary>
    /// Method to create new assessment
    /// </summary>
    /// <returns></returns>
    [HttpPost("assessments/create")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> CreateAssessment([FromBody] CreateAssessmentCommand createAssessmentCommand)
    {
        var result = await Mediator.Send(createAssessmentCommand);
        return Ok(result);
    }

    /// <summary>
    /// Method to add question
    /// </summary>
    /// <returns></returns>
    [HttpPut("assessments/{assessmentId}/add-question")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> AddQuestion([FromRoute] int assessmentId, [FromBody] AddQuestionToAssessmentByIdCommand addQuestionToAssessmentByIdCommand)
    {
        var result = await Mediator.Send(addQuestionToAssessmentByIdCommand);
        return Ok(result);
    }

    /// <summary>
    /// Method to get all assessments
    /// </summary>
    /// <returns></returns>
    [HttpGet("assessments")]
    [ProducesResponseType(typeof(ApiResponse<List<AssessmentListDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> GetAllAssessments([FromQuery] string? name, [FromQuery] int skip, [FromQuery] int take)
    {
        var result = await Mediator.Send(new GetAllAssessmentsQuery { Name = name, Skip = skip, Take = take });
        return Ok(result);
    }

    /// <summary>
    /// Method to get assessment details by assessment id
    /// </summary>
    /// <returns></returns>
    [HttpGet("assessments/{assessmentId}/questions")]
    [ProducesResponseType(typeof(ApiResponse<AssessmentDetailsDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> GetAssessmentDetails([FromRoute] int assessmentId)
    {
        var result = await Mediator.Send(new GetAssessmentQuestionsByIdQuery { Id = assessmentId });
        return Ok(result);
    }
}
