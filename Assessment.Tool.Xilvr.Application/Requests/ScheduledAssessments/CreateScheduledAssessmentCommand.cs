using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Assessment.Tool.Xilvr.Shared.Enum;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessments;

/// <summary>
/// Query class for CreateScheduledAssessmentCommand
/// </summary>
public class CreateScheduledAssessmentCommand : IQuery<ApiResponse<bool>>
{
    public int BatchId { get; set; }

    public int AssessmentId { get; set; }

    public TimeSpan AssessmentDuration { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool CanRandomizeQuestion { get; set; }

    public bool CanDisplayResult { get; set; }

    public bool CanSubmitBeforeEnd { get; set; }

    public string? Link { get; set; } = default!;
}

/// <summary>
/// Handler class for CreateScheduledAssessmentCommand
/// </summary>
public class CreateScheduledAssessmentCommandHandler : IQueryHandler<CreateScheduledAssessmentCommand, ApiResponse<bool>>
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
    /// Initializes a new instance of the <see cref="CreateScheduledAssessmentCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public CreateScheduledAssessmentCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService)
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
    public async Task<ApiResponse<bool>> Handle(CreateScheduledAssessmentCommand request, CancellationToken cancellationToken)
    {
        var batch = await _dbContext.Batches
            .FirstOrDefaultAsync(b => b.Id == request.BatchId && b.IsActive == true, cancellationToken);

        var assessment = await _dbContext.Assessments
            .FirstOrDefaultAsync(a => a.Id == request.AssessmentId, cancellationToken);

        if (batch is null || assessment is null)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, Constants.NO_DATA);
        }

        if (request.EndDate < DateTime.UtcNow)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, "Invalid time");
        }

        var assessmentStatus = AssessmentStatus.Upcoming;
        var email = _tokenService.TryGetEmailFromToken();

        var scheduledAssessment = new ScheduledAssessment
        {
            Batch = batch,
            Assessment = assessment,
            AssessmentDuration = request.AssessmentDuration,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AssessmentStatus = assessmentStatus,
            CanRandomizeQuestion = request.CanRandomizeQuestion,
            CanDisplayResult = request.CanDisplayResult,
            CanSubmitBeforeEnd = request.CanSubmitBeforeEnd,
            Link = request.Link,
            CreatedBy = email,
            CreatedDateTime = DateTime.UtcNow
        };

        _dbContext.ScheduledAssessments.Add(scheduledAssessment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
