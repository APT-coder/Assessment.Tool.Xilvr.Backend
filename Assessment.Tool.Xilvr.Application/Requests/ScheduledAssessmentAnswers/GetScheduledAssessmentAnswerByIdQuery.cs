using Assessment.Tool.Xilvr.Application.Contracts;
using Assessment.Tool.Xilvr.Application.Dtos.Assessments;
using Assessment.Tool.Xilvr.Application.Dtos.ScheduledAssessmentAnswers;
using Assessment.Tool.Xilvr.Base;
using Assessment.Tool.Xilvr.Base.CQRS;
using Assessment.Tool.Xilvr.Base.Helpers;
using Assessment.Tool.Xilvr.Base.Models;
using Assessment.Tool.Xilvr.Base.Shared.Exceptions;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Application.Requests.ScheduledAssessmentAnswers;

/// <summary>
/// Query class for GetScheduledAssessmentAnswerByIdQuery
/// </summary>
public class GetScheduledAssessmentAnswerByIdQuery : IQuery<ApiResponse<ScheduledAssessmentAnswerDetailsDto>>
{
    public int ScheduledAssessmentId { get; set; }

    public long EmployeeId { get; set; }
}

/// <summary>
/// Handler class for GetScheduledAssessmentAnswerByIdQuery
/// </summary>
public class GetScheduledAssessmentAnswerByIdQueryHandler : IQueryHandler<GetScheduledAssessmentAnswerByIdQuery, ApiResponse<ScheduledAssessmentAnswerDetailsDto>>
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
    /// Initializes a new instance of the <see cref="GetScheduledAssessmentAnswerByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The user dbcontext instance.</param>
    public GetScheduledAssessmentAnswerByIdQueryHandler(IApplicationDbContext dbContext, ITokenService tokenService)
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
    public async Task<ApiResponse<ScheduledAssessmentAnswerDetailsDto>> Handle(GetScheduledAssessmentAnswerByIdQuery request, CancellationToken cancellationToken)
    {
        var scheduledAssessment = await _dbContext.ScheduledAssessments
            .FirstOrDefaultAsync(s => s.Id == request.ScheduledAssessmentId, cancellationToken);

        var employee = await _dbContext.Employees
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);

        if (scheduledAssessment == null || employee == null)
        {
            throw new XilvrException(ExceptionCode.BadRequest, Constants.NO_DATA);
        }
        if (scheduledAssessment.AssessmentStatus != Shared.Enum.AssessmentStatus.Completed)
        {
            throw new XilvrException(ExceptionCode.UnprocessableEntity, "Assessment not completed");
        }

        var scheduledAssessmentAnswer = await _dbContext.ScheduledAssessmentsAnswers
            .Include(s => s.Question)
            .Where(s => s.ScheduledAssessmentId == request.ScheduledAssessmentId &&
                s.EmployeeId == request.EmployeeId)
            .ToListAsync(cancellationToken);

        var answersDto = scheduledAssessmentAnswer.Select(answer => new QuestionAnswerDto
        {
            QuestionId = answer.Question.Id,
            ScheduledAssessmentAnswerId = answer.Id,
            Text = answer.Question.Text,
            QuestionType = answer.Question.QuestionType,
            Options = answer.Question.Options,
            Answer = answer.Question.Answer,
            Points = answer.Question.Points,
            MarkedAnswer = answer.Answer ?? string.Empty,
            IsCorrect = answer.IsCorrect,
            Score = answer.Score
        }).ToList();

        var result = new ScheduledAssessmentAnswerDetailsDto
        {
            ScheduledAssessmentId = scheduledAssessment.Id,
            EmployeeId = employee.Id,
            EmployeeName = $"{employee.User.FirstName} {employee.User.LastName}",
            Answers = answersDto
        };

        return new ApiResponse<ScheduledAssessmentAnswerDetailsDto>(result, Constants.SUCCESS_MSG);
    }
}