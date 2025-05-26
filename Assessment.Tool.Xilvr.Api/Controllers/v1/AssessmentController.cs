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
}
