using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessments;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessments;

/// <summary>
/// Query class for GetScheduledAssessmentByIdQuery
/// </summary>
public class GetScheduledAssessmentByIdQuery : IQuery<ApiResponse<ScheduledAssessmentDto>>
{
    public int ScheduledAssessmentId { get; set; }
}

/// <summary>
/// Handler class for GetScheduledAssessmentByIdQuery
/// </summary>
public class GetScheduledAssessmentByIdQueryHandler : IQueryHandler<GetScheduledAssessmentByIdQuery, ApiResponse<ScheduledAssessmentDto>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token Service
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetScheduledAssessmentByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetScheduledAssessmentByIdQueryHandler(IApplicationDbContext dbContext, ITokenService tokenService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<ScheduledAssessmentDto>> Handle(GetScheduledAssessmentByIdQuery request, CancellationToken cancellationToken)
    {
        var scheduledAssessment = await _dbContext.ScheduledAssessments
            .FirstOrDefaultAsync(s => s.Id == request.ScheduledAssessmentId, cancellationToken);

        if (scheduledAssessment == null)
        {
            throw new XilvrException(ExceptionCode.BadRequest, Constants.NO_DATA);
        }

        var response = new ScheduledAssessmentDto
        {
            BatchId = scheduledAssessment.BatchId,
            AssessmentId = scheduledAssessment.AssessmentId,
            AssessmentDuration = scheduledAssessment.AssessmentDuration,
            StartDate = scheduledAssessment.StartDate,
            EndDate = scheduledAssessment.EndDate,
            CanRandomizeQuestion = scheduledAssessment.CanRandomizeQuestion,
            CanDisplayResult = scheduledAssessment.CanDisplayResult,
            CanSubmitBeforeEnd = scheduledAssessment.CanSubmitBeforeEnd,
            Link = scheduledAssessment.Link,
        };

        return new ApiResponse<ScheduledAssessmentDto>(response, Constants.SUCCESS_MSG);
    }
}