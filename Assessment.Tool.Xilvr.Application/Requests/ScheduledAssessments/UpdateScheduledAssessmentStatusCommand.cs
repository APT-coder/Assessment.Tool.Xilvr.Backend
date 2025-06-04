using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Assessment.Tool.Xilvr.Shared.Enum;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessments;

/// <summary>
/// Query class for UpdateScheduledAssessmentStatusCommand
/// </summary>
public class UpdateScheduledAssessmentStatusCommand : IQuery<ApiResponse<bool>>
{
    public int ScheduledAssessmentId { get; set; }

    public AssessmentStatus AssessmentStatus { get; set; }
}

/// <summary>
/// Handler class for UpdateScheduledAssessmentStatusCommand
/// </summary>
public class UpdateScheduledAssessmentStatusCommandHandler : IQueryHandler<UpdateScheduledAssessmentStatusCommand, ApiResponse<bool>>
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
    /// Initializes a new instance of the <see cref="UpdateScheduledAssessmentStatusCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public UpdateScheduledAssessmentStatusCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService)
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
    public async Task<ApiResponse<bool>> Handle(UpdateScheduledAssessmentStatusCommand request, CancellationToken cancellationToken)
    {
        var scheduledAssessment = await _dbContext.ScheduledAssessments
            .FirstOrDefaultAsync(s => s.Id == request.ScheduledAssessmentId, cancellationToken);

        if (scheduledAssessment == null)
            throw new XilvrException(ExceptionCode.UnprocessableEntity, Constants.UPDATE_FAILED);

        scheduledAssessment.AssessmentStatus = request.AssessmentStatus;
        scheduledAssessment.UpdatedBy = _tokenService.TryGetEmailFromToken();
        scheduledAssessment.UpdatedDateTime = DateTime.UtcNow;

        _dbContext.ScheduledAssessments.Update(scheduledAssessment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
