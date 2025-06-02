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
/// Query class for UpdateScheduledAssessmentCommand
/// </summary>
public class UpdateScheduledAssessmentCommand : ScheduledAssessmentDto, IQuery<ApiResponse<bool>>
{
    public int ScheduledAssessmentId { get; set; }
}

/// <summary>
/// Handler class for UpdateScheduledAssessmentCommand
/// </summary>
public class UpdateScheduledAssessmentCommandHandler : IQueryHandler<UpdateScheduledAssessmentCommand, ApiResponse<bool>>
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
    /// Initializes a new instance of the <see cref="UpdateScheduledAssessmentCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public UpdateScheduledAssessmentCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService)
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
    public async Task<ApiResponse<bool>> Handle(UpdateScheduledAssessmentCommand request, CancellationToken cancellationToken)
    {
        var scheduledAssessment = await _dbContext.ScheduledAssessments
            .FirstOrDefaultAsync(s => s.Id == request.ScheduledAssessmentId, cancellationToken);

        if (scheduledAssessment == null)
            throw new XilvrException(ExceptionCode.UnprocessableEntity, Constants.UPDATE_FAILED);

        if (request.BatchId is int batchId)
        {
            if (!await _dbContext.Batches.AnyAsync(b => b.Id == batchId, cancellationToken))
                throw new XilvrException(ExceptionCode.NotFound, "Batch not found");
            scheduledAssessment.BatchId = batchId;
        }

        if (request.AssessmentId is int assessmentId)
        {
            if (!await _dbContext.Assessments.AnyAsync(a => a.Id == assessmentId, cancellationToken))
                throw new XilvrException(ExceptionCode.NotFound, "Assessment not found");
            scheduledAssessment.AssessmentId = assessmentId;
        }

        var email = _tokenService.TryGetEmailFromToken();

        scheduledAssessment.AssessmentDuration = request.AssessmentDuration ?? scheduledAssessment.AssessmentDuration;
        scheduledAssessment.StartDate = request.StartDate ?? scheduledAssessment.StartDate;
        scheduledAssessment.EndDate = request.EndDate ?? scheduledAssessment.EndDate;
        scheduledAssessment.CanRandomizeQuestion = request.CanRandomizeQuestion ?? scheduledAssessment.CanRandomizeQuestion;
        scheduledAssessment.CanDisplayResult = request.CanDisplayResult ?? scheduledAssessment.CanDisplayResult;
        scheduledAssessment.CanSubmitBeforeEnd = request.CanSubmitBeforeEnd ?? scheduledAssessment.CanSubmitBeforeEnd;
        scheduledAssessment.Link = !string.IsNullOrWhiteSpace(request.Link) ? request.Link : scheduledAssessment.Link;

        scheduledAssessment.UpdatedBy = email;
        scheduledAssessment.UpdatedDateTime = DateTime.UtcNow;

        _dbContext.ScheduledAssessments.Update(scheduledAssessment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
