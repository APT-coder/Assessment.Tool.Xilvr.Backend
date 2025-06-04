using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentAnswers;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessments;

/// <summary>
/// Query class for UpdateScheduledAssessmentScoreCommand
/// </summary>
public class UpdateScheduledAssessmentScoreCommand : List<AnswerScoresDto>, IQuery<ApiResponse<bool>>
{
    public int ScheduledAssessmentId { get; set; }
}

/// <summary>
/// Handler class for UpdateScheduledAssessmentScoreCommand
/// </summary>
public class UpdateScheduledAssessmentScoreCommandHandler : IQueryHandler<UpdateScheduledAssessmentScoreCommand, ApiResponse<bool>>
{
    /// <summary>
    /// Application db context
    /// </summary>
    private readonly IApplicationDbContext _dbContext;

    /// <summary>
    /// Token Service
    /// </summary>
    private readonly ITokenService _tokenService;

    private readonly IScheduledAssessmentService _scheduledAssessmentService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateScheduledAssessmentScoreCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public UpdateScheduledAssessmentScoreCommandHandler(IApplicationDbContext dbContext, ITokenService tokenService,
        IScheduledAssessmentService scheduledAssessmentService)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));
        _dbContext = dbContext;
        Ensure.IsNotNull(tokenService, nameof(tokenService));
        _tokenService = tokenService;
        Ensure.IsNotNull(scheduledAssessmentService, nameof(scheduledAssessmentService));
        _scheduledAssessmentService = scheduledAssessmentService;
    }

    /// <summary>
    /// The handle method
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<bool>> Handle(UpdateScheduledAssessmentScoreCommand request, CancellationToken cancellationToken)
    {
        var scheduledAssessment = await _dbContext.ScheduledAssessments
            .FirstOrDefaultAsync(s => s.Id == request.ScheduledAssessmentId, cancellationToken);

        if (scheduledAssessment == null || scheduledAssessment.AssessmentStatus != Shared.Enum.AssessmentStatus.Completed)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, Constants.UPDATE_FAILED);
        }

        var employeeIds = request.Select(r => r.EmployeeId).Distinct().ToList();
        var questionIds = request.Select(r => r.QuestionId).Distinct().ToList();

        var answers = await _dbContext.ScheduledAssessmentsAnswers
            .Where(a => a.ScheduledAssessmentId == request.ScheduledAssessmentId &&
                        employeeIds.Contains(a.EmployeeId) &&
                        questionIds.Contains(a.QuestionId))
            .ToListAsync(cancellationToken);

        foreach (var scoreDto in request)
        {
            var answer = answers.FirstOrDefault(a =>
                a.EmployeeId == scoreDto.EmployeeId &&
                a.QuestionId == scoreDto.QuestionId);

            if (answer != null)
            {
                answer.Score = scoreDto.Score;
            }
        }

        try
        {
            await _scheduledAssessmentService.UpdateTotalScore(request.ScheduledAssessmentId, employeeIds, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, ex.Message);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
