using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentAnswers;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessmentAnswers;

/// <summary>
/// Query class for SaveScheduledAssessmentAnswerCommand
/// </summary>
public class SaveScheduledAssessmentAnswerCommand : List<ScheduledAssessmentAnswerDto>, IQuery<ApiResponse<bool>>
{
}

/// <summary>
/// Handler class for SaveScheduledAssessmentAnswerCommand
/// </summary>
public class SaveScheduledAssessmentAnswerCommandHandler : IQueryHandler<SaveScheduledAssessmentAnswerCommand, ApiResponse<bool>>
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
    /// Initializes a new instance of the <see cref="SaveScheduledAssessmentAnswerCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public SaveScheduledAssessmentAnswerCommandHandler(IApplicationDbContext dbContext,
        ITokenService tokenService, IScheduledAssessmentService scheduledAssessmentService)
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
    public async Task<ApiResponse<bool>> Handle(SaveScheduledAssessmentAnswerCommand request, CancellationToken cancellationToken)
    {
        if (request == null || request.Count == 0)
        {
            throw new XilvrException(ExceptionCode.BadRequest, Constants.NO_DATA);
        }

        foreach (var item in request)
        {
            var scheduledAssessmentAnswer = new ScheduledAssessmentAnswer();

            var scheduledAssessment = await _dbContext.ScheduledAssessments
                .FirstOrDefaultAsync(s => s.Id == item.ScheduledAssessmentId, cancellationToken);

            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == item.EmployeeId, cancellationToken);

            var question = await _dbContext.Questions
                .FirstOrDefaultAsync(q => q.Id == item.QuestionId, cancellationToken);

            if (scheduledAssessment is null || employee is null || question is null)
            {
                throw new XilvrException(ExceptionCode.UnprocessableEntity, Constants.CREATE_FAILED);
            }

            scheduledAssessmentAnswer.ScheduledAssessment = scheduledAssessment;
            scheduledAssessmentAnswer.Employee = employee;
            scheduledAssessmentAnswer.Question = question;

            if (question.QuestionType != Shared.Enum.QuestionType.Descriptive)
            {
                scheduledAssessmentAnswer = _scheduledAssessmentService.CalculateScoreForAnswer(scheduledAssessmentAnswer);
            }

            _dbContext.ScheduledAssessmentsAnswers.Add(scheduledAssessmentAnswer);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<bool>(true, Constants.SUCCESS_MSG);
    }
}
