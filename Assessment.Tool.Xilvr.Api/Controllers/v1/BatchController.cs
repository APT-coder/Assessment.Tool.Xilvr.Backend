using Assessment.Tool.Xilvr.Application.Dtos.Batches;
using Assessment.Tool.Xilvr.Application.Requests.Batches;
using Assessment.Tool.Xilvr.Base.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assessment.Tool.Xilvr.Api.Controllers.v1;

/// <summary>
/// Defines the <see cref="BatchController" />
/// </summary>
public class BatchController : BaseController
{
    /// <summary>
    /// Method to get all batches
    /// </summary>
    /// <returns></returns>
    [HttpGet("batches")]
    [ProducesResponseType(typeof(ApiResponse<BatchDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> GetAllBatches([FromQuery] string? isActive, [FromQuery] DateOnly? year, [FromQuery] string? search)
    {
        var result = await Mediator.Send(new GetAllBatchesQuery { IsActive = isActive, Year = year, Search = search });
        return Ok(result);
    }

    /// <summary>
    /// Method to create new batch
    /// </summary>
    /// <returns></returns>
    [HttpPost("batches")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> CreateBatch([FromBody] CreateBatchCommand createBatchCommand)
    {
        var result = await Mediator.Send(createBatchCommand);
        return Ok(result);
    }

    /// <summary>
    /// Method to update batch by id
    /// </summary>
    /// <returns></returns>
    [HttpPut("batches/{batchId}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Permission(Permissions.VIEW_CANDIDATE_DETAILS,
    //    Permissions.CANDIDATE_LISTING_VIEW)]
    public async Task<IActionResult> UpdateBatchById([FromRoute] int batchId, [FromQuery] string? batchName, [FromQuery] bool? isActive)
    {
        var result = await Mediator.Send(new UpdateBatchByIdCommand { Id = batchId, Name = batchName, IsActive = isActive });
        return Ok(result);
    }
}
